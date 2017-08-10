using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Requests;
using Speercs.Server.Modules.Exceptions;
using Speercs.Server.Services.Auth;
using Speercs.Server.Models.User;
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
                    if (ServerContext.Configuration.MaxUsers > -1 && userManager.RegisteredUserCount >= ServerContext.Configuration.MaxUsers)
                    {
                        throw new SecurityException("Maximum number of users for this server reached");
                    }
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
                    if (ServerContext.Configuration.InviteRequired)
                    {
                        // Validate invite key
                        if (!ServerContext.AppState.InviteKeys.Remove(req.InviteKey))
                        {
                            return HttpStatusCode.PaymentRequired;
                        }
                    }

                    // Attempt to register user
                    var newUser = await userManager.RegisterUserAsync(req);

                    // queue persist
                    ServerContext.AppState.QueuePersist();

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
                var user = await userManager.FindUserByUsernameAsync(req.Username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate password
                    if (user.Enabled && await userManager.CheckPasswordAsync(req.Password, user))
                    {
                        // Return user details
                        return Response.AsJsonNet(user);
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

            Delete("/delete", async args =>
            {
                // Login fields are the same as those for account deletion
                var req = this.Bind<UserLoginRequest>();

                var user = await userManager.FindUserByUsernameAsync(req.Username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate password
                    if (user.Enabled && await userManager.CheckPasswordAsync(req.Password, user))
                    {
                        // Password was correct, delete account
                        await userManager.DeleteUserAsync(user.Identifier);

                        // queue persist
                        ServerContext.AppState.QueuePersist();

                        return HttpStatusCode.OK;
                    }
                    return HttpStatusCode.Unauthorized;
                }
                catch { return HttpStatusCode.Unauthorized; }
            });

            // Allow changing passswords
            Patch("/changepassword", async args =>
            {
                var req = this.Bind<UserPasswordChangeRequest>();
                var user = await userManager.FindUserByUsernameAsync(req.Username);

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

                    if (user.Enabled && await userManager.CheckPasswordAsync(req.OldPassword, user))
                    {
                        // Update password
                        await userManager.ChangeUserPasswordAsync(user, req.NewPassword);
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
                var user = await userManager.FindUserByUsernameAsync(req.Username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate key
                    if (user.Enabled && user.ApiKey == req.ApiKey)
                    {
                        // Return user details
                        return Response.AsJsonNet(user);
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
                var user = await userManager.FindUserByUsernameAsync(req.Username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try
                {
                    // Validate key
                    if (user.Enabled && user.ApiKey == req.ApiKey)
                    {
                        // Update key
                        await userManager.GenerateNewApiKeyAsync(user);
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
