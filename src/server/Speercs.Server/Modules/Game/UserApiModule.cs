using Nancy.Security;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Modules.Game
{
    /// <summary>
    /// Defines a module that is part of the **authenticated** user API.
    /// </summary>
    public abstract class UserApiModule : SBaseModule
    {
        public UserManagerService UserManager { get; private set; }
        
        public RegisteredUser CurrentUser { get; private set; }

        public ISContext ServerContext { get; private set; }

        internal UserApiModule(string path, ISContext serverContext) : base(path)
        {
            ServerContext = serverContext;

            // require claims from stateless auther, defined in bootstrapper
            this.RequiresAuthentication();

            // add a pre-request hook to load the user manager
            Before += ctx =>
            {
                UserManager = new UserManagerService(ServerContext);
                CurrentUser = UserManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name).Result;
                return null;
            };
        }
    }
}
