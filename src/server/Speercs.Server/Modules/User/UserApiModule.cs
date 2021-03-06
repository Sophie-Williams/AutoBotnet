using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models;
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

        public PersistentDataService playerDataService { get; private set; }

        public UserPersistentData persistentData { get; private set; }

        public UserMetricsService metricsService { get; private set; }

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
                playerDataService = new PersistentDataService(this.serverContext);
                persistentData = playerDataService.get(userIdentifier);
                metricsService = new UserMetricsService(this.serverContext);
                currentUser = userManager.findUserByIdentifierAsync(userIdentifier).Result;
                return null;
            };
        }

        protected void logMetrics(MetricsEventType eventType) {
            metricsService.log(currentUser.identifier, eventType);
        }
    }
}