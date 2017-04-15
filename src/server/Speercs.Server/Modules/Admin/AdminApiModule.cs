using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;
using Speercs.Server.Services.Auth.Security;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Modules.Admin
{
    /// <summary>
    /// Defines a module that is part of the **authenticated** user API.
    /// </summary>
    public abstract class AdminApiModule : SBaseModule
    {
        public UserManagerService UserManager { get; private set; }

        public PlayerPersistentDataService PlayerDataService { get; private set; }
        
        public ISContext ServerContext { get; private set; }

        internal AdminApiModule(string path, ISContext serverContext) : base(path)
        {
            ServerContext = serverContext;

            // require claims from stateless auther, defined in bootstrapper
            this.RequiresAdminAuthentication();

            // add a pre-request hook to load the user manager
            Before += ctx =>
            {
                UserManager = new UserManagerService(ServerContext);
                PlayerDataService = new PlayerPersistentDataService(ServerContext);
                return null;
            };
        }
    }
}
