using Newtonsoft.Json;
using Speercs.Server.Configuration;

namespace Speercs.Server.Models
{
    public class PublicMetadata
    {
        [JsonProperty("name")]
        public string serverName { get; set; }
        [JsonProperty("motd")]
        public string serverMotd { get; set; }
        [JsonProperty("version")]
        public string serverVersion { get; set; }

        public PublicMetadata(ISContext context)
        {
            serverName = context.Configuration.GlobalName;
            serverMotd = context.Configuration.GlobalMessage.Replace("{ver}",SContext.Version);
            serverVersion = SContext.Version;
        }
    }
}