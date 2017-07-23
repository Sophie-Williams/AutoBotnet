using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Models.Requests;
using Speercs.Server.Utilities;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Modules.User
{
    public class UserMetadataModule : UserApiModule
    {
        public UserMetadataModule(ISContext serverContext) : base("/user/meta", serverContext)
        {
            Get("/", _ => new SelfUser(CurrentUser));
            Put("/", async _ => {
                var userManager = new UserManagerService(ServerContext);
                var req = this.Bind<UserModificationRequest>();
                var newUser = CurrentUser;
                if (Request.Form.Email.HasValue) {
                    newUser.Email = req.Email;
                }
                if (Request.Form.Analytics.HasValue) {
                    newUser.AnalyticsEnabled = req.Analytics;
                }
                await userManager.UpdateUserInDatabaseAsync(newUser);
                return new SelfUser(newUser);
            });
            Get("/analytics", _ => ServerContext.AppState.UserAnalyticData[CurrentUser.Identifier]);
            Delete("/analytics", _ => {
                var analyticsObject = ServerContext.AppState.UserAnalyticData[CurrentUser.Identifier];
                analyticsObject = new UserAnalytics();
                return Response.AsJsonNet(analyticsObject);
            });
            Get("/player/{id}", async args =>
            {
                var user = await UserManager.FindUserByIdentifierAsync(((string)args.id));
                if (user == null) return HttpStatusCode.NotFound;
                var publicProfile = new PublicUser(user);
                return Response.AsJsonNet(publicProfile);
            });
        }
    }
}
