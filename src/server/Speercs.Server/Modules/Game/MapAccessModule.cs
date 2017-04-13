using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Requests;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game
{
    public class MapAccessModule : UserApiModule
    {
        public MapAccessModule(ISContext serverContext) : base("/game/map", serverContext)
        {
            Get("/", async _ => (await UserManager.FindUserByIdentifierAsync(Context.CurrentUser.Identity.Name)).Username);

            Get("/room", _ =>
            {
                if (!int.TryParse(((string)Request.Query.x), out int x)) return HttpStatusCode.BadRequest;
                if (!int.TryParse(((string)Request.Query.y), out int y)) return HttpStatusCode.BadRequest;
                var room = ServerContext.AppState.WorldMap[x, y];
                if (room == null) return HttpStatusCode.NotFound;
                return Response.AsJsonNet(room);
            });

            Post("/genroom", _ =>
            {
                MapGenerator mapGen = new MapGenerator();
                var req = this.Bind<RoomGenerationRequest>();
                if (ServerContext.AppState.WorldMap[req.X, req.Y] != null) return HttpStatusCode.BadRequest;
                var newRoom = mapGen.GenerateRoom(req.Density);
                ServerContext.AppState.WorldMap[req.X, req.Y] = newRoom;
                return Response.AsJson(newRoom);
            });
        }
    }
}
