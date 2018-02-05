using Nancy;
using Speercs.Server.Configuration;

namespace Speercs.Server.Modules {
    /// <summary>
    /// Defines an API module for Speercs
    /// </summary>
    public abstract class SBaseModule : NancyModule {
        public ISContext serverContext { get; private set; }

        internal SBaseModule(string path, ISContext serverContext) : base($"/a{path}") {
            this.serverContext = serverContext;
        }
    }
}