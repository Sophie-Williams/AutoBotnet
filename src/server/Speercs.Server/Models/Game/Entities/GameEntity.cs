using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
    public abstract class GameEntity : DependencyObject
    {
        public (int, int) RoomIdentifier { get; set; }

        public Point Location { get; set; }

        public int Size { get; set; }

        public GameEntity(ISContext serverContext) : base(serverContext)
        {
            ServerContext.AppState.Entities.Insert(this);
        }

        public Point Move(Point point)
        {
            if (point.X >= Room.MapEdgeSize) point = new Point(Room.MapEdgeSize - 1, point.Y);
            if (point.X < 0) point = new Point(0, point.Y);
            if (point.Y >= Room.MapEdgeSize) point = new Point(point.X, Room.MapEdgeSize - 1);
            if (point.Y < 0) point = new Point(point.X, 0);
            Location = point;
            return Location;
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

            // Change this once tiles have a `Walkable` flag
            if (ServerContext.AppState.WorldMap[roomX, roomY].Tiles[newX, newY] != TileType.Floor) return Location;

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
            int newX = Location.X + x;
            int newY = Location.Y + y;
            Point newLoc = new Point(Location.X + x, Location.Y + y);
            Location = new Point(Location.X + x, Location.Y + y);
            return Location;
        }

        public bool AttemptMoveRoom((int, int) roomIdentifier)
        {
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
