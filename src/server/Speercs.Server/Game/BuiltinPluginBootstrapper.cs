using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen.Features;

namespace Speercs.Server.Game
{
    public class BuiltinPluginBootstrapper : DependencyObject
    {
        public BuiltinPluginBootstrapper(ISContext context) : base(context)
        {
        }

        public void LoadAll()
        {
            // Load plugins
            ServerContext.PluginLoader.Load<DefaultFeaturesPlugin>();

            // Load containers
            foreach (var plugin in ServerContext.PluginLoader.Plugins)
            {
                plugin.BeforeActivation(ServerContext.ExtensibilityContainer);
            }
        }
    }
}
