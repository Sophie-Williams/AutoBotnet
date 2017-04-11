using System;
using Speercs.Server.Game.MapGen;

namespace Speercs.DevTests {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Starting");

            var generator = new MapGenerator();
            var room = generator.GenerateRoom();
            room.Print();
        }
    }
}
