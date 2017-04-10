using System.Security;
using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Services;
using Speercs.Server.Utilities;

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
                var req = this.Bind<UserRegistrationRequest>();
                // Valdiate username length, charset
                if (req.Username.Length < 2)
                {
                    throw new SecurityException("Username must be at least 2 characters.");
                }
                // Validate phone number

                // Validate password
                if (req.Password.Length < 8)
                {
                    throw new SecurityException("Password must be at least 8 characters.");
                }

                if (req.Username.Length > 24)
                {
                    throw new SecurityException("Username may not exceed 24 characters.");
                }

                if (req.Password.Length > 128)
                {
                    throw new SecurityException("Password may not exceed 128 characters.");
                }

                // Check invite key if enabled
                if (!string.IsNullOrWhiteSpace(ServerContext.Configuration.InviteKey))
                {
                    if (req.InviteKey != ServerContext.Configuration.InviteKey)
                    {
                        throw new SecurityException("The invite key is not recognized.");
                    }
                }

                // Validate registration
                var newUser = await userManager.RegisterUserAsync(req);

                // Return user details
                return Response.AsJsonNet(new RemoteAuthResponse
                {
                    User = newUser,
                    ApiKey = newUser.ApiKey
                });
                return HttpStatusCode.Unauthorized;
            });
        }
    }
}