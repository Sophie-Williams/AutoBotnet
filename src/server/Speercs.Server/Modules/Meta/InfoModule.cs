using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Models.Meta;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Meta {
    public class InfoModule : SBaseModule {
        public InfoModule(ISContext serverContext) : base("/info") {
            Get("/", _ => Response.asJsonNet(new PublicInfo(serverContext)));
        }
    }
}