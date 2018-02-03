namespace Speercs.Server.Models.Requests {
    public class UserKeyResetRequest {
        public string username { get; set; }

        public string apiKey { get; set; }
    }
}