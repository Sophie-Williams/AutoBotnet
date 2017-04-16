using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.MapGen;
using Speercs.Server.Game.MapGen.Features.Resources;

namespace Speercs.Server.Game.MapGen.Features
{
    public class DefaultFeaturesPlugin : ISpeercsPlugin
    {
        public void BeforeActivation(CookieJar container)
        {
            // Register MapGen features
            container.Register<IMapGenFeature>(new NRGResourceFeature());
            // container.Register<IMapGenFeature>(new TestOresFeature());
            container.Register<IMapGenFeature>(new OreFeature(
                () => new Tiles.TileOre(),
                0.038,
                OreFeature.Location.ExposedWall
            ));
            container.Register<IMapGenFeature>(new OreFeature(
                () => new Tiles.TileRareOre(),
                0.021,
                OreFeature.Location.UnexposedWall
            ));
        }
    }
}
