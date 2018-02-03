using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.MapGen;

namespace Speercs.Server.Game.MapGen.Features {
    public class DefaultFeaturesPlugin : ISpeercsPlugin {
        public void beforeActivation(CookieJar container) {
            // Register MapGen features
            container.Register<IMapGenFeature>(new OreFeature(
                () => new Tiles.TileNrgOre(),
                0.003,
                OreFeature.Location.Wall,
                1, 4
            ));
        }
    }
}