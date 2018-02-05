using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Models.Meta;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Meta {
    public class MetadataModule : SBaseModule {
        public MetadataModule(ISContext serverContext) : base("/meta", serverContext) {
            Get("/", _ => Response.asJsonNet(new PublicMetadata(serverContext)));
        }
    }
}