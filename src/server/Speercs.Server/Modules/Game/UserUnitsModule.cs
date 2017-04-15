using Speercs.Server.Configuration;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game
{
    public class UserUnitsModule : UserApiModule
    {
        public UserUnitsModule(ISContext serverContext) : base("/game/units", serverContext)
        {
            // Should there be an instance of `UserTeam` for each user?
            Get("/", _ => Response.AsJsonNet(CurrentUser.Username));
        }
    }
}