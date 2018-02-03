using Newtonsoft.Json;

namespace Speercs.Server.Configuration {
    public class SDatabaseConfiguration {
        [JsonProperty("fileName")]
        public string fileName { get; } = "speercs.lidb";
    }
}