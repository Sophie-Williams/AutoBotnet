using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Map {
    public struct RoomPosition {
        public readonly Point pos;
        public readonly Point roomPos;

        // Constructors

        public RoomPosition(Point roomPos, Point pos) {
            this.roomPos = roomPos;
            this.pos = pos;
        }

        public RoomPosition(Room room, Point pos) : this(new Point(room.x, room.y), pos) { }

        // Methods

        /// <summary>
        /// Distance (possibly spanning multiple rooms)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double distance(RoomPosition other) {
            return (Point.distance(roomPos, other.roomPos) * Room.MAP_EDGE_SIZE) + Point.distance(pos, other.pos);
        }
        
        public int pathDistance(RoomPosition other) {
            return Room.MAP_EDGE_SIZE * (System.Math.Abs(roomPos.x - other.roomPos.x) + System.Math.Abs(roomPos.y - other.roomPos.y)) +
                   System.Math.Abs(pos.x - other.pos.x) + System.Math.Abs(pos.y - other.pos.y);
        }

        public double distance(GameEntity entity) {
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
            return context.appState.worldMap[roomPos.x, roomPos.y];
        }

        public ITile getTile(ISContext context) {
            return getRoom(context).tiles[pos.x, pos.y];
        }


        public override bool Equals(object obj) {
            return obj is RoomPosition position && this == position;
        }

        public override int GetHashCode() {
            unchecked {
                var hash = 17;
                hash = hash * 31 + pos.x;
                hash = hash * 31 + pos.y;
                hash = hash * 31 + roomPos.x;
                hash = hash * 31 + roomPos.y;
                return hash;
            }
        }

        public static bool operator ==(RoomPosition a, RoomPosition b) {
            return a.pos.x == b.pos.x && a.pos.y == b.pos.y && a.roomPos.x == b.roomPos.x && a.roomPos.y == b.roomPos.y;
        }

        public static bool operator !=(RoomPosition a, RoomPosition b) {
            return a.pos.x != b.pos.x || a.pos.y != b.pos.y || a.roomPos.x != b.roomPos.x || a.roomPos.y != b.roomPos.y;
        }
    }
}