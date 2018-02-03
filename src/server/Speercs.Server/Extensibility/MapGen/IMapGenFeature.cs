using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Extensibility.MapGen {
    public interface IMapGenFeature {
        void Generate(Room room, IMapGenerator generator);
    }
}