using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Extensibility.Map.Features;
using Speercs.Server.Extensibility.Map.Tiles;
using Speercs.Server.Models.Game.Materials;

namespace Speercs.Server.Game.MapGen.Features {
    public class DefaultFeaturesPlugin : ISpeercsPlugin {
        public void beforeActivation(CookieJar container) {
            // Register MapGen features
            container.Register<IMapGenFeature>(new OreFeature(
                () => new TileOre {
                    resource = ResourceId.NRG
                },
                0.003,
                OreFeature.Location.Wall,
                1, 4
            ));
        }
    }
}