using Osmium.PluginEngine.Types;

namespace Speercs.Server.Extensibility {
    public interface ISpeercsPlugin : IOsmiumPlugin {
        string name { get; }
    }
}