using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests.User;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Metrics;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.User {
    public class UserMetadataModule : UserApiModule {
        public UserMetadataModule(ISContext serverContext) : base("/user", serverContext) {
            Get("/", _ => Response.asJsonNet(currentUser));

            Put("/", async _ => {
                var req = this.Bind<UserModificationRequest>();
                req.apply(currentUser);
                await userManager.updateUserInDatabaseAsync(currentUser);
                return Response.asJsonNet(currentUser);
            });

            // users own their data.
            Get("/metrics", _ => {
                var metricsService = new UserMetricsService(serverContext, currentUser.identifier);
                return Response.asJsonNet(metricsService.get());
            });
        }
    }
}