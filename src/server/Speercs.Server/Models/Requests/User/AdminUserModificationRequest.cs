namespace Speercs.Server.Models.Requests.User {
    public class AdminUserModificationRequest : UserModificationRequest {
        public bool enabled { get; set; } = true;
    }
}