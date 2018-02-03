using Newtonsoft.Json;

namespace Speercs.Server.Models.User {
    /// <summary>
    /// Public user data model
    /// </summary>
    public class PublicUser : DatabaseObject {
        [JsonProperty("username")]
        public string Username { get; set; }

        public PublicUser(RegisteredUser user) {
            Username = user.Username;
        }
    }
}