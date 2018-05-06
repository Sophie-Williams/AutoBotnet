using System;
using Speercs.Server.Configuration;

namespace Speercs.Server.Tests {
    public abstract class DependencyFixture : IDisposable {
        public SContext serverContext { get; }

        public DependencyFixture(SConfiguration configuration) {
            serverContext = createTransientServerContext(configuration);
        }

        private SContext createTransientServerContext(SConfiguration configuration) {
            var scx = new SContext(configuration);
            configuration.databaseConfiguration.fileName = null; // use transisent database mode
            scx.connectDatabase();
            scx.appState = new SAppState();
            return scx;
        }

        public virtual void Dispose() {
            serverContext.Dispose();
        }
    }
}