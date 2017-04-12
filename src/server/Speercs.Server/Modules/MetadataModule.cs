using Speercs.Server.Configuration;

namespace Speercs.Server.Modules
{
    public class MetadataModule : SBaseModule
    {
        public MetadataModule() : base("/meta")
        {
            Get("/", _ => "Speercs Server Pre-Alpha");
            Get("/version", _ => SContext.Version); // automagically get version
        }
    }
}