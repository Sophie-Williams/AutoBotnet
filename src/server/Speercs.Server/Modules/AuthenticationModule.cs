using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Requests;
using Speercs.Server.Modules.Exceptions;
using Speercs.Server.Services.Auth;
using Speercs.Server.Services.Game;
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

            // Register account
            Post("/register", async args =>
            {
                Regex charsetRegex = new Regex(@"^[a-zA-Z0-9._-]{3,24}$");

                var req = this.Bind<UserRegistrationRequest>();

                try
                {
                    // Valdiate username length
                    if (req.Username.Length < 2)
                    {
                        throw new InvalidParameterException("Username must be at least 2 characters.");
                    }

                    if (req.Username.Length > 24)
                    {
                        throw new InvalidParameterException("Username may not exceed 24 characters.");
                    }

                    // Validate username charset
                    if (charsetRegex.Matches(req.Username).Count <= 0)
                    {
                        throw new InvalidParameterException("Invalid character in username.");
                    }

                    // Validate password
                    if (req.Password.Length < 8)
                    {
                        throw new InvalidParameterException("Password must be at least 8 characters.");
                    }
                    
                    if (req.Password.Length > 128)
                    {
                        throw new InvalidParameterException("Password may not exceed 128 characters.");
                    }

                    // Check invite key if enabled
                    if (!string.IsNullOrWhiteSpace(ServerContext.Configuration.InviteKey))
                    {
                        // Validate invite key
                        if (req.InviteKey != ServerContext.Configuration.InviteKey)
                        {
                            return HttpStatusCode.PaymentRequired;
                        }
                    }

                    // Attempt to register user
                    var newUser = await userManager.RegisterUserAsync(req);

                    // create team data
                    ServerContext.AppState.PlayerData[newUser.Identifier] = new UserTeam();

                    // create persistent data
                    var persistentDataService = new PlayerPersistentDataService(ServerContext);
                    await persistentDataService.CreatePersistentDataAsync(newUser.Identifier);

                    // Return user details
                    return Response.AsJsonNet(newUser);
                }
                catch (NullReferenceException)
                {
                    return HttpStatusCode.BadRequest;
                }
                catch (SecurityException sx)
                {
                    return Response.AsText(sx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
                catch (InvalidParameterException sx)
                {
                    return Response.AsText(sx.Message)
                        .WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            });

            // Log in with username and password
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

            // Allow changing passswords
            Patch("/changepassword", async args =>
            {
                var req = this.Bind<UserPasswordChangeRequest>();
                var selectedUser = await userManager.FindUserByUsernameAsync(req.Username);

                try
                {
                    // Validate password
                    if (req.NewPassword.Length < 8)
                    {
                        throw new InvalidParameterException("Password must be at least 8 characters.");
                    }

                    if (req.NewPassword.Length > 128)
                    {
                        throw new InvalidParameterException("Password may not exceed 128 characters.");
                    }

                    if (selectedUser.Enabled && await userManager.CheckPasswordAsync(req.OldPassword, selectedUser))
                    {
                        // Update password
                        await userManager.ChangeUserPasswordAsync(selectedUser, req.NewPassword);
                        return HttpStatusCode.OK;
                    }
                    return HttpStatusCode.Unauthorized;
                }
                catch (NullReferenceException)
                {
                    // A parameter was not provided
                    return new Response().WithStatusCode(HttpStatusCode.BadRequest);
                }
                catch (SecurityException secEx)
                {
                    // Registration blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
                catch (InvalidParameterException sx)
                {
                    return Response.AsText(sx.Message)
                        .WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            });

            // Authenticate with API key (used to validate a key during auto-login)
            Post("/reauth", async args =>
            {
                var req = this.Bind<UserReauthRequest>();
                var selectedUser = await userManager.FindUserByUsernameAsync(req.Username);

                if (selectedUser == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate key
                    if (selectedUser.Enabled && selectedUser.ApiKey == req.ApiKey)
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
                    // Blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
            });

            // Request generation of a new API key
            Patch("/newkey", async _ =>
            {
                var req = this.Bind<UserKeyResetRequest>();
                var selectedUser = await userManager.FindUserByUsernameAsync(req.Username);

                if (selectedUser == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate key
                    if (selectedUser.Enabled && selectedUser.ApiKey == req.ApiKey)
                    {                        
                        // Update key
                        await userManager.GenerateNewApiKeyAsync(selectedUser);
                        return HttpStatusCode.NoContent;
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
                    // Blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
            });
        }
    }
}
