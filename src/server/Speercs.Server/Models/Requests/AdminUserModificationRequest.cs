namespace Speercs.Server.Models.Requests
{
    public class AdminUserModificationRequest
    {
        public string Email { get; set; } = string.Empty;
    
        public string Username { get; set; } = string.Empty;

        public bool Enabled { get; set; } = false;
    }
}

