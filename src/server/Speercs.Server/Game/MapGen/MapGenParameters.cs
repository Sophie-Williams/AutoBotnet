using Speercs.Server.Models.Map;

namespace Speercs.Server.Game.MapGen {
    public class MapGenParameters {
        /// Wall Density ///
        public double minRoomDensity = 0.40;

        public double maxRoomDensity = 0.50;

        public double densityFalloffExponent = 20;

        /// Cellular Automaton / Smoothing ///
        public int cellularAutomatonIterations = 12;

        public int cellularAutomatonMinNeighbors = 4;

        public int cellularAutomatonMaxNeighbors = 5;

        /// Room Exits ///
        public int minExitSize = Room.MAP_EDGE_SIZE / 16;

        public int maxExitSize = Room.MAP_EDGE_SIZE / 2;

        public int exitCarveDepth = Room.MAP_EDGE_SIZE / 20;

        /// Tile Type Stuff ///
        public int bedrockDepth = 2;
    }
}