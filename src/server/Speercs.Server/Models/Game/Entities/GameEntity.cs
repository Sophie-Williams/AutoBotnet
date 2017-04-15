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

        }

        public Point Move(Point point)
        {
            if (point.X >= Room.MapEdgeSize) point = new Point(Room.MapEdgeSize-1, point.Y);
            if (point.X < 0) point = new Point(0, point.Y);
            if (point.Y >= Room.MapEdgeSize) point = new Point(point.X, Room.MapEdgeSize-1);
            if (point.Y < 0) point = new Point(point.X, 0);
            Location = point;
            return Location;
        }

        public Point Move(int x, int y)
        {
            if (x >= Room.MapEdgeSize) x = Room.MapEdgeSize-1;
            if (x < 0) x = 0;
            if (y >= Room.MapEdgeSize) y = Room.MapEdgeSize-1;
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
                    newY++;
                    break;
                case 1:
                    newX++;
                    break;
                case 2:
                    newY--;
                    break;
                case 3:
                    newX--;
                    break;
            }
            (int roomX, int roomY) = RoomIdentifier;
            if (ServerContext.AppState.WorldMap[roomX, roomY].Tiles[newX, newY] != TileType.Floor) return Location;
            if (newX > Room.MapEdgeSize && ServerContext.AppState.WorldMap[roomX - 1, roomY] != null)
            {
                    RoomIdentifier = (roomX - 1, roomY);
                    newX = 0;
            }
            if (newX < 0 && ServerContext.AppState.WorldMap[roomX + 1, roomY] != null)
            {
                    RoomIdentifier = (roomX + 1, roomY);
                    newX = Room.MapEdgeSize-1;
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
    }
}
