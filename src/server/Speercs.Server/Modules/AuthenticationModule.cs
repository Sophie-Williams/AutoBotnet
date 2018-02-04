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
using Speercs.Server.Models.Requests.User;

namespace Speercs.Server.Modules {
    public class AuthenticationModule : SBaseModule {
        private UserManagerService _userManager;

        public ISContext serverContext { get; private set; }

        public AuthenticationModule(ISContext serverContext) : base("/auth") {
            this.serverContext = serverContext;
            Before += ctx => {
                _userManager = new UserManagerService(this.serverContext);
                return null;
            };

            // Register account
            Post("/register", async args => {
                var charsetRegex = new Regex(@"^[a-zA-Z0-9._-]{3,24}$");

                var req = this.Bind<UserRegistrationRequest>();

                try {
                    if (this.serverContext.configuration.maxUsers > -1 &&
                        _userManager.registeredUserCount >= this.serverContext.configuration.maxUsers) {
                        throw new SecurityException("Maximum number of users for this server reached");
                    }

                    // Valdiate username length
                    if (req.username.Length < 2) {
                        throw new InvalidParameterException("Username must be at least 2 characters.");
                    }

                    if (req.username.Length > 24) {
                        throw new InvalidParameterException("Username may not exceed 24 characters.");
                    }

                    // Validate username charset
                    if (charsetRegex.Matches(req.username).Count <= 0) {
                        throw new InvalidParameterException("Invalid character in username.");
                    }

                    // Validate password
                    if (req.password.Length < 8) {
                        throw new InvalidParameterException("Password must be at least 8 characters.");
                    }

                    if (req.password.Length > 128) {
                        throw new InvalidParameterException("Password may not exceed 128 characters.");
                    }

                    // Check invite key if enabled
                    if (this.serverContext.configuration.inviteRequired) {
                        // Validate invite key
                        if (!this.serverContext.appState.inviteKeys.Remove(req.inviteKey)) {
                            return HttpStatusCode.PaymentRequired;
                        }
                    }

                    // Attempt to register user
                    var newUser = await _userManager.registerUserAsync(req);

                    // queue persist
                    this.serverContext.appState.queuePersist();

                    // Return user details
                    return Response.asJsonNet(newUser);
                } catch (NullReferenceException) {
                    return HttpStatusCode.BadRequest;
                } catch (SecurityException sx) {
                    return Response.AsText(sx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                } catch (InvalidParameterException sx) {
                    return Response.AsText(sx.Message)
                        .WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            });

            // Log in with username and password
            Post("/login", async args => {
                var req = this.Bind<UserLoginRequest>();
                var user = await _userManager.findUserByUsernameAsync(req.username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try {
                    // Validate password
                    if (user.enabled && await _userManager.checkPasswordAsync(req.password, user)) {
                        // Return user details
                        return Response.asJsonNet(user);
                    }

                    return HttpStatusCode.Unauthorized;
                } catch (NullReferenceException) {
                    // A parameter was not provided
                    return HttpStatusCode.BadRequest;
                } catch (SecurityException secEx) {
                    // Registration blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
            });

            Delete("/delete", async args => {
                // Login fields are the same as those for account deletion
                var req = this.Bind<UserLoginRequest>();

                var user = await _userManager.findUserByUsernameAsync(req.username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try {
                    // Validate password
                    if (user.enabled && await _userManager.checkPasswordAsync(req.password, user)) {
                        // Password was correct, delete account
                        await _userManager.deleteUserAsync(user.identifier);

                        // queue persist
                        this.serverContext.appState.queuePersist();

                        return HttpStatusCode.OK;
                    }

                    return HttpStatusCode.Unauthorized;
                } catch { return HttpStatusCode.Unauthorized; }
            });

            // Allow changing passswords
            Patch("/changepassword", async args => {
                var req = this.Bind<UserPasswordChangeRequest>();
                var user = await _userManager.findUserByUsernameAsync(req.username);

                try {
                    // Validate password
                    if (req.newPassword.Length < 8) {
                        throw new InvalidParameterException("Password must be at least 8 characters.");
                    }

                    if (req.newPassword.Length > 128) {
                        throw new InvalidParameterException("Password may not exceed 128 characters.");
                    }

                    if (user.enabled && await _userManager.checkPasswordAsync(req.oldPassword, user)) {
                        // Update password
                        await _userManager.changeUserPasswordAsync(user, req.newPassword);
                        return HttpStatusCode.OK;
                    }

                    return HttpStatusCode.Unauthorized;
                } catch (NullReferenceException) {
                    // A parameter was not provided
                    return new Response().WithStatusCode(HttpStatusCode.BadRequest);
                } catch (SecurityException secEx) {
                    // Registration blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                } catch (InvalidParameterException sx) {
                    return Response.AsText(sx.Message)
                        .WithStatusCode(HttpStatusCode.UnprocessableEntity);
                }
            });

            // Authenticate with API key (used to validate a key during auto-login)
            Post("/reauth", async args => {
                var req = this.Bind<UserReauthRequest>();
                var user = await _userManager.findUserByUsernameAsync(req.username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try {
                    // Validate key
                    if (user.enabled && user.apiKey == req.apiKey) {
                        // Return user details
                        return Response.asJsonNet(user);
                    }

                    return HttpStatusCode.Unauthorized;
                } catch (NullReferenceException) {
                    // A parameter was not provided
                    return HttpStatusCode.BadRequest;
                } catch (SecurityException secEx) {
                    // Blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
            });

            // Request generation of a new API key
            Patch("/newkey", async _ => {
                var req = this.Bind<UserKeyResetRequest>();
                var user = await _userManager.findUserByUsernameAsync(req.username);

                if (user == null) return HttpStatusCode.Unauthorized;

                try {
                    // Validate key
                    if (user.enabled && user.apiKey == req.apiKey) {
                        // Update key
                        await _userManager.generateNewApiKeyAsync(user);
                        return HttpStatusCode.NoContent;
                    }

                    return HttpStatusCode.Unauthorized;
                } catch (NullReferenceException) {
                    // A parameter was not provided
                    return HttpStatusCode.BadRequest;
                } catch (SecurityException secEx) {
                    // Blocked for security reasons
                    return Response.AsText(secEx.Message)
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
            });
        }
    }
}