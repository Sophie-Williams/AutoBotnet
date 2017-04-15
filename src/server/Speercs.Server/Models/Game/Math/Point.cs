namespace Speercs.Server.Models.Math
{
    public struct Point
    {
        public int X { get; }

        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool EqualTo(Point p) => (X == p.X) && (Y == p.Y);
    }
}
