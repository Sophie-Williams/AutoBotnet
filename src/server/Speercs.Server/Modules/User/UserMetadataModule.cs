using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests.User;
using Speercs.Server.Models.User;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.User {
    public class UserMetadataModule : UserApiModule {
        public UserMetadataModule(ISContext serverContext) : base("/user/meta", serverContext) {
            Get("/", _ => Response.asJsonNet(currentUser));

            Put("/", async _ => {
                var req = this.Bind<UserModificationRequest>();
                req.apply(currentUser);
                await userManager.updateUserInDatabaseAsync(currentUser);
                return Response.asJsonNet(currentUser);
            });

            Get("/u/{id}", async args => {
                var user = await userManager.findUserByIdentifierAsync((string) args.id);
                if (user == null) return HttpStatusCode.NotFound;
                var publicProfile = new PublicUser(user);
                return Response.asJsonNet(publicProfile);
            });
        }
    }
}