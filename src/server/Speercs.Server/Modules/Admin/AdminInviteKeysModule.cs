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
                if (req.Ammount < 1) return HttpStatusCode.BadRequest;
                var newCodes = new List<string>();
                for (var i=0; i < req.Ammount; i++) {
                    newCodes.Add(StringUtils.SecureRandomString(16));
                }
                ServerContext.AppState.InviteKeys.AddRange(newCodes);
                return Response.AsJson(newCodes);
            });

            Get("/active", _ => Response.AsJson(ServerContext.AppState.InviteKeys));

            Delete("/delete", _ => {
                var req = this.Bind<KeyDeletionRequest>();
                if (req.Keys.Count < 1) return HttpStatusCode.BadRequest;
                foreach (var key in req.Keys) {
                    ServerContext.AppState.InviteKeys.Remove(key);
                }
                return true;
            });
        }
    }
}
