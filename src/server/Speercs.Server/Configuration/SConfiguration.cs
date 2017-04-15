using Newtonsoft.Json;

namespace Speercs.Server.Configuration
{
    public class SConfiguration
    {
        [JsonProperty("databaseConfig")]
        public SDatabaseConfiguration DatabaseConfiguration { get; set; } = new SDatabaseConfiguration();

        [JsonProperty("inviteKey")]
        public string InviteKey { get; set; } = null;

        [JsonProperty("adminKeys")]
        public string[] AdminKeys { get; set; } = new string[0];

        [JsonIgnore]
        public string BaseDirectory { get; set; }

        [JsonIgnore]
        public string GameName { get; } = "Speercs";

        [JsonProperty("globalName")]
        public string GlobalName { get; set; } = "Speercs";

        [JsonProperty("globalMessage")]
        public string GlobalMessage { get; set; } = "Speercs server v{ver}\nThis message is configurable by the server admins.";

        [JsonProperty("corsOrigins")]
        public string[] CorsOrigins { get; set; } = new string[0];
    }
}
