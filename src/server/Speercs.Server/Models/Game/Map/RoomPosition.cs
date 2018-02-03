using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using MoreLinq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Models.Game.Map {
    public struct RoomPosition {
        public int X { get; set; }
        public int Y { get; set; }
        public int RoomX { get; set; }
        public int RoomY { get; set; }

        // Constructors

        public RoomPosition(int roomX, int roomY, int x, int y) {
            RoomX = roomX;
            RoomY = roomY;
            X = x;
            Y = y;
        }

        public RoomPosition(Room room, int x, int y) {
            RoomX = room.X;
            RoomY = room.Y;
            X = x;
            Y = y;
        }

        public RoomPosition(RoomPosition room, int x, int y) {
            RoomX = room.RoomX;
            RoomY = room.RoomY;
            X = x;
            Y = y;
        }

        // Methods

        public int Distance(RoomPosition other) {
            return Room.MapEdgeSize * (System.Math.Abs(RoomX - other.RoomX) + System.Math.Abs(RoomY - other.RoomY)) +
                   System.Math.Abs(X - other.X) + System.Math.Abs(Y - other.Y);
        }

        public int Distance(GameEntity entity) {
            return Distance(entity.Position);
        }

        public T GetClosestEntity<T>(ISContext context) where T : GameEntity {
            var _this = this;
            return GetRoom(context).Entities
                .Select(entry => entry.Value)
                .OfType<T>()
                .MinBy(entity => _this.Distance(entity));
        }

        public T GetClosestEntity<T>(ISContext context, Func<T, bool> predicate) where T : GameEntity {
            var _this = this;
            return GetRoom(context).Entities
                .Select(entry => entry.Value)
                .OfType<T>()
                .Where(predicate)
                .MinBy(entity => _this.Distance(entity));
        }

        public List<RoomPosition> PathTo(ISContext context, RoomPosition goal) {
            return new Pathfinder(context, this, goal).FindPath();
        }

        public List<RoomPosition> PathTo(ISContext context, RoomPosition goal, Predicate<ITile> passable) {
            return new Pathfinder(context, this, goal, passable).FindPath();
        }

        public Room GetRoom(ISContext context) {
            return context.AppState.WorldMap[RoomX, RoomY];
        }

        public ITile GetTile(ISContext context) {
            return GetRoom(context).Tiles[X, Y];
        }


        public override bool Equals(object obj) {
            return obj is RoomPosition && this == (RoomPosition) obj;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 31 + X;
                hash = hash * 31 + Y;
                hash = hash * 31 + RoomX;
                hash = hash * 31 + RoomY;
                return hash;
            }
        }

        public static bool operator ==(RoomPosition a, RoomPosition b) {
            return a.X == b.X && a.Y == b.Y && a.RoomX == b.RoomX && a.RoomY == b.RoomY;
        }

        public static bool operator !=(RoomPosition a, RoomPosition b) {
            return a.X != b.X || a.Y != b.Y || a.RoomX != b.RoomX || a.RoomY != b.RoomY;
        }
    }
}