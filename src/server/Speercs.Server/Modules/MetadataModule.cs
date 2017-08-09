using Nancy;
using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules
{
    public class MetadataModule : SBaseModule
    {
        public MetadataModule(ISContext serverContext) : base("/meta")
        {
            Get("/", _ => Response.AsJsonNet(new PublicMetadata(serverContext)));
        }
    }
}
