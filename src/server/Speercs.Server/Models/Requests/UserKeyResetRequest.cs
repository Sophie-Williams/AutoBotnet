namespace Speercs.Server.Models.Requests
{
    public class UserKeyResetRequest
    {
        public string Username { get; set; }

        public string ApiKey { get; set; }
    }
}
