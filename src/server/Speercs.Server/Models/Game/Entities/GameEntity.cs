using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
    public abstract class GameEntity : DependencyObject
    {
        public (int, int) RoomIdentifier { get; set; }

        public Point Location { get; set; }

        public GameEntity(ISContext serverContext) : base(serverContext)
        {
            ServerContext.AppState.Entities.Insert(this);
        }

        public Point Move(Point point)
        {
            return Move(point.X, point.Y);
        }

        public Point Move(int x, int y)
        {
            if (x >= Room.MapEdgeSize) x = Room.MapEdgeSize - 1;
            if (x < 0) x = 0;
            if (y >= Room.MapEdgeSize) y = Room.MapEdgeSize - 1;
            if (y < 0) y = 0;
            Location = new Point(x, y);
            return Location;
        }

        public Point MoveRelative(int direction)
        {
            int newX = Location.X;
            int newY = Location.Y;
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
                return Location; // not Walkable; don't move

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
            Location = new Point(newX, newY);
            return Location;
        }

        public Point MoveRelative(int x, int y)
        {
            Location = new Point(Location.X + x, Location.Y + y);
            return Location;
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
