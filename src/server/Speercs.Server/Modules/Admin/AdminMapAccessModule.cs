using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Requests.Game;

namespace Speercs.Server.Modules.Admin {
    public class AdminMapAccessModule : AdminApiModule {
        public AdminMapAccessModule(ISContext serverContext) : base("/map", serverContext) {
            Post("/genroom", _ => {
                var mapGen = new MapGenerator(this.serverContext);
                var req = this.Bind<RoomGenerationRequest>();
                if (this.serverContext.appState.worldMap[req.x, req.y] != null) return HttpStatusCode.BadRequest;
                var newRoom = req.density == 0
                    ? mapGen.generateRoom(req.x, req.y)
                    : mapGen.generateRoom(req.x, req.y, req.density);
                this.serverContext.appState.worldMap[req.x, req.y] = newRoom;
                return Response.AsJson(newRoom);
            });
        }
    }
}