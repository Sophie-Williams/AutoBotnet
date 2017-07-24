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
                var user = await userManager.FindUserByIdentifierAsync((string)args.id);
                if (user == null) return HttpStatusCode.NotFound;
                return Response.AsJsonNet((RegisteredUser) user);
            });

            Put("/{id}", async args =>
            {
                var userManager = new UserManagerService(ServerContext);
                var req = this.Bind<AdminUserModificationRequest>();
                var user = await userManager.FindUserByIdentifierAsync((string)args.id);
                user.AnalyticsEnabled = req.AnalyticsEnabled;
                user.Email = req.Email;
                user.Enabled = req.Enabled;
                await userManager.UpdateUserInDatabaseAsync(user);
                return Response.AsJsonNet(user);
            });

            Delete("/{id}", async args =>
            {
                var userManager = new UserManagerService(ServerContext);
                await userManager.DeleteUserAsync((string)args.id);
                return HttpStatusCode.OK;
            });

            Get("/analytics/{id}", args => Response.AsJsonNet(ServerContext.AppState.UserAnalyticsData[(string)args.id]));

            Delete("/analytics/{id}", args =>
            {
                ServerContext.AppState.UserAnalyticsData[(string)args.id] = new UserAnalytics();
                return HttpStatusCode.OK;
            });
        }
    }
}

