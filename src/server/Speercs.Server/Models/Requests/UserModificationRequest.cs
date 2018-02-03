namespace Speercs.Server.Models.Requests {
    public class UserModificationRequest {
        public string email { get; set; } = string.Empty;

        public bool analyticsEnabled { get; set; } = false;
    }
}