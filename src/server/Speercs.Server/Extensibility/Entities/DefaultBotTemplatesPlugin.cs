using CookieIoC;
using Speercs.Server.Extensibility.Entities.Templates;
using Speercs.Server.Extensibility.Entities.Templates.Cores;

namespace Speercs.Server.Extensibility.Entities {
    public class DefaultBotTemplatesPlugin : ISpeercsPlugin {
        public string name => nameof(DefaultBotTemplatesPlugin);
        
        public void beforeActivation(CookieJar container) {
            // register bot templates
            container.register<IBotTemplate>(new ScoutBotTemplate());
            // register core templates
            container.register<IBotCoreTemplate>(new CoreStorage1Template());
            container.register<IBotCoreTemplate>(new CoreDrill1Template());
        }
    }
}