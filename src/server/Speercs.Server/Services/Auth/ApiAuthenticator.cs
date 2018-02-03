using Speercs.Server.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System;

namespace Speercs.Server.Services.Auth {
    public class ApiAuthenticator : DependencyObject {
        public const string AUTH_TYPE_CLAIM_KEY = "auth_type";

        public const string USER_IDENTIFIER_CLAIM_KEY = "user_id";

        public ApiAuthenticator(ISContext context) : base(context) { }

        public ClaimsPrincipal resolveIdentity(string apikey) {
            // check admin keys
            var adminKey = serverContext.configuration.adminKeys.FirstOrDefault(x => x == apikey);
            if (adminKey != null) {
                var adminAuthClaims = new List<Claim>() {
                    new Claim(AUTH_TYPE_CLAIM_KEY, ApiAccessScope.Admin.ToString()),
                };
                var adminAuthIdentity = new ClaimsIdentity(adminAuthClaims);
                var adminIdentity = new ClaimsPrincipal(adminAuthIdentity);
                return adminIdentity;
            }

            // get user identity
            var userManager = new UserManagerService(serverContext);
            var user = userManager.findUserByApiKeyAsync(apikey).Result;
            if (user == null) return null;
            if (user.analyticsEnabled) {
                var analyticsObject = serverContext.appState.userAnalyticsData[user.identifier];
                analyticsObject.apiRequests++;
                analyticsObject.lastRequest = (ulong) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            var userAuthClaims = new List<Claim>() {
                new Claim(AUTH_TYPE_CLAIM_KEY, ApiAccessScope.User.ToString()),
                new Claim(USER_IDENTIFIER_CLAIM_KEY, user.identifier)
            };
            var userAuthIdentity = new ClaimsIdentity(userAuthClaims);
            var userIdentity = new ClaimsPrincipal(userAuthIdentity);
            // TODO: Maybe more claims?
            return userIdentity;
        }
    }
}