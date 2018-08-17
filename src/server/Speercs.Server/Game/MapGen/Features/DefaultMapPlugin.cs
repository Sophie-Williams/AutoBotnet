using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Extensibility.Map.Features;
using Speercs.Server.Extensibility.Map.Tiles;
using Speercs.Server.Game.MapGen.Tiles;

namespace Speercs.Server.Game.MapGen.Features {
    public class DefaultMapPlugin : ISpeercsPlugin {
        public void beforeActivation(CookieJar container) {
            container.registerType<Tile>(typeof(TileBedrock));
            container.registerType<Tile>(typeof(TileCrashSite));
            container.registerType<Tile>(typeof(TileFloor));
            container.registerType<Tile>(typeof(TileRock));
            // Register MapGen features
            container.register<IMapGenFeature>(new OreFeature(
                () => TileOre.create(new TileOre.NRGOre(), amount: 40),
                0.003,
                OreFeature.Location.Wall,
                1, 4
            ));
            container.registerType<Tile>(typeof(TileOre));
        }
    }
}