namespace Speercs.Server.Models.Requests
{
    public class AdminUserModificationRequest : UserModificationRequest
    {
        public bool Enabled { get; set; } = true;
    }
}
