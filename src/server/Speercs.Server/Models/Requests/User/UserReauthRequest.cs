namespace Speercs.Server.Models.Requests.User {
    public class UserReauthRequest {
        public string username { get; set; }

        public string apiKey { get; set; }
    }
}