using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Utilities;
using System.Collections.Generic;

namespace Speercs.Server.Modules.Admin
{
    public class AdminInviteKeysModule : AdminApiModule
    {
        public AdminInviteKeysModule(ISContext serverContext) : base("/keys", serverContext)
        {
            Post("/gen", _ =>
            {
                var req = this.Bind<KeyGenerationRequest>();
                if (req.Amount < 1) return HttpStatusCode.BadRequest;
                var newCodes = new List<string>();
                for (var i = 0; i < req.Amount; i++)
                {
                    newCodes.Add(StringUtils.SecureRandomString(16));
                }
                ServerContext.AppState.InviteKeys.AddRange(newCodes);
                return Response.AsJsonNet(newCodes);
            });

            Get("/active", _ => Response.AsJsonNet(ServerContext.AppState.InviteKeys));

            Delete("/delete/{key}", (args) =>
            {
                return ServerContext.AppState.InviteKeys.Remove(args.key);
            });
        }
    }
}
