using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Models.Game.Map
{
    public struct RoomPosition
    {
        public int X { get; }
        public int Y { get; }
        public int RoomX { get; }
        public int RoomY { get; }
        
        // Constructors
        
        public RoomPosition(int roomX, int roomY, int x, int y)
        {
            RoomX = roomX;
            RoomY = roomY;
            X = x;
            Y = y;
        }
        
        public RoomPosition(Room room, int x, int y)
        {
            RoomX = room.X;
            RoomY = room.Y;
            X = x;
            Y = y;
        }
        
        // Methods
        
        public Room GetRoom(ISContext context)
        {
            return context.AppState.WorldMap[RoomX, RoomY];
        }
        
        public ITile GetTile(ISContext context)
        {
            return GetRoom(context).Tiles[X, Y];
        }
    }
}