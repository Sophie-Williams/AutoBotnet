using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Speercs.Server.Configuration;

namespace Speercs.Server.Services.Auth
{
    public class ApiAuthenticator : DependencyObject
    {
        public const string AuthTypeClaimKey = "auth_type";

        public const string UserIdentifierClaimKey = "user_id";

        public ApiAuthenticator(ISContext context) : base(context)
        {
        }

        public ClaimsPrincipal ResolveIdentity(string apiKey)
        {            
            // check admin keys
            var adminKey = ServerContext.Configuration.AdminKeys.FirstOrDefault(x => x == apiKey);
            if (adminKey != null)
            {
                var adminAuthClaims = new List<Claim>()
                {
                    new Claim(AuthTypeClaimKey, ApiAccessScope.Admin.ToString()),
                };
                var adminAuthIdentity = new ClaimsIdentity(adminAuthClaims);
                var adminIdentity = new ClaimsPrincipal(adminAuthIdentity);
                return adminIdentity;
            }

            // get user identity
            var userManager = new UserManagerService(ServerContext);
            var user = userManager.FindUserByApiKeyAsync(apiKey).Result;
            if (user == null) return null;
            var userAuthClaims = new List<Claim>()
            {
                new Claim(AuthTypeClaimKey, ApiAccessScope.User.ToString()),
                new Claim(UserIdentifierClaimKey, user.Identifier)
            };
            var userAuthIdentity = new ClaimsIdentity(userAuthClaims);
            var userIdentity = new ClaimsPrincipal(userAuthIdentity);
            // TODO: Maybe more claims?
            return userIdentity;
        }
    }
}