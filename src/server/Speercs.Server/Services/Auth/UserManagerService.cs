using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests.User;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Game;
using Speercs.Server.Services.Metrics;
using Speercs.Server.Utilities;

namespace Speercs.Server.Services.Auth {
    public class UserManagerService : DependencyObject {
        public const string REGISTERED_USERS_KEY = "r_users";
        private LiteCollection<RegisteredUser> _userCollection;

        public UserManagerService(ISContext serverContext) : base(serverContext) {
            _userCollection = serverContext.database.GetCollection<RegisteredUser>(REGISTERED_USERS_KEY);
        }

        public async Task<RegisteredUser> registerUserAsync(UserRegistrationRequest regRequest) {
            if (await findUserByUsernameAsync(regRequest.username) != null)
                throw new SecurityException("A user with the same username already exists!");
            return await Task.Run(() => {
                // Calculate cryptographic info
                var cryptoConf = PasswordCryptoConfiguration.createDefault();
                var cryptoHelper = new AuthCryptoHelper(cryptoConf);
                var pwSalt = cryptoHelper.generateSalt();
                var encryptedPassword =
                    cryptoHelper.calculateUserPasswordHash(regRequest.password, pwSalt);
                // Create user
                var user = new RegisteredUser {
                    identifier = Guid.NewGuid().ToString(),
                    username = regRequest.username,
                    email = regRequest.email,
                    apiKey = StringUtils.secureRandomString(AuthCryptoHelper.DEFAULT_API_KEY_LENGTH),
                    crypto = new ItemCrypto {
                        salt = pwSalt,
                        conf = cryptoConf,
                        key = encryptedPassword
                    },
                    enabled = true
                };

                // Add the user to the database
                _userCollection.Insert(user);

                // Index database
                _userCollection.EnsureIndex(x => x.identifier);
                _userCollection.EnsureIndex(x => x.apiKey);
                _userCollection.EnsureIndex(x => x.username);

                // create persistent data
                var persistentDataService = new PersistentDataService(serverContext);
                persistentDataService.createPersistentData(user.identifier);

                var userMetricsService = new UserMetricsService(serverContext);
                userMetricsService.create(user);

                return user;
            });
        }

        public Task<RegisteredUser> findUserByApiKeyAsync(string apikey) {
            return Task.Run(() => (_userCollection.FindOne(x => x.apiKey == apikey)));
        }

        public Task<RegisteredUser> findUserByUsernameAsync(string username) {
            return Task.Run(() => (_userCollection.FindOne(x => x.username == username)));
        }

        public Task<RegisteredUser> findUserByIdentifierAsync(string id) {
            return Task.Run(() => (_userCollection.FindOne(x => x.identifier == id)));
        }

        /// <summary>
        /// Warning: Callers are expected to use their own locks before calling this method!
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> updateUserInDatabaseAsync(RegisteredUser user) {
            return await Task.Run(() => _userCollection.Update(user));
        }

        public async Task<bool> checkPasswordAsync(string password, RegisteredUser user) {
            var ret = false;
            var lockEntry = serverContext.serviceTable.getOrCreate(user.username).userLock;
            await lockEntry.withConcurrentReadAsync(Task.Run(() => {
                //Calculate hash and compare
                var cryptoHelper = new AuthCryptoHelper(user.crypto.conf);
                var pwKey =
                    cryptoHelper.calculateUserPasswordHash(password, user.crypto.salt);
                ret = StructuralComparisons.StructuralEqualityComparer.Equals(pwKey, user.crypto.key);
            }));
            return ret;
        }

        public async Task changeUserPasswordAsync(RegisteredUser user, string newPassword) {
            var lockEntry = serverContext.serviceTable.getOrCreate(user.username).userLock;
            await lockEntry.withExclusiveWriteAsync(Task.Run(async () => {
                // Recompute password crypto
                var cryptoConf = PasswordCryptoConfiguration.createDefault();
                var cryptoHelper = new AuthCryptoHelper(cryptoConf);
                var pwSalt = cryptoHelper.generateSalt();
                var encryptedPassword =
                    cryptoHelper.calculateUserPasswordHash(newPassword, pwSalt);
                user.crypto = new ItemCrypto {
                    salt = pwSalt,
                    conf = cryptoConf,
                    key = encryptedPassword
                };
                // regenerate key to invalidate old sessions
                await generateNewApiKeyAsync(user);

                // Save changes
                await updateUserInDatabaseAsync(user);
            }));
        }

        public Task deleteUserAsync(string userId) {
            return Task.Run(() => {
                _userCollection.Delete(x => x.identifier == userId);

                // remove persistent data
                var persistentDataService = new PersistentDataService(serverContext);
                persistentDataService.removePersistentData(userId);

                var userMetricsService = new UserMetricsService(serverContext);
                userMetricsService.delete(userId);

                // TODO: do something with the orphaned entities
            });
        }

        public async Task generateNewApiKeyAsync(RegisteredUser user) {
            var lockEntry = serverContext.serviceTable.getOrCreate(user.username).userLock;
            await lockEntry.withExclusiveWriteAsync(Task.Run(async () => {
                // Recompute key
                user.apiKey = StringUtils.secureRandomString(AuthCryptoHelper.DEFAULT_API_KEY_LENGTH);
                await updateUserInDatabaseAsync(user);
            }));
        }

        public async Task setEnabledAsync(RegisteredUser user, bool status) {
            var lockEntry = serverContext.serviceTable.getOrCreate(user.username).userLock;
            await lockEntry.obtainExclusiveWriteAsync();
            user.enabled = status;
            await updateUserInDatabaseAsync(user);
            lockEntry.releaseExclusiveWrite();
        }

        public IEnumerable<RegisteredUser> getUsers() {
            return _userCollection.FindAll();
        }

        public int registeredUserCount => _userCollection.Count();
    }
}