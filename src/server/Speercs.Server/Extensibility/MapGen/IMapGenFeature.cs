using Speercs.Server.Models.Map;

namespace Speercs.Server.Extensibility.MapGen {
    public interface IMapGenFeature {
        void generate(Room room, IMapGenerator generator);
    }
}