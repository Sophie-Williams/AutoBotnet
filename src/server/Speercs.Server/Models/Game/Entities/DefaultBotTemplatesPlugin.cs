using CookieIoC;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Extensibility.Entities.Templates;

namespace Speercs.Server.Models.Entities {
    public class DefaultBotTemplatesPlugin : ISpeercsPlugin {
        public void beforeActivation(CookieJar container) {
            // register bot templates
            container.Register<IBotTemplate>(new ScoutBotTemplate());
        }
    }
}