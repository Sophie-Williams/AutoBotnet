namespace Speercs.Server.Models.Math {
    public struct Point {
        public int X { get; }

        public int Y { get; }

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return EqualTo((Point) obj);
        }

        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public bool EqualTo(Point p) => (X == p.X) && (Y == p.Y);
    }
}