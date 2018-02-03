namespace Speercs.Server.Models.Requests {
    public class UserModificationRequest {
        public string Email { get; set; } = string.Empty;

        public bool AnalyticsEnabled { get; set; } = false;
    }
}