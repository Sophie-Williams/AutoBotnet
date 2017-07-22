using Newtonsoft.Json;

namespace Speercs.Server.Configuration
{
    public class SConfiguration
    {
        [JsonProperty("databaseConfig")]
        public SDatabaseConfiguration DatabaseConfiguration { get; set; } = new SDatabaseConfiguration();

        /// <summary>
        /// Controls whether the Webpack Dev Middleware is enabled for serving the
        /// web interface in development mode.
        /// </summary>
        [JsonProperty("enableDevelopmentWebInterface")]
        public bool EnableDevelopmentWebInterface { get; set; } = true;

        /// <summary>
        /// If an invite key is required for registration
        /// </summary>
        [JsonProperty("requireInvite")]
        public bool InviteRequired { get; set; } = false;

        /// <summary>
        /// Special API keys that grant privileged admin access (keep these secret!)
        /// </summary>
        [JsonProperty("adminKeys")]
        public string[] AdminKeys { get; set; } = new string[0];

        /// <summary>
        /// A dynamically updated parameter used in setting up the server context.
        /// </summary>
        [JsonIgnore]
        public string BaseDirectory { get; set; }

        /// <summary>
        /// The name of the game as displayed to end-users. Currently immutable.
        /// </summary>
        [JsonIgnore]
        public string GameName { get; } = "AutoBotnet";

        /// <summary>
        /// The name of the server.
        /// </summary>
        [JsonProperty("globalName")]
        public string GlobalName { get; set; } = "AutoBotnet";

        /// <summary>
        /// A MOTD (message of the day) to display to all users.
        /// </summary>
        [JsonProperty("globalMessage")]
        public string GlobalMessage { get; set; } = "AutoBotnet server v{ver}\nThis message is configurable by the server admins.";

        /// <summary>
        /// List of origins to allow CORS requests from. Can possibly be used to enable API access from another domain hosting a custom client.
        /// </summary>
        [JsonProperty("corsOrigins")]
        public string[] CorsOrigins { get; set; } = new string[0];

        /// <summary>
        /// Interval (in milliseconds) between running the loop function
        /// If UseDynamicTickRate is enabled, this will be the minimum amount of time between ticks
        /// </summary>
        [JsonProperty("tickRate")]
        public int TickRate { get; set; } = 1000;

        /// <summary>
        /// Dynamically set the TickRate based on server load
        /// (Not currently implemented)
        /// </summary>
        [JsonProperty("useDynamicTickRate")]
        public bool UseDynamicTickRate { get; set; } = false;

        /// <summary>
        /// The maximum amount of time (in milliseconds) to wait for loading user code into the engine.
        /// </summary>
        [JsonProperty("codeLoadTimeLimit")]
        public int CodeLoadTimeLimit { get; set; } = 200;

        /// <summary>
        /// The limit (in chars) of the raw user source code length.
        /// </summary>
        [JsonProperty("codeSizeLimit")]
        public int CodeSizeLimit { get; set; } = 24000;

        /// <summary>
        /// Maximum number of registered users. Set to -1 for unlimited.
        /// </summary>
        [JsonProperty("maxUsers")]
        public int MaxUsers { get; set; } = -1;

        /// <summary>
        /// Database persistence interval in milliseconds.
        /// </summary>
        [JsonProperty("persistenceInterval")]
        public int PersistenceInterval { get; set; } = 1000 * 60;
    }
}
