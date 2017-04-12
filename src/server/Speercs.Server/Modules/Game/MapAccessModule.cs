using Speercs.Server.Configuration;

namespace Speercs.Server.Modules.Game
{
    public class MapAccessModule : UserApiModule
    {
        public MapAccessModule(ISContext serverContext) : base("/game/map", serverContext)
        {
            Get("/", async _ => (await UserManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name)).Username);
        }
    }
}