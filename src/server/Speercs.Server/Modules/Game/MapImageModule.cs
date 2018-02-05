using System.IO;
using Nancy;
using SixLabors.ImageSharp.Formats.Png;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Misc;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game {
    public class MapImageModule : SBaseModule {
        public MapImageModule(ISContext serverContext) : base("/map", serverContext) {
            Get("/map.png", _ => {
                var stream = new MemoryStream();
                new RoomImageRenderer().drawMap(serverContext.appState.worldMap).Save(stream, new PngEncoder());
                stream.Position = 0;
                return Response.FromStream(stream, "image/png");
            });
            Get("/room/{x:int}/{y:int}.png", parameters => {
                Room room = serverContext.appState.worldMap[parameters.x, parameters.y];
                if (room == null) return HttpStatusCode.NotFound;
                var stream = new MemoryStream();
                new RoomImageRenderer().drawRoom(room).Save(stream, new PngEncoder());
                stream.Position = 0;
                return Response.FromStream(stream, "image/png");
            });
        }
    }
}