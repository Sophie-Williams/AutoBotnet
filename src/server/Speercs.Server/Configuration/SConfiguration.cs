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

        /// <summary>
        /// Name of the server
        /// </summary>
        [JsonProperty("globalName")]
        public string GlobalName { get; set; } = "AutoBotnet";

        /// <summary>
        /// Message to show all users
        /// </summary>
        [JsonProperty("globalMessage")]
        public string GlobalMessage { get; set; } = "AutoBotnet server v{ver}\nThis message is configurable by the server admins.";

        /// <summary>
        /// List of origins to allow CORS requests from
        /// </summary>
        [JsonProperty("corsOrigins")]
        public string[] CorsOrigins { get; set; } = new string[0];

        /// <summary>
        /// Interval (in MS) between running the loop function
        /// If UseDynamicTickRate is enabled, this will be the minimum ammount of time between ticks
        /// </summary>
        [JsonProperty("tickRate")]
        public int TickRate { get; set; } = 1000;

        /// <summary>
        /// Dynamically set the TickRate based on server load
        /// Not currently implemented
        /// </summary>
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

        /// <summary>
        /// Maximum number of registered users
        /// </summary>
        [JsonProperty("maxUsers")]
        public int MaxUsers { get; set; } = -1;

        /// <summary>
        /// Database persistence interval in milliseconds
        /// </summary>
        [JsonProperty("persistenceInterval")]
        public int PersistenceInterval { get; set; } = 1000 * 60;
    }
}
