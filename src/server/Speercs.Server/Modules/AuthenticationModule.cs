using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Services.Auth;
using Speercs.Server.Utilities;
using System;
using System.Security;
using System.Text.RegularExpressions;

namespace Speercs.Server.Modules
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
            Post("/register", async args =>
            {
                Regex charsetRegex = new Regex(@"^[a-zA-Z0-9\.\_\-]{3,24}$");

                var req = this.Bind<UserRegistrationRequest>();
                // Valdiate username length
                if (req.Username.Length < 2)
                {
                    throw new SecurityException("Username must be at least 2 characters.");
                }
                if (req.Username.Length > 24)
                {
                    throw new SecurityException("Username may not exceed 24 characters.");
                }

                // Validate username charset
                if (charsetRegex.Matches(req.Username).Count <= 0)
                {
                    throw new SecurityException("Invalid character in username.");
                }

                // Validate password
                if (req.Password.Length < 8)
                {
                    throw new SecurityException("Password must be at least 8 characters.");
                }
                if (req.Password.Length > 128)
                {
                    throw new SecurityException("Password may not exceed 128 characters.");
                }

                // Check invite key if enabled
                if (!string.IsNullOrWhiteSpace(ServerContext.Configuration.InviteKey))
                {
                    // Validate invite key
                    if (req.InviteKey != ServerContext.Configuration.InviteKey)
                    {
                        throw new SecurityException("The invite key is not recognized.");
                    }
                }

                // Register user
                var newUser = await userManager.RegisterUserAsync(req);

                // Return user details
                return Response.AsJsonNet(newUser);
            });

            Post("/login", async args =>
            {
                var req = this.Bind<UserLoginRequest>();
                var selectedUser = await userManager.FindUserByUsernameAsync(req.Username);

                if (selectedUser == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate password
                    if (selectedUser.Enabled && await userManager.CheckPasswordAsync(req.Password, selectedUser))
                    {
                        // Return user details
                        return Response.AsJsonNet(selectedUser);
                    }
                    return HttpStatusCode.Unauthorized;
                }
                catch (NullReferenceException)
                {
                    // A parameter was not provided
                    return HttpStatusCode.BadRequest;
                }
                catch (SecurityException secEx)
                {
                    // Registration blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
            });
        }
    }
}