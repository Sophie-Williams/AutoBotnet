using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Requests;

namespace Speercs.Server.Modules.Admin
{
    public class AdminMapAccessModule : AdminApiModule
    {
        public AdminMapAccessModule(ISContext serverContext) : base("/map", serverContext)
        {
            Post("/genroom", _ =>
            {
                MapGenerator mapGen = new MapGenerator(ServerContext);
                var req = this.Bind<RoomGenerationRequest>();
                if (ServerContext.AppState.WorldMap[req.X, req.Y] != null) return HttpStatusCode.BadRequest;
                var newRoom = mapGen.GenerateRoom(req.X, req.Y, req.Density);
                ServerContext.AppState.WorldMap[req.X, req.Y] = newRoom;
                return Response.AsJson(newRoom);
            });
        }
    }
}
