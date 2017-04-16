using System.Security.Claims;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Web.Realtime
{
    public class RealtimeContext : DependencyObject
    {
        public RealtimeContext(ISContext context) : base(context)
        {
        }

        public ClaimsPrincipal Identity { get; private set; }

        public bool AuthenticateWith(string apikey)
        {
            // call authenticator
            var auther = new ApiAuthenticator(ServerContext);
            Identity = auther.ResolveIdentity(apikey);
            return Identity != null;
        }
    }
}
