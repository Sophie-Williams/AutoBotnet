using System.Collections.Generic;
using Newtonsoft.Json;

namespace Speercs.Server.Configuration
{
    public class SConfiguration
    {
        public SDatabaseConfiguration DatabaseConfiguration { get; set; } = new SDatabaseConfiguration();

        [JsonProperty("inviteKey")]
        public string InviteKey { get; set; } = null;

        public string[] AdminKeys { get; set; } = new string[0];

        public string BaseDirectory { get; set; }

        public string GameName { get; } = "Speercs";

        public string GlobalMessage { get; set; }

        public string GlobalName { get; set; }
        public string[] CorsOrigins { get; set; } = new string[0];
    }
}
