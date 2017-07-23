namespace Speercs.Server.Models.Requests
{
    public class UserModificationRequest
    {
        public string Email { get; set; } = "";
    
        public bool Analytics { get; set; } = false;
    }
}

