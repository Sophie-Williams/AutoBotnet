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
        public string GameName { get; } = "AutoBotnet";

        [JsonProperty("globalName")]
        public string GlobalName { get; set; } = "AutoBotnet";

        [JsonProperty("globalMessage")]
        public string GlobalMessage { get; set; } = "AutoBotnet server v{ver}\nThis message is configurable by the server admins.";

        [JsonProperty("corsOrigins")]
        public string[] CorsOrigins { get; set; } = new string[0];

        [JsonProperty("tickRate")]
        public int TickRate { get; set; } = 1000;

        [JsonProperty("useDynamicTickRate")]
        public bool UseDynamicTickRate { get; set; } = false;

        /// <summary>
        /// The maximum amount of time (in ms) to wait for loading user code
        /// </summary>
        [JsonProperty("codeLoadTimeLimit")]
        public int CodeLoadTimeLimit { get; set; } = 200;

        /// <summary>
        /// The limit (in chars) of the raw user source code length
        /// </summary>
        [JsonProperty("codeSizeLimit")]
        public int CodeSizeLimit { get; set; } = 24000;
    }
}
