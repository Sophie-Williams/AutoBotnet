using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Game.MapGen.Tiles;

namespace Speercs.Server.Game.MapGen {
    public class CoreMapPlugin : ISpeercsPlugin {
        public string name => nameof(CoreMapPlugin);
        
        public void beforeActivation(CookieJar container) {
            container.registerType<Tile>(typeof(TileBedrock));
            container.registerType<Tile>(typeof(TileCrashSite));
            container.registerType<Tile>(typeof(TileFloor));
            container.registerType<Tile>(typeof(TileRock));
        }
    }
}