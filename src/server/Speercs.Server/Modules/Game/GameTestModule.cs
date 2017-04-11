using System;
using System.Security;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Services.Auth;
using Speercs.Server.Utilities;

namespace  Speercs.Server.Modules.Game
{
    public class GameTestModule : SBaseModule
    {
        private UserManagerService userManager;

        public ISContext ServerContext { get; private set; }
        
        public GameTestModule(ISContext serverContext) : base("/game/test")
        {
            ServerContext = serverContext;
            
            this.RequiresAuthentication();

            Before += ctx =>
            {
                userManager = new UserManagerService(ServerContext);
                return null;
            };
            Get("/", async _ => (await userManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name)).Username);
        }
    }
}