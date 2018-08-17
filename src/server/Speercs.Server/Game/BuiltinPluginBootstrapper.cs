using System.Reflection;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Application;

namespace Speercs.Server.Game {
    public class BuiltinPluginBootstrapper : DependencyObject {
        public BuiltinPluginBootstrapper(ISContext context) : base(context) { }

        public void loadAll() {
            serverContext.log.writeLine("Loading plugins", SpeercsLogger.LogLevel.Information);
            // Automatically load plugins from this assembly
            serverContext.pluginLoader.loadFrom(Assembly.GetExecutingAssembly());

            // Load plugin assemblies
            foreach (var pluginAssemblyPath in serverContext.configuration.pluginAssemblies) {
                var asm = serverContext.pluginLoader.loadAssembly(pluginAssemblyPath);
                var assemblyPlugins = serverContext.pluginLoader.loadFrom(asm);
            }

            // Run the plugin to load types into our container
            foreach (var plugin in serverContext.pluginLoader.plugins) {
                serverContext.log.writeLine($"plugin {plugin.name}", SpeercsLogger.LogLevel.Trace);
                plugin.beforeActivation(serverContext.extensibilityContainer);
            }
            
            serverContext.log.writeLine("Rebuilding item cache", SpeercsLogger.LogLevel.Information);
            // update item cache
            serverContext.registry.recache();
        }
    }
}