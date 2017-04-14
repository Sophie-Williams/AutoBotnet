using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen
{
    public interface IMapGenFeature
    {
        void Generate(Room room, IMapGenerator generator);
    }
}