using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
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

        public RoomPosition Move(Point point)
        {
            return Move(point.X, point.Y);
        }

        public RoomPosition Move(int x, int y)
        {
            if (x >= Room.MapEdgeSize) x = Room.MapEdgeSize - 1;
            if (x < 0) x = 0;
            if (y >= Room.MapEdgeSize) y = Room.MapEdgeSize - 1;
            if (y < 0) y = 0;
            Position = new RoomPosition(Position, x, y);
            return Position;
        }

        public bool MoveRelative(int direction)
        {
            var roomX = Position.RoomX;
            var roomY = Position.RoomY;
            int newX = Position.X;
            int newY = Position.Y;
            
            switch (direction)
            {
                case 0:
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

                case 1:
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

                case 2:
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

                case 3:
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

            if (!ServerContext.AppState.WorldMap[roomX, roomY].Tiles[newX, newY].IsWalkable())
                return false; // not Walkable; don't move
            
            Position = new RoomPosition(roomX, roomY, newX, newY);
            return true;
        }

        public RoomPosition MoveRelative(int x, int y)
        {
            Position = new RoomPosition(Position, Position.X + x, Position.Y + y);
            return Position;
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
