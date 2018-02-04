using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Application;
using Speercs.Server.Services.Auth;
using Speercs.Server.Services.Auth.Security;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Modules.Admin {
    /// <summary>
    /// Defines a module that is part of the **authenticated** user API.
    /// </summary>
    public abstract class AdminApiModule : SBaseModule {
        public UserManagerService userManager { get; private set; }

        public PlayerPersistentDataService playerDataService { get; private set; }

        public ISContext serverContext { get; private set; }

        internal AdminApiModule(string path, ISContext serverContext) : base($"/admin{path}") {
            this.serverContext = serverContext;

            // require claims from stateless auther, defined in bootstrapper
            this.requiresAdminAuthentication();

            // add a pre-request hook to load the user manager
            Before += ctx => {
                // retrieve the key used for admin auth for logging purposes
                var adminKey = Context.CurrentUser.Claims
                        .FirstOrDefault(x => x.Type == ApiAuthenticator.API_KEY_CLAIM_KEY)
                        ?.Value;
                serverContext.log.writeLine($"Admin authenticated using key {adminKey}", SpeercsLogger.LogLevel.Information);
                
                userManager = new UserManagerService(this.serverContext);
                playerDataService = new PlayerPersistentDataService(this.serverContext);
                return null;
            };
        }
    }
}