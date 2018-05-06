using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Entities {
    public enum Direction {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public abstract class GameEntity : DependencyObject {
        public string id { get; }

        public RoomPosition position { get; set; }

        public GameEntity(ISContext serverContext, RoomPosition pos) : base(serverContext) {
            position = pos;

            id = Guid.NewGuid().ToString("N");
            this.serverContext.appState.entities.insert(this);
        }

        public RoomPosition move(int x, int y) {
            if (x < 0 || y < 0 || x >= Room.MAP_EDGE_SIZE || y >= Room.MAP_EDGE_SIZE)
                throw new ArgumentException("Cannot move outside of the Room's boundaries");
            return move(new RoomPosition(position.roomPos, new Point(x, y)));
        }

        public RoomPosition move(RoomPosition pos) {
            return position = pos;
        }

        public virtual bool moveRelative(Direction direction) {
            var roomX = position.roomPos.x;
            var roomY = position.roomPos.y;
            var newX = position.pos.x;
            var newY = position.pos.y;

            // TODO: Movement between rooms

            switch (direction) {
                case Direction.North:
                    newY--;

                    break;

                case Direction.East:
                    newX++;

                    break;

                case Direction.South:
                    newY++;

                    break;

                case Direction.West:
                    newX--;

                    break;
                default:
                    // this can happen if an int is casted to Direction
                    throw new ArgumentException("direction must be one of the four cardinal directions", "direction");
            }

            var newPos = new RoomPosition(new Point(roomX, roomY), new Point(newX, newY));
            if (!newPos.getTile(serverContext).isWalkable())
                return false; // not Walkable; don't move

            position = newPos;
            return true;
        }

        private bool moveRoom(int roomX, int roomY) {
            // only allow moving to an adjacent room that exists
            if (System.Math.Abs(position.roomPos.x - roomX) == 1 || System.Math.Abs(position.roomPos.y - roomY) == 1) {
                if (serverContext.appState.worldMap[roomX, roomY] != null) {
                    position = new RoomPosition(new Point(roomX, roomY), position.pos);
                }
            }

            return false;
        }
    }
}