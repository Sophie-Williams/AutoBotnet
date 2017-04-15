using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
    public abstract class GameEntity : DependencyObject
    {
        public string RoomIdentifier { get; set; }

        public Point Location { get; set; }

        public int Size { get; set; }

        public GameEntity(ISContext serverContext) : base(serverContext)
        {

        }

        public Point Move(Point point)
        {
            if (point.X > Room.MapEdgeSize) point = new Point(Room.MapEdgeSize, point.Y);
            if (point.X < 0) point = new Point(0, point.Y);
            if (point.Y > Room.MapEdgeSize) point = new Point(point.X, Room.MapEdgeSize);
            if (point.Y < 0) point = new Point(point.X, 0);
            this.Location = point;
            return this.Location;
        }

        public Point Move(int x, int y)
        {
            if (x > Room.MapEdgeSize) x = Room.MapEdgeSize;
            if (x < 0) x = 0;
            if (y > Room.MapEdgeSize) y = Room.MapEdgeSize;
            if (y < 0) y = 0;
            this.Location = new Point(x, y);
            return this.Location;
        }

        public Point MoveRelative(int direction)
        {
            // TODO: Check for walls and stuff
            return this.Location;
        }

        public Point MoveRelative(int x, int y)
        {
            Point newLoc = new Point(this.Location.X + x, this.Location.Y + y);
            this.Location = new Point(this.Location.X + x, this.Location.Y + y);
            return this.Location;
        }
    }
}
