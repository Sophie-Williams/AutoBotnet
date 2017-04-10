using Nancy;
using Speercs.Server.Configuration;
using Speercs.Server.Services;

namespace  Speercs.Server.Modules
{
    public class AuthenticationModule : SBaseModule
    {
        private UserManagerService userManager;

        public ISContext ServerContext { get; private set; }
        
        public AuthenticationModule(ISContext serverContext) : base("/auth")
        {
            ServerContext = serverContext;
            Before += ctx =>
            {
                userManager = new UserManagerService(ServerContext);
                return null;
            };
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