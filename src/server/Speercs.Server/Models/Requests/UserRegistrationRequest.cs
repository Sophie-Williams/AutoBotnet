namespace Speercs.Server.Models.Requests
{
    public class UserRegistrationRequest
    {
        public string Username { get; }

        public string Password { get; }
        
        public string InviteKey { get; }
    }
}