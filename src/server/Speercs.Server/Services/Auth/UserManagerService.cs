using System;
using System.Threading.Tasks;
using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;

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

        public Task<RegisteredUser> RegisterUserAsync(UserRegistrationRequest regRequest)
        {
            
        }
    }
}