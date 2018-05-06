using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Entities {
    public enum Direction {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public abstract class GameEntity : ProtectedDependencyObject {
        public string id { get; }

        private RoomPosition _position;

        public RoomPosition position {
            get => _position;
            set { propagatePosition(value); }
        }

        public GameEntity(ISContext serverContext, RoomPosition pos) : base(serverContext) {
            position = pos;

            id = Guid.NewGuid().ToString("N");
            this.serverContext.appState.entities.insert(this);
        }

        private void propagatePosition(RoomPosition pos) {
            // TODO: Propagate position to spatial hash, etc.
            serverContext.appState.entities.spatialHash[pos.roomPos].Remove(this);
            serverContext.appState.entities.insertSpatialHash(this);
            _position = pos;
        }

        public RoomPosition move(RoomPosition pos) {
            return position = pos;
        }

        public virtual bool moveRelative(Direction direction) {
            var roomX = position.roomPos.x;
            var roomY = position.roomPos.y;
            var newX = position.pos.x;
            var newY = position.pos.y;

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

            if (newX < 0 && moveRoom(roomX - 1, roomY)) {
                roomX--;
            } else {
                return false;
            }

            if (newX > Room.MAP_EDGE_SIZE && moveRoom(roomX + 1, roomY)) {
                roomX++;
            } else {
                return false;
            }

            if (newY < 0 && moveRoom(roomX, roomY - 1)) {
                roomY--;
            } else {
                return false;
            }

            if (newY > Room.MAP_EDGE_SIZE && moveRoom(roomX, roomY + 1)) {
                roomY++;
            } else {
                return false;
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