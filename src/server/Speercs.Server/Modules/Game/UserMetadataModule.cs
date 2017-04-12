using Speercs.Server.Configuration;

namespace Speercs.Server.Modules.Game
{
    public class UserMetadataModule : UserApiModule
    {
        public UserMetadataModule(ISContext serverContext) : base("/game/umeta", serverContext)
        {
            Get("/me", async _ => (await UserManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name)).Username);
        }
    }
}