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
            container.Register<IMapGenFeature>(new TestOresFeature());
        }
    }
}
