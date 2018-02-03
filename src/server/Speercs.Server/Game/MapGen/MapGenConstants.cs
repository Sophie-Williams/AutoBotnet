using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen {
    public static class MapGenConstants {
        /// Wall Density ///
        public const double MIN_ROOM_DENSITY = 0.40;

        public const double MAX_ROOM_DENSITY = 0.50;

        public const double DENSITY_FALLOFF_EXPONENT = 20;

        /// Cellular Automaton / Smoothing ///
        public const int CELLULAR_AUTOMATON_ITERATIONS = 12;

        /// Room Exits ///
        public const int MIN_EXIT_SIZE = Room.MAP_EDGE_SIZE / 16;

        public const int MAX_EXIT_SIZE = Room.MAP_EDGE_SIZE / 2;

        public const int EXIT_CARVE_DEPTH = Room.MAP_EDGE_SIZE / 20;

        /// Tile Type Stuff ///
        public const int BEDROCK_DEPTH = 2;
    }
}