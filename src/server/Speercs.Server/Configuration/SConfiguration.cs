using Newtonsoft.Json;

namespace Speercs.Server.Configuration
{
    public class SConfiguration
    {
        public SDatabaseConfiguration DatabaseConfiguration { get; set; } = new SDatabaseConfiguration();

        [JsonProperty("inviteKey")]
        public string InviteKey { get; set; } = null;

        public string BaseDirectory { get; set; }
    }
}