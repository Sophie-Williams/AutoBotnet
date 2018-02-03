using Speercs.Server.Configuration;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.User {
    public class UserUnitsModule : UserApiModule {
        public UserUnitsModule(ISContext serverContext) : base("/user/units", serverContext) {
            Get("/",
                _ => Response.asJsonNet(
                    base.serverContext.appState.entities.getAllByUser(
                        base.serverContext.appState.playerData[Context.CurrentUser.Identity.Name])));
        }
    }
}