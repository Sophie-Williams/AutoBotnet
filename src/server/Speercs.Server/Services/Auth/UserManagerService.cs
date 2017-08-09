using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Models.User;
using Speercs.Server.Utilities;
using System;
using System.Collections;
using System.Security;
using System.Threading.Tasks;
using Speercs.Server.Services.Game;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Program;

namespace Speercs.Server.Services.Auth
{
    public class UserManagerService : DependencyObject
    {
        public const string RegisteredUsersKey = "r_users";
        private LiteCollection<RegisteredUser> userCollection;

        public UserManagerService(ISContext serverContext) : base(serverContext)
        {
            userCollection = serverContext.Database.GetCollection<RegisteredUser>(RegisteredUsersKey);
        }

        public async Task<RegisteredUser> RegisterUserAsync(UserRegistrationRequest regRequest)
        {
            if (await FindUserByUsernameAsync(regRequest.Username) != null) throw new SecurityException("A user with the same username already exists!");
            return await Task.Run(async () =>
            {
                // Calculate cryptographic info
                var cryptoConf = PasswordCryptoConfiguration.CreateDefault();
                var cryptoHelper = new AuthCryptoHelper(cryptoConf);
                var pwSalt = cryptoHelper.GenerateSalt();
                var encryptedPassword =
                    cryptoHelper.CalculateUserPasswordHash(regRequest.Password, pwSalt);
                // Create user
                var user = new RegisteredUser
                {
                    Identifier = Guid.NewGuid().ToString(),
                    Username = regRequest.Username,
                    Email = regRequest.Email,
                    ApiKey = StringUtils.SecureRandomString(AuthCryptoHelper.DefaultApiKeyLength),
                    Crypto = new ItemCrypto
                    {
                        Salt = pwSalt,
                        Conf = cryptoConf,
                        Key = encryptedPassword
                    },
                    Enabled = true,
                    AnalyticsEnabled = false,
                };

                // Add the user to the database
                userCollection.Insert(user);

                // Index database
                userCollection.EnsureIndex(x => x.Identifier);
                userCollection.EnsureIndex(x => x.ApiKey);
                userCollection.EnsureIndex(x => x.Username);

                // Create additional data containers
                // create team data
                ServerContext.AppState.PlayerData[user.Identifier] = new UserTeam
                {
                    UserIdentifier = user.Identifier
                };

                // create persistent data
                var persistentDataService = new PlayerPersistentDataService(ServerContext);
                await persistentDataService.CreatePersistentDataAsync(user.Identifier);

                ServerContext.AppState.UserAnalyticsData[user.Identifier] = new UserAnalytics();

                return user;
            });
        }

        public async Task<RegisteredUser> FindUserByApiKeyAsync(string apikey)
        {
            return await Task.Run(() => (userCollection.FindOne(x => x.ApiKey == apikey)));
        }

        public async Task<RegisteredUser> FindUserByUsernameAsync(string username)
        {
            return await Task.Run(() => (userCollection.FindOne(x => x.Username == username)));
        }

        public async Task<RegisteredUser> FindUserByIdentifierAsync(string id)
        {
            return await Task.Run(() => (userCollection.FindOne(x => x.Identifier == id)));
        }

        /// <summary>
        /// Warning: Callers are expected to use their own locks before calling this method!
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserInDatabaseAsync(RegisteredUser user)
        {
            return await Task.Run(() => userCollection.Update(user));
        }

        public async Task<bool> CheckPasswordAsync(string password, RegisteredUser user)
        {
            var ret = false;
            var lockEntry = ServerContext.ServiceTable.GetOrCreate(user.Username).UserLock;
            await lockEntry.WithConcurrentReadAsync(Task.Run(() =>
            {
                //Calculate hash and compare
                var cryptoHelper = new AuthCryptoHelper(user.Crypto.Conf);
                var pwKey =
                    cryptoHelper.CalculateUserPasswordHash(password, user.Crypto.Salt);
                ret = StructuralComparisons.StructuralEqualityComparer.Equals(pwKey, user.Crypto.Key);
            }));
            return ret;
        }

        public async Task ChangeUserPasswordAsync(RegisteredUser user, string newPassword)
        {
            var lockEntry = ServerContext.ServiceTable.GetOrCreate(user.Username).UserLock;
            await lockEntry.WithExclusiveWriteAsync(Task.Run(async () =>
            {
                // Recompute password crypto
                var cryptoConf = PasswordCryptoConfiguration.CreateDefault();
                var cryptoHelper = new AuthCryptoHelper(cryptoConf);
                var pwSalt = cryptoHelper.GenerateSalt();
                var encryptedPassword =
                    cryptoHelper.CalculateUserPasswordHash(newPassword, pwSalt);
                user.Crypto = new ItemCrypto
                {
                    Salt = pwSalt,
                    Conf = cryptoConf,
                    Key = encryptedPassword
                };
                // Save changes
                await UpdateUserInDatabaseAsync(user);
            }));
        }

        public async Task DeleteUserAsync(string userId)
        {
            await Task.Run(async () =>
            {
                userCollection.Delete(x => x.Identifier == userId);

                // purge additional data
                // remove team data
                ServerContext.AppState.PlayerData.Remove(userId);

                // remove persistent data
                var persistentDataService = new PlayerPersistentDataService(ServerContext);
                await persistentDataService.RemovePersistentDataAsync(userId);

                ServerContext.AppState.UserAnalyticsData.Remove(userId);

                // TODO: Purge all entities
            });
        }

        public async Task GenerateNewApiKeyAsync(RegisteredUser user)
        {
            var lockEntry = ServerContext.ServiceTable.GetOrCreate(user.Username).UserLock;
            await lockEntry.WithExclusiveWriteAsync(Task.Run(async () =>
            {
                // Recompute key
                user.ApiKey = StringUtils.SecureRandomString(AuthCryptoHelper.DefaultApiKeyLength);
                await UpdateUserInDatabaseAsync(user);
            }));
        }

        public async Task SetEnabledAsync(RegisteredUser user, bool status)
        {
            var lockEntry = ServerContext.ServiceTable.GetOrCreate(user.Username).UserLock;
            await lockEntry.ObtainExclusiveWriteAsync();
            user.Enabled = status;
            await UpdateUserInDatabaseAsync(user);
            lockEntry.ReleaseExclusiveWrite();
        }

        public int RegisteredUserCount => userCollection.Count();
    }
}
