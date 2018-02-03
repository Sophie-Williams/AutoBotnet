using Speercs.Server.Configuration;
using Speercs.Server.Game.Subsystems;
using System.Threading;
using Speercs.Server.Game.Scripting;

namespace Speercs.Server.Game {
    public class SGameBootstrapper : DependencyObject {
        public TickSystem tickSystem { get; }

        public CancellationToken tickSystemCancelToken { get; }

        public GameTickHandler tickHandler { get; }

        public SGameBootstrapper(ISContext context) : base(context) {
            // Create tick handler
            tickHandler = new GameTickHandler(context);
            // Create tick system
            tickSystem = new TickSystem(context.configuration.tickrate,
                context.configuration.useDynamicTickrate,
                tickHandler.onTickAsync);
            tickSystemCancelToken = new CancellationTokenSource().Token;
        }

        public void onStartup() {
            // start tick system
            var tickLoopTask = tickSystem.startAsync(tickSystemCancelToken);
        }
    }
}