using System;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Extensibility.MapGen
{
    public interface IMapGenerator
    {
        Random Random { get; }

        Room GenerateRoom();

        // TODO
    }
}