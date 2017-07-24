using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Services.Auth;
using Speercs.Server.Utilities;
using Speercs.Server.Models.User;

namespace Speercs.Server.Modules.Admin
{
    public class AdminUserModule : AdminApiModule
    {
        public AdminUserModule(ISContext serverContext) : base("/user", serverContext)
        {
            Get("/{id}", async args =>
            {
                var userManager = new UserManagerService(ServerContext);
                var user = await userManager.FindUserByIdentifierAsync(args.id);
                if (user == null) return HttpStatusCode.NotFound;
                return Response.AsJsonNet((RegisteredUser) user);
            });

            Put("/{id}", async args =>
            {
                var userManager = new UserManagerService(ServerContext);
                var req = this.Bind<AdminUserModificationRequest>();
                var newUser = userManager.FindUserByIdentifierAsync(args.id);
                if (Request.Form.Email.HasValue)
                {
                    newUser.AnalyticsEnabled = req.Email;
                }
                if (Request.Form.Username.HasValue)
                {
                    newUser.Username = req.Email;
                }
                if (Request.Form.Enabled.HasValue)
                {
                    newUser.Enabled = req.Enabled;
                }
                await userManager.UpdateUserInDatabaseAsync(newUser);
                return Response.AsJsonNet((RegisteredUser) newUser);
            });

            Delete("/{id}", async args => {
                var userManager = new UserManagerService(ServerContext);
                await userManager.DeleteUserAsync(args.id);
                return HttpStatusCode.OK;
            });

            Get("/{id}/analytics", args => Response.AsJsonNet((UserAnalytics) ServerContext.AppState.UserAnalyticsData[args.id]));

            Delete("/{id}/analytics", args =>
            {
                ServerContext.AppState.UserAnalyticsData[args.id] = new UserAnalytics();
                return HttpStatusCode.OK;
            });
        }
    }
}

