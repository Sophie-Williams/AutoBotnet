using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.User {
    public class UserUnitsModule : UserApiModule {
        public UserUnitsModule(ISContext serverContext) : base("/user/units", serverContext) {
            Get("/",
                _ => Response.asJsonNet(
                    EntityBag.getByUser(
                        persistentData.team)));
        }
    }
}