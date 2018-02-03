using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Extensibility.MapGen {
    public interface IMapGenFeature {
        void generate(Room room, IMapGenerator generator);
    }
}