using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Speercs.Server.Models.User;

namespace Speercs.Server.Web.Realtime
{
    public class RealtimeContext : DependencyObject
    {
        public RealtimeContext(ISContext context) : base(context)
        {
        }

        public ClaimsPrincipal Identity { get; private set; }

        public string UserIdentifier { get; private set; }

        public bool AuthenticateWith(string apikey)
        {
            // call authenticator
            var auther = new ApiAuthenticator(ServerContext);
            Identity = auther.ResolveIdentity(apikey);
            UserIdentifier = Identity.Claims.FirstOrDefault(x => x.Type == ApiAuthenticator.UserIdentifierClaimKey).Value;
            return Identity != null;
        }

        public async Task<RegisteredUser> GetCurrentUserAsync()
        {
            var userManager = new UserManagerService(ServerContext);
            return await userManager.FindUserByIdentifierAsync(UserIdentifier);
        }
    }
}
