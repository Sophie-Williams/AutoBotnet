using ImageSharp.Formats;
using Nancy;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Utilities;
using System.IO;

namespace Speercs.Server.Modules.Game {
    public class MapImageModule : SBaseModule {
        public MapImageModule(ISContext serverContext) : base("/map") {
            Get("/map.png", _ => {
                MemoryStream stream = new MemoryStream();
                new RoomImage().drawMap(serverContext.appState.worldMap).Save(stream, new PngEncoder());
                stream.Position = 0;
                return Response.FromStream(stream, "image/png");
            });
            Get("/room/{x:int}/{y:int}.png", (parameters) => {
                Room room = serverContext.appState.worldMap[parameters.x, parameters.y];
                if (room == null) return HttpStatusCode.NotFound;
                MemoryStream stream = new MemoryStream();
                new RoomImage().drawRoom(room).Save(stream, new PngEncoder());
                stream.Position = 0;
                return Response.FromStream(stream, "image/png");
            });
        }
    }
}