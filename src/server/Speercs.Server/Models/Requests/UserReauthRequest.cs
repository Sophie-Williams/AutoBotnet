namespace Speercs.Server.Models.Requests
{
    public class UserReauthRequest
    {
        public string Username { get; set; }

        public string ApiKey { get; set; }
    }
}
