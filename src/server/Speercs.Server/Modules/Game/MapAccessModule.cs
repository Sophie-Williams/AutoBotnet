using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Modules.Game
{
    public class MapAccessModule : UserApiModule
    {
        private UserManagerService userManager;

        public MapAccessModule(ISContext serverContext) : base("/game/test", serverContext)
        {
            Get("/", async _ => (await userManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name)).Username);
        }
    }
}