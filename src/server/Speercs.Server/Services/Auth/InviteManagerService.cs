using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Utilities;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Speercs.Server.Services.Auth
{
    public class InviteManagerService : DependencyObject
    {
        public const string InviteKeyKey = "r_invites";
        private LiteCollection<string> keyCollection;

        public InviteManagerService(ISContext serverContext) : base(serverContext)
        {
            keyCollection = serverContext.Database.GetCollection<string>(InviteKeyKey);
        }

        public async Task<string> GenerateInviteAsync()
        {
            return await Task.Run(() =>
            {
                //Generate new invite key
                var inviteKey = StringUtils.SecureRandomString(16);

                // Add the user to the database
                keyCollection.Insert(inviteKey);

                // Index database
                return inviteKey;
            });
        }

        public async Task<bool> UseKeyAsync(string inviteKey)
        {
            return await Task.Run(() => keyCollection.Delete(inviteKey));
        }
    }
}
