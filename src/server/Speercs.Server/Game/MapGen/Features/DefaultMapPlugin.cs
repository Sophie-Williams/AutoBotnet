using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Extensibility.Map.Features;
using Speercs.Server.Extensibility.Map.Tiles;
using Speercs.Server.Game.MapGen.Tiles;

namespace Speercs.Server.Game.MapGen.Features {
    public class DefaultMapPlugin : ISpeercsPlugin {
        public void beforeActivation(CookieJar container) {
            container.registerType<ITile>(typeof(TileBedrock));
            container.registerType<ITile>(typeof(TileCrashSite));
            container.registerType<ITile>(typeof(TileFloor));
            container.registerType<ITile>(typeof(TileWall));
            // Register MapGen features
            container.register<IMapGenFeature>(new OreFeature(
                () => new TileOre {
                    resource = "nrg"
                },
                0.003,
                OreFeature.Location.Wall,
                1, 4
            ));
            container.registerType<ITile>(typeof(TileOre));
        }
    }
}