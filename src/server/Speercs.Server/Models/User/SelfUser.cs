using Newtonsoft.Json;

namespace Speercs.Server.Models.User {
    /// <summary>
    /// Public user data model
    /// </summary>
    public class SelfUser : DatabaseObject {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("analyticsEnabled")]
        public bool AnalyticsEnabled { get; set; }

        [JsonProperty("id")]
        public string Identifier { get; set; }

        public SelfUser(RegisteredUser user) {
            Username = user.Username;
            Email = user.Email;
            AnalyticsEnabled = user.AnalyticsEnabled;
            Identifier = user.Identifier;
        }
    }
}