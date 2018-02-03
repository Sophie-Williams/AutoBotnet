using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen.Features;

namespace Speercs.Server.Game {
    public class BuiltinPluginBootstrapper : DependencyObject {
        public BuiltinPluginBootstrapper(ISContext context) : base(context) { }

        public void loadAll() {
            // Load plugins
            serverContext.pluginLoader.load<DefaultFeaturesPlugin>();

            // Load containers
            foreach (var plugin in serverContext.pluginLoader.plugins) {
                plugin.beforeActivation(serverContext.extensibilityContainer);
            }
        }
    }
}