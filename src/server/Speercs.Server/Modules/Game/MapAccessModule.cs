using Nancy;
using Speercs.Server.Configuration;
using Speercs.Server.Modules.User;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game {
    public class MapAccessModule : UserApiModule {
        public MapAccessModule(ISContext serverContext) : base("/game/map", serverContext) {
            Get("/room", _ => {
                if (!int.TryParse(((string) Request.Query.x), out var x)) return HttpStatusCode.BadRequest;
                if (!int.TryParse(((string) Request.Query.y), out var y)) return HttpStatusCode.BadRequest;
                var room = this.serverContext.appState.worldMap[x, y];
                if (room == null) return HttpStatusCode.PreconditionFailed;
                return Response.asJsonNet(room);
            });
        }
    }
}