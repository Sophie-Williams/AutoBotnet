using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Models.Game.Entities;

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
        
        public int Distance(RoomPosition other)
        {
            return Room.MapEdgeSize*((RoomX-other.RoomX) + (RoomY-other.RoomY)) +
                   (X-other.X) + (Y-other.Y);
        }
        public int Distance(GameEntity entity)
        {
            return Distance(entity.Position);
        }
        
        public T GetClosestEntity<T>(ISContext context) where T : GameEntity
        {
            var _this = this;
            return GetRoom(context).Entities
                    .Select(entry => entry.Value)
                    .OfType<T>()
                    .MinBy(entity => _this.Distance(entity));
        }
        
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