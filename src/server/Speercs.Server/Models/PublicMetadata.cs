using Newtonsoft.Json;
using Speercs.Server.Configuration;

namespace Speercs.Server.Models
{
    public class PublicMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("motd")]
        public string MOTD { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        public PublicMetadata(SConfiguration configuration)
        {
            Name = configuration.GlobalName;
            MOTD = configuration.GlobalMessage.Replace("{ver}", SContext.Version);
            Version = SContext.Version;
        }
    }
}