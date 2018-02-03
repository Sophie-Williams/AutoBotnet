namespace Speercs.Server.Models.Requests {
    public class UserRegistrationRequest {
        public string username { get; set; }

        public string email { get; set; } = "";

        public string password { get; set; }

        public string inviteKey { get; set; } = "";
    }
}