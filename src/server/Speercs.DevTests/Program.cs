using Speercs.Server.Game.MapGen;
using System;

namespace Speercs.DevTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting mapgen test");

            var generator = new MapGenerator();
            var room = generator.GenerateRoom(0.45);
            room.Print();
        }
    }
}