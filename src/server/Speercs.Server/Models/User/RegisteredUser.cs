using Newtonsoft.Json;

namespace Speercs.Server.Models.User
{
    public class RegisteredUser : DatabaseObject
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("apikey")]
        public string ApiKey { get; set; }

        [JsonProperty("analyticsEnabled")]
        public bool AnalyticsEnabled { get; set; }

        [JsonIgnore]
        public ItemCrypto Crypto { get; set; }

        [JsonIgnore]
        public string Identifier { get; set; }

        [JsonIgnore]
        public bool Enabled { get; set; }
    }
}
