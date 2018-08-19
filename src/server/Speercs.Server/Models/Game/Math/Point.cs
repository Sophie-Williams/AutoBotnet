namespace Speercs.Server.Models.Math {
    public struct Point {
        public int x { get; }

        public int y { get; }

        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return equalTo((Point) obj);
        }

        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                return hash;
            }
        }

        public bool equalTo(Point p) => (x == p.x) && (y == p.y);

        public override string ToString() => $"({x}, {y})";

        public static double dist(Point p1, Point p2) {
            return System.Math.Sqrt(System.Math.Pow(p1.x - p2.x, 2) + System.Math.Pow(p1.y - p2.y, 2));
        }
        
        /// <summary>
        /// Manhattan dist
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static int mhDist(Point p1, Point p2) {
            return System.Math.Abs(p1.x - p2.x) + System.Math.Abs(p1.y - p2.y);
        }

        /// <summary>
        /// Chebyshev dist
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static int chDist(Point p1, Point p2) {
            return System.Math.Max(System.Math.Abs(p1.x - p2.x), System.Math.Abs(p1.y - p2.y));
        }

    }
}