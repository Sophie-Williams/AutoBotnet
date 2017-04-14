using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Auth;
using Speercs.Server.Services.Auth.Security;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Modules.Game
{
    /// <summary>
    /// Defines a module that is part of the **authenticated** user API.
    /// </summary>
    public abstract class UserApiModule : SBaseModule
    {
        public UserManagerService UserManager { get; private set; }

        public PlayerPersistentDataService PlayerDataService { get; private set; }
        
        public RegisteredUser CurrentUser { get; private set; }

        public ISContext ServerContext { get; private set; }

        internal UserApiModule(string path, ISContext serverContext) : base(path)
        {
            ServerContext = serverContext;

            // require claims from stateless auther, defined in bootstrapper
            this.RequiresUserAuthentication();

            // add a pre-request hook to load the user manager
            Before += ctx =>
            {
                var userIdentifier = Context.CurrentUser.Claims.FirstOrDefault(x => x.Type == ApiAuthenticator.UserIdentifierClaimKey).Value;
                
                UserManager = new UserManagerService(ServerContext);
                PlayerDataService = new PlayerPersistentDataService(ServerContext);
                CurrentUser = UserManager.FindUserByIdentifierAsync(userIdentifier).Result;
                return null;
            };
        }
    }
}
