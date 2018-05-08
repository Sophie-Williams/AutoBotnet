using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Web.Realtime {
    public class RealtimeContext : DependencyObject {
        public RealtimeContext(ISContext context) : base(context) { }

        public ClaimsPrincipal identity { get; private set; }

        public string userId { get; private set; }

        public bool authenticateWith(string apikey) {
            // call authenticator
            var auther = new ApiAuthenticator(serverContext);
            identity = auther.resolveIdentity(apikey);
            userId = identity.Claims.FirstOrDefault(x => x.Type == ApiAuthenticator.USER_IDENTIFIER_CLAIM_KEY)
                ?.Value;
            return identity != null;
        }

        public async Task<RegisteredUser> getCurrentUserAsync() {
            var userManager = new UserManagerService(serverContext);
            return await userManager.findUserByIdentifierAsync(userId);
        }
    }
}