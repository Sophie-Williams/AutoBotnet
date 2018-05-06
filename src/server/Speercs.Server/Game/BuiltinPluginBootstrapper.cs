using System.Runtime.Loader;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen.Features;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game {
    public class BuiltinPluginBootstrapper : DependencyObject {
        public BuiltinPluginBootstrapper(ISContext context) : base(context) { }

        public void loadAll() {
            // Register default plugins
            serverContext.pluginLoader.load<DefaultFeaturesPlugin>();
            serverContext.pluginLoader.load<DefaultBotTemplatesPlugin>();

            // Load plugin assemblies
            foreach (var pluginAssemblyPath in serverContext.configuration.pluginAssemblies) {
                var asm = serverContext.pluginLoader.loadAssembly(pluginAssemblyPath);
                var assemblyPlugins = serverContext.pluginLoader.loadFrom(asm);
            }

            // Load containers
            foreach (var plugin in serverContext.pluginLoader.plugins) {
                plugin.beforeActivation(serverContext.extensibilityContainer);
            }
        }
    }
}