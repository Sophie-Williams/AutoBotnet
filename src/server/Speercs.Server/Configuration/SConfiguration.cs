using System.Collections.Generic;
using Newtonsoft.Json;
using Speercs.Server.Services.Application;

namespace Speercs.Server.Configuration {
    public class SConfiguration {
        [JsonProperty("databaseConfig")]
        public SDatabaseConfiguration databaseConfiguration { get; set; } = new SDatabaseConfiguration();

        /// <summary>
        /// Plugin assemblies for dynamically loading extensibility plugins
        /// </summary>
        [JsonProperty("plugins")]
        public List<string> pluginAssemblies { get; set; } = new List<string>();

        /// <summary>
        /// Whether to enable ASP.NET Core verbose logging
        /// </summary>
        [JsonProperty("aspnetVerboseLogging")]
        public bool aspnetVerboseLogging { get; set; } = true;

        /// <summary>
        /// If an invite key is required for registration
        /// </summary>
        [JsonProperty("requireInvite")]
        public bool inviteRequired { get; set; }

        /// <summary>
        /// Special API keys that grant privileged admin access (keep these secret!)
        /// </summary>
        [JsonProperty("adminKeys")]
        public string[] adminKeys { get; set; } = new string[0];

        /// <summary>
        /// A dynamically updated parameter used in setting up the server context.
        /// </summary>
        [JsonIgnore]
        public string baseDirectory { get; set; }

        /// <summary>
        /// The name of the game as displayed to end-users.
        /// </summary>
        [JsonIgnore]
        public string gameName { get; set; } = "AutoBotnet";

        /// <summary>
        /// The name of the server instance.
        /// </summary>
        [JsonProperty("globalName")]
        public string globalName { get; set; } = "AutoBotnet";

        /// <summary>
        /// A MOTD (message of the day) to display to all users.
        /// </summary>
        [JsonProperty("globalMessage")]
        public string globalMessage { get; set; } =
            "Welcome to AutoBotnet server v{ver}.";

        /// <summary>
        /// List of origins to allow CORS requests from. Can possibly be used to enable API access from another domain hosting a custom client.
        /// </summary>
        [JsonProperty("corsOrigins")]
        public string[] corsOrigins { get; set; } = new string[0];

        /// <summary>
        /// Interval (in milliseconds) between running the loop function
        /// If UseDynamicTickrate is enabled, this will be the minimum amount of time between ticks
        /// </summary>
        [JsonProperty("tickrate")]
        public int tickrate { get; set; } = 1000;

        /// <summary>
        /// Dynamically set the Tickrate based on server load
        /// (Not currently implemented)
        /// </summary>
        [JsonProperty("useDynamicTickrate")]
        public bool useDynamicTickrate { get; set; }

        /// <summary>
        /// The maximum amount of time (in milliseconds) to wait for loading user code into the engine.
        /// </summary>
        [JsonProperty("codeLoadTimeLimit")]
        public int codeLoadTimeLimit { get; set; } = 200;

        /// <summary>
        /// The limit of the depth of recursion allowed in user scripts
        /// </summary>
        [JsonProperty("codeRecursionLimit")]
        public int codeRecursionLimit { get; set; } = 12;

        /// <summary>
        /// The limit (in chars) of the raw user source code length.
        /// </summary>
        [JsonProperty("codeSizeLimit")]
        public int codeSizeLimit { get; set; } = 24000;

        /// <summary>
        /// The limit (in bytes) of script executor memory usage
        /// </summary>
        [JsonProperty("codeMemoryLimit")]
        public long codeMemoryLimit { get; set; } = 16 * 1024 * 1024;

        /// <summary>
        /// The verbosity of the application logger.
        /// </summary>
        [JsonProperty("logLevel")]
        public SpeercsLogger.LogLevel logLevel { get; set; } = SpeercsLogger.LogLevel.Information;

        /// <summary>
        /// Maximum number of registered users. Set to -1 for unlimited.
        /// </summary>
        [JsonProperty("maxUsers")]
        public int maxUsers { get; set; } = -1;

        /// <summary>
        /// Database persistence interval in milliseconds.
        /// </summary>
        [JsonProperty("persistenceInterval")]
        public int persistenceInterval { get; set; } = 1000 * 60;
    }
}