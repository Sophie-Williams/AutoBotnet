using Nancy;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game
{
    public class UserMetadataModule : UserApiModule
    {
        public UserMetadataModule(ISContext serverContext) : base("/game/umeta", serverContext)
        {
            Get("/me", _ => (CurrentUser.Username));
            Get("/user/{id}", async args =>
            {
                var user = await UserManager.FindUserByIdentifierAsync(((string)args.id));
                if (user == null) return HttpStatusCode.NotFound;
                var publicProfile = new PublicUser(user);
                return Response.AsJsonNet(publicProfile);
            });
        }
    }
}
