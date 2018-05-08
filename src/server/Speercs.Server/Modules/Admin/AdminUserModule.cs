using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests.User;
using Speercs.Server.Services.Metrics;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Admin {
    public class AdminUserModule : AdminApiModule {
        public AdminUserModule(ISContext serverContext) : base("/user", serverContext) {
            Get("/{id}", async args => {
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                if (user == null) return HttpStatusCode.NotFound;
                return Response.asJsonNet(user);
            });

            Get("/metrics/{id}", async args => {
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                if (user == null) return HttpStatusCode.NotFound;
                var metricsService = new UserMetricsService(serverContext, (string) args.id);
                return Response.asJsonNet(metricsService.get());
            });

            Put("/{id}", async args => {
                var req = this.Bind<AdminUserModificationRequest>();
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                user.email = req.email;
                user.enabled = req.enabled;
                await userManager.updateUserInDatabaseAsync(user);
                return Response.asJsonNet(user);
            });
        }
    }
}