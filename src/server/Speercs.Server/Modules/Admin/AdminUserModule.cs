using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Services.Auth;
using Speercs.Server.Utilities;
using Speercs.Server.Models.User;

namespace Speercs.Server.Modules.Admin
{
    public class AdminUserModule : AdminApiModule
    {
        public AdminUserModule(ISContext serverContext) : base("/user", serverContext)
        {
            Get("/{id}", async args =>
            {
                var userManager = new UserManagerService(ServerContext);
                var user = await userManager.FindUserByIdentifierAsync(args.id);
                if (user == null) return HttpStatusCode.NotFound;
                return Response.AsJsonNet((RegisteredUser)user);
            });
        }
    }
}

