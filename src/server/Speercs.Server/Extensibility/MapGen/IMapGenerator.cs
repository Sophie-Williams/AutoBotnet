using System;
using System.Collections.Generic;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Extensibility.MapGen {
    public interface IMapGenerator {
        Random random { get; }

        Room generateRoom(int roomX, int roomY);

        ISet<Point> walls { get; set; }

        ISet<Point> exposedWalls { get; set; }

        ISet<Point> unexposedWalls { get; set; }

        Point randomWall();

        Point randomExposedWall();

        Point randomUnexposedWall();
    }
}