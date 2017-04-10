using System;
using System.Security;
using System.Threading.Tasks;
using LiteDB;
using PenguinUpload.Services.Authentication;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Models.User;
using Speercs.Server.Utilities;

namespace  Speercs.Server.Services.Auth
{
    public class UserManagerService : DependencyObject
    {
        public const string RegisteredUsersKey = "r_users";
        private LiteCollection<RegisteredUser> userCollection;
        public UserManagerService(ISContext serverContext) : base(serverContext)
        {
            userCollection = serverContext.Database.GetCollection<RegisteredUser>(RegisteredUsersKey);
        }

        public Task<RegisteredUser> FindUserByUsernameAsync(string username)
        {
            return Task.FromResult(userCollection.FindOne(x => x.Username == username));
        }

        public async Task<RegisteredUser> RegisterUserAsync(UserRegistrationRequest regRequest)
        {
            if (await FindUserByUsernameAsync(regRequest.Username) != null) throw new SecurityException("A user with the same username already exists!");
            return await Task.Run(() => 
            {
                // Calculate cryptographic info
                var cryptoConf = PasswordCryptoConfiguration.CreateDefault();
                var cryptoHelper = new AuthCryptoHelper(cryptoConf);
                var pwSalt = cryptoHelper.GenerateSalt();
                var encryptedPassword =
                    cryptoHelper.CalculateUserPasswordHash(regRequest.Password, pwSalt);
                // Create user
                var userRecord = new RegisteredUser
                {
                    Identifier = Guid.NewGuid().ToString(),
                    Username = regRequest.Username,
                    ApiKey = StringUtils.SecureRandomString(AuthCryptoHelper.DefaultApiKeyLength),
                    Crypto = new ItemCrypto
                    {
                        Salt = pwSalt,
                        Conf = cryptoConf,
                        Key = encryptedPassword
                    },
                };
                // Add the user to the database
                userCollection.Insert(userRecord);

                // Index database
                userCollection.EnsureIndex(x => x.Identifier);
                userCollection.EnsureIndex(x => x.ApiKey);
                userCollection.EnsureIndex(x => x.Username);
                return userRecord;
            });
        }
    }
}