using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Auth;
using Speercs.Server.Services.Auth.Security;
using Speercs.Server.Services.Game;
using Speercs.Server.Services.Metrics;

namespace Speercs.Server.Modules.User {
    /// <summary>
    /// Defines a module that is part of the **authenticated** user API.
    /// </summary>
    public abstract class UserApiModule : SBaseModule {
        public UserManagerService userManager { get; private set; }

        public PlayerPersistentDataService playerDataService { get; private set; }
        
        public UserMetricsService userMetrics { get; private set; }

        public RegisteredUser currentUser { get; private set; }

        internal UserApiModule(string path, ISContext serverContext) : base(path, serverContext) {
            // require claims from stateless auther, defined in bootstrapper
            this.requiresUserAuthentication();

            // add a pre-request hook to load the user manager
            Before += ctx => {
                var userIdentifier = Context.CurrentUser.Claims
                    .FirstOrDefault(x => x.Type == ApiAuthenticator.USER_IDENTIFIER_CLAIM_KEY)
                    ?.Value;

                userManager = new UserManagerService(this.serverContext);
                playerDataService = new PlayerPersistentDataService(this.serverContext);
                userMetrics = new UserMetricsService(this.serverContext, userIdentifier);
                currentUser = userManager.findUserByIdentifierAsync(userIdentifier).Result;
                return null;
            };
        }
    }
}