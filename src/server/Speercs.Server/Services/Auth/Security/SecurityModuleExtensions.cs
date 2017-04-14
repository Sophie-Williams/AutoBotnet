using Nancy;
using System.Security.Claims;

namespace Speercs.Server.Services.Auth.Security
{
    public static class ApiAccessModuleSecurityExtensions
    {
        static Claim AdminClaim = new Claim(ApiAuthenticator.AuthTypeClaimKey, ApiAccessScope.Admin.ToString());
        static Claim UserClaim = new Claim(ApiAuthenticator.AuthTypeClaimKey, ApiAccessScope.User.ToString());

        public static void RequiresUserAuthentication(this NancyModule module)
        {
            InjectAuthenticationHook(module, UserClaim);
        }

        public static void RequiresAdminAuthentication(this NancyModule module)
        {
            InjectAuthenticationHook(module, AdminClaim);
        }

        public static void InjectAuthenticationHook(NancyModule module, Claim requiredClaim)
        {
            module.Before.AddItemToEndOfPipeline((ctx) =>
            {
                if (ctx.CurrentUser == null)
                {
                    return HttpStatusCode.Unauthorized;
                }
                
                if (ctx.CurrentUser.EnsureClaim(requiredClaim))
                {
                    return null;
                }
                return HttpStatusCode.Unauthorized;
            });
        }

        /// <summary>
        /// Ensures that a ClaimsPrincipal posesses a claim by checking the Type and Value fields
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static bool EnsureClaim(this ClaimsPrincipal principal, Claim claim)
        {
            return principal.HasClaim(claim.Type, claim.Value);
        }
    }
}