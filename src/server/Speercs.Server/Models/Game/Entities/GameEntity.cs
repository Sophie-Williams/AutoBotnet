using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
    public abstract class GameEntity : DependencyObject
    {
        public (int, int) RoomIdentifier { get; set; }
        
        public readonly string ID;

        public RoomPosition Position { get; set; }

        public GameEntity(ISContext serverContext) : base(serverContext)
        {
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
            Position = new RoomPosition(Position.RoomX, Position.RoomY, x, y);
            return Position;
        }

        public RoomPosition MoveRelative(int direction)
        {
            int newX = Position.X;
            int newY = Position.Y;
            switch (direction)
            {
                case 0:
                    newY--;
                    break;

                case 1:
                    newX++;
                    break;

                case 2:
                    newY++;
                    break;

                case 3:
                    newX--;
                    break;
            }
            (int roomX, int roomY) = RoomIdentifier;

            if (!ServerContext.AppState.WorldMap[roomX, roomY].Tiles[newX, newY].IsWalkable())
                return Position; // not Walkable; don't move

            if (newX >= Room.MapEdgeSize)
            {
                if (ServerContext.AppState.WorldMap[roomX + 1, roomY] != null)
                {
                    RoomIdentifier = (roomX + 1, roomY);
                    newX = 0;
                }
                else
                {
                    newX--;
                }
            }
            if (newX < 0)
            {
                if (ServerContext.AppState.WorldMap[roomX - 1, roomY] != null)
                {
                    RoomIdentifier = (roomX - 1, roomY);
                    newX = Room.MapEdgeSize - 1;
                }
                else
                {
                    newX++;
                }
            }
            if (newY >= Room.MapEdgeSize)
            {
                if (ServerContext.AppState.WorldMap[roomX, roomY + 1] != null)
                {
                    RoomIdentifier = (roomX, roomY + 1);
                    newY = 0;
                }
                else
                {
                    newY--;
                }
            }
            if (newY < 0)
            {
                if (ServerContext.AppState.WorldMap[roomX, roomY - 1] != null)
                {
                    RoomIdentifier = (roomX, roomY - 1);
                    newY = Room.MapEdgeSize - 1;
                }
                else
                {
                    newY++;
                }
            }
            Position = new RoomPosition(Position.RoomX, Position.RoomY, newX, newY);
            return Position;
        }

        public RoomPosition MoveRelative(int x, int y)
        {
            Position = new RoomPosition(Position.RoomX, Position.RoomY, Position.X + x, Position.Y + y);
            return Position;
        }

        public bool AttemptMoveRoom((int, int) roomIdentifier)
        {
            // only allow moving to an adjacent room that exists
            (int nRoomX, int nRoomY) = roomIdentifier;
            (int roomX, int roomY) = RoomIdentifier;
            if (ServerContext.AppState.WorldMap[nRoomX, nRoomY] != null)
            {
                if (roomX + 1 == nRoomX || roomX - 1 == nRoomX ||
                    roomY + 1 == nRoomY || roomY - 1 == nRoomY)
                {
                    RoomIdentifier = roomIdentifier;
                    return true;
                }
            }
            return false;
        }
    }
}
