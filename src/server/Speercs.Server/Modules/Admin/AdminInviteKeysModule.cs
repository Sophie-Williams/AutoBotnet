using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests;
using Speercs.Server.Utilities;
using System.Collections.Generic;
using Speercs.Server.Models.Requests.Game;

namespace Speercs.Server.Modules.Admin {
    public class AdminInviteKeysModule : AdminApiModule {
        public AdminInviteKeysModule(ISContext serverContext) : base("/keys", serverContext) {
            Post("/gen", _ => {
                var req = this.Bind<KeyGenerationRequest>();
                if (req.amount < 1) return HttpStatusCode.BadRequest;
                var newCodes = new List<string>();
                for (var i = 0; i < req.amount; i++) {
                    newCodes.Add(StringUtils.secureRandomString(16));
                }

                base.serverContext.appState.inviteKeys.AddRange(newCodes);
                return Response.asJsonNet(newCodes);
            });

            Get("/active", _ => Response.asJsonNet(base.serverContext.appState.inviteKeys));

            Delete("/delete/{key}", (args) => {
                return base.serverContext.appState.inviteKeys.Remove((string) args.key);
            });
        }
    }
}