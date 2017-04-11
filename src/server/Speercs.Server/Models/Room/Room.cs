
using System;

namespace Speercs.Server.Models.Room {

    public enum TileType {
        Floor, Wall
    }

    public class Room {

        public const int SIZE = 64;

        public Room() {
            tiles = new TileType[SIZE, SIZE];
        }

        public static char GetTileChar(TileType t) {
            switch (t) {
            case TileType.Floor:
                return '.';
            case TileType.Wall:
                return '#';
            default:
                return '?';
            }
        }
        
        public void Print() {
            for (var x = 0; x < SIZE; x++) {
                for (var y = 0; y < SIZE; y++) {
                    Console.Write(GetTileChar(tiles[x, y]));
                }
                Console.WriteLine();
            }
        }
        
        public TileType[,] tiles;

    }

}