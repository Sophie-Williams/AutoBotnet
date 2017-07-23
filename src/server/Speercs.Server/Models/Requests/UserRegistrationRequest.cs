namespace Speercs.Server.Models.Requests
{
    public class UserRegistrationRequest
    {
        public string Username { get; set; }

        public string Email { get; set; } = "";

        public string Password { get; set; }

        public string InviteKey { get; set; }
    }
}
