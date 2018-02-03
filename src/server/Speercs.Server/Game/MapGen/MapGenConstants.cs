using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen {
    public static class MapGenConstants {
        /// Wall Density ///
        public const double MinRoomDensity = 0.40;

        public const double MaxRoomDensity = 0.50;

        public const double DensityFalloffExponent = 20;

        /// Cellular Automaton / Smoothing ///
        public const int CellularAutomatonIterations = 12;

        /// Room Exits ///
        public const int MinExitSize = Room.MapEdgeSize / 16;

        public const int MaxExitSize = Room.MapEdgeSize / 2;

        public const int ExitCarveDepth = Room.MapEdgeSize / 20;

        /// Tile Type Stuff ///
        public const int BedrockDepth = 2;
    }
}