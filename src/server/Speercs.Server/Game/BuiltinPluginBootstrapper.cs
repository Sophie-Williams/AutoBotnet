using System.Reflection;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game {
    public class BuiltinPluginBootstrapper : DependencyObject {
        public BuiltinPluginBootstrapper(ISContext context) : base(context) { }

        public void loadAll() {
            // Automatically load plugins from this assembly
            serverContext.pluginLoader.loadFrom(Assembly.GetExecutingAssembly());

            // Load plugin assemblies
            foreach (var pluginAssemblyPath in serverContext.configuration.pluginAssemblies) {
                var asm = serverContext.pluginLoader.loadAssembly(pluginAssemblyPath);
                var assemblyPlugins = serverContext.pluginLoader.loadFrom(asm);
            }

            // Load containers
            foreach (var plugin in serverContext.pluginLoader.plugins) {
                plugin.beforeActivation(serverContext.extensibilityContainer);
            }
            
            // update item cache
            serverContext.registry.recache();
        }
    }
}