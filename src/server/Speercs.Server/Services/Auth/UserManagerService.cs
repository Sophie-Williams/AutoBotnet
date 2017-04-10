using System;
using LiteDB;
using Speercs.Server.Configuration;

namespace  Speercs.Server.Services.Auth
{
    public class UserManagerService : DependencyObject
    {
        private LiteCollection<RegisteredUser> userCollection;
        public UserManagerService(ISContext serverContext) : base(serverContext)
        {
        }

        public async Task<RegisteredUser> GetUserByUsername(string username)
        {

        }
    }
}