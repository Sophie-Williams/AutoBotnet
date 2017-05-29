using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
    
    public abstract class GameEntity : DependencyObject
    {
        public readonly string ID;

        public RoomPosition Position { get; set; }

        public GameEntity(ISContext serverContext, RoomPosition pos) : base(serverContext)
        {
            Position = pos;
            
            ID = System.Guid.NewGuid().ToString("N");
            ServerContext.AppState.Entities.Insert(this);
        }

        public RoomPosition Move(int x, int y)
        {
            if (x<0 || y<0 || x>=Room.MapEdgeSize || y>=Room.MapEdgeSize)
                throw new ArgumentException("Cannot move outside of the Room's boundaries");
            return Move(new RoomPosition(Position, x, y));
        }
        
        public RoomPosition Move(RoomPosition pos)
        {
            return Position = pos;
        }

        public bool MoveRelative(Direction direction)
        {
            var roomX = Position.RoomX;
            var roomY = Position.RoomY;
            int newX = Position.X;
            int newY = Position.Y;
            
            switch (direction)
            {
                case Direction.North:
                    newY--;
                    if (newY < 0)
                    {
                        if (ServerContext.AppState.WorldMap[roomX, roomY - 1] != null)
                        {
                            roomY--;
                            newY = Room.MapEdgeSize - 1;
                        }
                        else return false;
                    }
                    break;

                case Direction.East:
                    newX++;
                    if (newX >= Room.MapEdgeSize)
                    {
                        if (ServerContext.AppState.WorldMap[roomX + 1, roomY] != null)
                        {
                            roomX++;
                            newX = 0;
                        }
                        else return false;
                    }
                    break;

                case Direction.South:
                    newY++;
                    if (newY >= Room.MapEdgeSize)
                    {
                        if (ServerContext.AppState.WorldMap[roomX, roomY + 1] != null)
                        {
                            roomY++;
                            newY = 0;
                        }
                        else return false;
                    }
                    break;

                case Direction.West:
                    newX--;
                    if (newX < 0)
                    {
                        if (ServerContext.AppState.WorldMap[roomX - 1, roomY] != null)
                        {
                            roomX--;
                            newX = Room.MapEdgeSize - 1;
                        }
                        else return false;
                    }
                    break;
            }

            var newPos = new RoomPosition(roomX, roomY, newX, newY);
            if (!newPos.GetTile(ServerContext).IsWalkable())
                return false; // not Walkable; don't move
            
            Position = newPos;
            return true;
        }

        public RoomPosition MoveRelative(int x, int y)
        {
            return Move(Position.X + x, Position.Y + y);
        }

        public bool AttemptMoveRoom((int, int) newRoom)
        {
            // only allow moving to an adjacent room that exists
            (int nRoomX, int nRoomY) = newRoom;
            var roomX = Position.RoomX;
            var roomY = Position.RoomY;
            if (ServerContext.AppState.WorldMap[nRoomX, nRoomY] != null)
            {
                if (roomX + 1 == nRoomX || roomX - 1 == nRoomX ||
                    roomY + 1 == nRoomY || roomY - 1 == nRoomY)
                {
                    Position = new RoomPosition(nRoomX, nRoomY, Position.X, Position.Y);
                    return true;
                }
            }
            return false;
        }
    }
}
