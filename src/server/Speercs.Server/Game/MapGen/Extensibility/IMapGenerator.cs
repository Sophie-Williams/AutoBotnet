using System;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen
{
    public interface IMapGenerator
    {
        Random Random { get; }

        Room GenerateRoom();

        // TODO
    }
}