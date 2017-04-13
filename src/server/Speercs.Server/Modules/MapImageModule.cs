using ImageSharp;
using Nancy;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Utilities;
using Speercs.Server.Configuration;
using ImageSharp.Formats;
using System.IO;

namespace Speercs.Server.Modules.Game
{
    public class MapImageModule : NancyModule
    {
        public MapImageModule(ISContext serverContext)
        {
            Get("/map.png", _ => {
                Image img = new Image(Room.MapEdgeSize, Room.MapEdgeSize);

                return "";
            });
            Get("/room/{x:int}/{y:int}.png", (parameters) => {
                Room room = serverContext.AppState.WorldMap[parameters.x, parameters.y];
                if (room == null) return HttpStatusCode.NotFound;
                MemoryStream stream = new MemoryStream();
                new RoomImage().drawRoom(room).Save(stream, new PngEncoder());
                stream.Position = 0;
                return Response.FromStream(stream, "image/png");
            });
        }
    }
}