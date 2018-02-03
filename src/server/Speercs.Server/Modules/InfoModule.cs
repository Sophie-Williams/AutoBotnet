using Nancy;
using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules {
    public class InfoModule : SBaseModule {
        public InfoModule(ISContext serverContext) : base("/info") {
            Get("/", _ => Response.AsJsonNet(new PublicInfo(serverContext)));
        }
    }
}