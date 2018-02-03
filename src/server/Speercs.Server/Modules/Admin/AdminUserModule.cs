using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Services.Auth;
using Speercs.Server.Utilities;
using Speercs.Server.Models.User;

namespace Speercs.Server.Modules.Admin {
    public class AdminUserModule : AdminApiModule {
        public AdminUserModule(ISContext serverContext) : base("/user", serverContext) {
            Get("/{id}", async args => {
                var userManager = new UserManagerService(base.serverContext);
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                if (user == null) return HttpStatusCode.NotFound;
                return Response.asJsonNet((RegisteredUser) user);
            });

            Put("/{id}", async args => {
                var userManager = new UserManagerService(base.serverContext);
                var req = this.Bind<AdminUserModificationRequest>();
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                user.analyticsEnabled = req.analyticsEnabled;
                user.email = req.email;
                user.enabled = req.enabled;
                await userManager.updateUserInDatabaseAsync(user);
                return Response.asJsonNet(user);
            });

            Delete("/{id}", async args => {
                var userManager = new UserManagerService(base.serverContext);
                await userManager.deleteUserAsync((string) args.id);
                return HttpStatusCode.OK;
            });

            Get("/analytics/{id}",
                args => Response.asJsonNet(base.serverContext.appState.userAnalyticsData[(string) args.id]));

            Delete("/analytics/{id}", args => {
                base.serverContext.appState.userAnalyticsData[(string) args.id] = new UserAnalytics();
                return HttpStatusCode.OK;
            });
        }
    }
}