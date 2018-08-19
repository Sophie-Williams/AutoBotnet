using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Meta;
using Speercs.Server.Models.Requests.Game;
using Speercs.Server.Services.Application;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Admin {
    public class AdminInviteKeysModule : AdminApiModule {
        public AdminInviteKeysModule(ISContext serverContext) : base("/keys", serverContext) {
            Post("/gen", _ => {
                var req = this.Bind<KeyGenerationRequest>();
                if (req.amount < 1) return HttpStatusCode.BadRequest;
                var newCodes = new List<InviteKey>();
                var creationTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                for (var i = 0; i < req.amount; i++) {
                    newCodes.Add(new InviteKey {
                        key = StringUtils.secureRandomString(16),
                        timestamp = creationTime
                    });
                }

                serverContext.log.writeLine($"Admin generated {req.amount} invite keys",
                    SpeercsLogger.LogLevel.Information);
                var inviteKeys =
                    this.serverContext.database.GetCollection<InviteKey>(DatabaseKeys.COLLECTION_INVITEKEYS);
                inviteKeys.InsertBulk(newCodes);
                inviteKeys.EnsureIndex(x => x.key);
                return Response.asJsonNet(newCodes);
            });

            Get("/active", _ => {
                var inviteKeys =
                    this.serverContext.database.GetCollection<InviteKey>(DatabaseKeys.COLLECTION_INVITEKEYS);
                return Response.asJsonNet(inviteKeys);
            });

            Delete("/delete/{key}", args => {
                var inviteKeys =
                    this.serverContext.database.GetCollection<InviteKey>(DatabaseKeys.COLLECTION_INVITEKEYS);
                var key = (string) args.key;
                return inviteKeys.Delete(x => x.key == key) > 0
                    ? HttpStatusCode.NoContent
                    : HttpStatusCode.NotFound;
            });
        }
    }
}