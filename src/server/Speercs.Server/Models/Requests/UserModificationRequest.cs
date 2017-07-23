namespace Speercs.Server.Models.Requests
{
    public class UserModificationRequest
    {
        public string Email { get; set; } = string.Empty;
    
        public bool Analytics { get; set; } = false;
    }
}

