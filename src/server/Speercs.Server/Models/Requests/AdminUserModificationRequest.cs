namespace Speercs.Server.Models.Requests {
    public class AdminUserModificationRequest : UserModificationRequest {
        public bool enabled { get; set; } = true;
    }
}