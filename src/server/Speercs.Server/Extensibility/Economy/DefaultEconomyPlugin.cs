using CookieIoC;
using Speercs.Server.Extensibility.Economy.Resources;

namespace Speercs.Server.Extensibility.Economy {
    public class DefaultEconomyPlugin : ISpeercsPlugin {
        public string name => nameof(DefaultEconomyPlugin);

        public void beforeActivation(CookieJar container) {
            // register economy resources
            container.register<EconomyResource>(new NRGResource());
        }
    }
}