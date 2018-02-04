using Newtonsoft.Json;
using Speercs.Server.Configuration;

namespace Speercs.Server.Models.Meta {
    public class PublicMetadata : ProtectedDependencyObject {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("motd")]
        public string motd { get; set; }

        [JsonProperty("version")]
        public string version { get; set; }

        public PublicMetadata(ISContext serverContext) : base(serverContext) {
            name = serverContext.configuration.globalName;
            motd = serverContext.configuration.globalMessage.Replace("{ver}", SContext.version);
            version = SContext.version;
        }
    }
}