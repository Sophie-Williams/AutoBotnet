using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities {
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

            id = System.Guid.NewGuid().ToString("N");
            base.serverContext.appState.entities.insert(this);
        }

        public RoomPosition move(int x, int y) {
            if (x < 0 || y < 0 || x >= Room.MAP_EDGE_SIZE || y >= Room.MAP_EDGE_SIZE)
                throw new ArgumentException("Cannot move outside of the Room's boundaries");
            return move(new RoomPosition(position, x, y));
        }

        public RoomPosition move(RoomPosition pos) {
            return position = pos;
        }

        public virtual bool moveRelative(Direction direction) {
            var roomX = position.roomX;
            var roomY = position.roomY;
            int newX = position.x;
            int newY = position.y;

            switch (direction) {
                case Direction.North:
                    newY--;
                    if (newY < 0) {
                        if (serverContext.appState.worldMap[roomX, roomY - 1] != null) {
                            roomY--;
                            newY = Room.MAP_EDGE_SIZE - 1;
                        } else return false;
                    }

                    break;

                case Direction.East:
                    newX++;
                    if (newX >= Room.MAP_EDGE_SIZE) {
                        if (serverContext.appState.worldMap[roomX + 1, roomY] != null) {
                            roomX++;
                            newX = 0;
                        } else return false;
                    }

                    break;

                case Direction.South:
                    newY++;
                    if (newY >= Room.MAP_EDGE_SIZE) {
                        if (serverContext.appState.worldMap[roomX, roomY + 1] != null) {
                            roomY++;
                            newY = 0;
                        } else return false;
                    }

                    break;

                case Direction.West:
                    newX--;
                    if (newX < 0) {
                        if (serverContext.appState.worldMap[roomX - 1, roomY] != null) {
                            roomX--;
                            newX = Room.MAP_EDGE_SIZE - 1;
                        } else return false;
                    }

                    break;
                default:
                    // this can happen if an int is casted to Direction
                    throw new ArgumentException("direction must be one of the four cardinal directions", "direction");
            }

            var newPos = new RoomPosition(roomX, roomY, newX, newY);
            if (!newPos.getTile(serverContext).isWalkable())
                return false; // not Walkable; don't move

            position = newPos;
            return true;
        }

        public RoomPosition moveRelative(int x, int y) {
            return move(position.x + x, position.y + y);
        }

        public bool attemptMoveRoom((int, int) newRoom) {
            // only allow moving to an adjacent room that exists
            (int nRoomX, int nRoomY) = newRoom;
            var roomX = position.roomX;
            var roomY = position.roomY;
            if (serverContext.appState.worldMap[nRoomX, nRoomY] != null) {
                if (roomX + 1 == nRoomX || roomX - 1 == nRoomX ||
                    roomY + 1 == nRoomY || roomY - 1 == nRoomY) {
                    position = new RoomPosition(nRoomX, nRoomY, position.x, position.y);
                    return true;
                }
            }

            return false;
        }
    }
}