using Nancy;
using Speercs.Server.Configuration;

namespace Speercs.Server.Modules
{
    public class MetadataModule : NancyModule
    {
        public MetadataModule(ISContext serverContext) : base("/meta")
        {
            Get("/", _ => $"{serverContext.Configuration.GameName} Server Pre-Alpha");
            Get("/version", _ => SContext.Version); // automagically get version
            Get("/motd", _ => serverContext.Configuration.GlobalMessage.Replace("{ver}",SContext.Version));
            Get("/name", _ => serverContext.Configuration.GlobalName);
        }
    }
}
