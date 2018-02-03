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
        public int x { get; set; }
        public int y { get; set; }
        public int roomX { get; set; }
        public int roomY { get; set; }

        // Constructors

        public RoomPosition(int roomX, int roomY, int x, int y) {
            this.roomX = roomX;
            this.roomY = roomY;
            this.x = x;
            this.y = y;
        }

        public RoomPosition(Room room, int x, int y) {
            roomX = room.x;
            roomY = room.y;
            this.x = x;
            this.y = y;
        }

        public RoomPosition(RoomPosition room, int x, int y) {
            roomX = room.roomX;
            roomY = room.roomY;
            this.x = x;
            this.y = y;
        }

        // Methods

        public int distance(RoomPosition other) {
            return Room.MAP_EDGE_SIZE * (System.Math.Abs(roomX - other.roomX) + System.Math.Abs(roomY - other.roomY)) +
                   System.Math.Abs(x - other.x) + System.Math.Abs(y - other.y);
        }

        public int distance(GameEntity entity) {
            return distance(entity.position);
        }

        public T getClosestEntity<T>(ISContext context) where T : GameEntity {
            var _this = this;
            return getRoom(context).entities
                .Select(entry => entry.Value)
                .OfType<T>()
                .MinBy(entity => _this.distance(entity));
        }

        public T getClosestEntity<T>(ISContext context, Func<T, bool> predicate) where T : GameEntity {
            var _this = this;
            return getRoom(context).entities
                .Select(entry => entry.Value)
                .OfType<T>()
                .Where(predicate)
                .MinBy(entity => _this.distance(entity));
        }

        public List<RoomPosition> pathTo(ISContext context, RoomPosition goal) {
            return new Pathfinder(context, this, goal).findPath();
        }

        public List<RoomPosition> pathTo(ISContext context, RoomPosition goal, Predicate<ITile> passable) {
            return new Pathfinder(context, this, goal, passable).findPath();
        }

        public Room getRoom(ISContext context) {
            return context.appState.worldMap[roomX, roomY];
        }

        public ITile getTile(ISContext context) {
            return getRoom(context).tiles[x, y];
        }


        public override bool Equals(object obj) {
            return obj is RoomPosition && this == (RoomPosition) obj;
        }

        public override int GetHashCode() {
            unchecked {
                var hash = 17;
                hash = hash * 31 + x;
                hash = hash * 31 + y;
                hash = hash * 31 + roomX;
                hash = hash * 31 + roomY;
                return hash;
            }
        }

        public static bool operator ==(RoomPosition a, RoomPosition b) {
            return a.x == b.x && a.y == b.y && a.roomX == b.roomX && a.roomY == b.roomY;
        }

        public static bool operator !=(RoomPosition a, RoomPosition b) {
            return a.x != b.x || a.y != b.y || a.roomX != b.roomX || a.roomY != b.roomY;
        }
    }
}