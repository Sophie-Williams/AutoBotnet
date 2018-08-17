using CookieIoC;
using Speercs.Server.Extensibility.Map.Features;
using Speercs.Server.Extensibility.Map.Tiles;
using Speercs.Server.Game.MapGen.Tiles;

namespace Speercs.Server.Extensibility.Map {
    public class DefaultMapPlugin : ISpeercsPlugin {
        public string name => nameof(DefaultMapPlugin);
        
        public void beforeActivation(CookieJar container) {
            // Register MapGen features
            var nrgOre = new NRGOre();
            container.register<IMapGenFeature>(new OreFeature(
                () => TileOre.create(nrgOre, amount: nrgOre.resource.chunk * 4),
                0.003,
                OreFeature.Location.Wall,
                1, 4
            ));
            container.registerType<Tile>(typeof(TileOre));
        }
    }
}