using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Speercs.Server.Models.User;

namespace Speercs.Server.Web.Realtime {
    public class RealtimeContext : DependencyObject {
        public RealtimeContext(ISContext context) : base(context) { }

        public ClaimsPrincipal identity { get; private set; }

        public string userIdentifier { get; private set; }

        public bool authenticateWith(string apikey) {
            // call authenticator
            var auther = new ApiAuthenticator(serverContext);
            identity = auther.resolveIdentity(apikey);
            userIdentifier = identity.Claims.FirstOrDefault(x => x.Type == ApiAuthenticator.USER_IDENTIFIER_CLAIM_KEY)
                .Value;
            return identity != null;
        }

        public async Task<RegisteredUser> getCurrentUserAsync() {
            var userManager = new UserManagerService(serverContext);
            return await userManager.findUserByIdentifierAsync(userIdentifier);
        }
    }
}