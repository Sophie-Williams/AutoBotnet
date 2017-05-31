namespace Speercs.Server.Models.Requests
{
    public class UserPasswordChangeRequest
    {
        public string Username { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
