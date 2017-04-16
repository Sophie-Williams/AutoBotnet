using Newtonsoft.Json;

namespace Speercs.Server.Models.User
{
    public class RegisteredUser : DatabaseObject
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonIgnore]
        public ItemCrypto Crypto { get; set; }

        [JsonIgnore]
        public string Identifier { get; set; }

        [JsonIgnore]
        public bool Enabled { get; set; }
    }
}
