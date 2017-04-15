using Newtonsoft.Json;

namespace Speercs.Server.Configuration
{
    public class SConfiguration
    {
        public SDatabaseConfiguration DatabaseConfiguration { get; set; } = new SDatabaseConfiguration();

        [JsonProperty("inviteKey")]
        public string InviteKey { get; set; } = null;

        [JsonProperty("adminKeys")]
        public string[] AdminKeys { get; set; } = new string[0];

        [JsonIgnore]
        public string BaseDirectory { get; set; }

        [JsonProperty("gameName")]
        public string GameName { get; } = "Speercs";

        [JsonProperty("globalMessage")]
        public string GlobalMessage { get; set; }

        [JsonProperty("corsOrigins")]
        public string[] CorsOrigins { get; set; } = new string[0];
    }
}
