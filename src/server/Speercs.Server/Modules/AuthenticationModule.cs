using Nancy;

namespace  Speercs.Server.Modules
{
    public class AuthenticationModule : SBaseModule
    {
        public AuthenticationModule() : base("/auth")
        {
            Post("/register", args => 
            {
                // Form data:
                // username: Username
                // password: Password
                return HttpStatusCode.Unauthorized;
            });
        }
    }
}