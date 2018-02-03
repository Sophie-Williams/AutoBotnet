using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;
using System;
using System.Collections.Generic;

namespace Speercs.Server.Extensibility.MapGen {
    public interface IMapGenerator {
        Random Random { get; }

        Room GenerateRoom(int roomX, int roomY);

        ISet<Point> Walls { get; set; }

        ISet<Point> ExposedWalls { get; set; }

        ISet<Point> UnexposedWalls { get; set; }

        Point RandomWall();

        Point RandomExposedWall();

        Point RandomUnexposedWall();
    }
}