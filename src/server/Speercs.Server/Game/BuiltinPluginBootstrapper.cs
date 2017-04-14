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
            ServerContext.PluginLoader.Load<DefaultFeaturesPlugin>();
        }
    }
}