using Nancy.Security;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Modules.Game
{
    public abstract class UserApiModule : SBaseModule
    {
        public UserManagerService UserManager { get; private set; }
        
        public ISContext ServerContext { get; private set; }

        internal UserApiModule(string path, ISContext serverContext) : base(path)
        {
            ServerContext = serverContext;

            this.RequiresAuthentication();

            Before += ctx =>
            {
                UserManager = new UserManagerService(ServerContext);
                return null;
            };
        }
    }
}