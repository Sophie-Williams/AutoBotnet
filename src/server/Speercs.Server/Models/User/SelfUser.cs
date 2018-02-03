using Newtonsoft.Json;

namespace Speercs.Server.Models.User {
    /// <summary>
    /// Public user data model
    /// </summary>
    public class SelfUser : DatabaseObject {
        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("analyticsEnabled")]
        public bool analyticsEnabled { get; set; }

        [JsonProperty("id")]
        public string identifier { get; set; }

        public SelfUser(RegisteredUser user) {
            username = user.username;
            email = user.email;
            analyticsEnabled = user.analyticsEnabled;
            identifier = user.identifier;
        }
    }
}