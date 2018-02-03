using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Models.Requests;
using Speercs.Server.Utilities;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Modules.User {
    public class UserMetadataModule : UserApiModule {
        public UserMetadataModule(ISContext serverContext) : base("/user/meta", serverContext) {
            Get("/", _ => Response.asJsonNet(new SelfUser(currentUser)));

            Put("/", async _ => {
                var userManager = new UserManagerService(base.serverContext);
                var req = this.Bind<UserModificationRequest>();
                var newUser = currentUser;
                newUser.email = req.email;
                newUser.analyticsEnabled = req.analyticsEnabled;
                await userManager.updateUserInDatabaseAsync(newUser);
                return Response.asJsonNet(new SelfUser(newUser));
            });

            Get("/analytics",
                _ => Response.asJsonNet(base.serverContext.appState.userAnalyticsData[currentUser.identifier]));

            Delete("/analytics", _ => {
                base.serverContext.appState.userAnalyticsData[currentUser.identifier] = new UserAnalytics();
                return HttpStatusCode.OK;
            });

            Get("/player/{id}", async args => {
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                if (user == null) return HttpStatusCode.NotFound;
                var publicProfile = new PublicUser(user);
                return Response.asJsonNet(publicProfile);
            });
        }
    }
}