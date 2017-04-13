using Speercs.Server.Configuration;

namespace Speercs.Server.Modules.Game
{
    public class GameTestModule : UserApiModule
    {
        public GameTestModule(ISContext serverContext) : base("/game/test", serverContext)
        {
            Get("/", async _ => (await UserManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name)).Username);
        }
    }
}
