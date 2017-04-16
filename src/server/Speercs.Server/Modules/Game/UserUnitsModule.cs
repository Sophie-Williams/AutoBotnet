using Speercs.Server.Configuration;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game
{
    public class UserUnitsModule : UserApiModule
    {
        public UserUnitsModule(ISContext serverContext) : base("/game/units", serverContext)
        {
            Get("/", _ => Response.AsJsonNet(ServerContext.AppState.Entities.GetAllByUser(ServerContext.AppState.PlayerData[Context.CurrentUser.Identity.Name])));
        }
    }
}