using Speercs.Server.Models.Map;

namespace Speercs.Server.Extensibility.Map {
    public interface IMapGenFeature {
        void generate(Room room, IMapGenerator generator);
    }
}