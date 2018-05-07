using System.Threading;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Subsystems;
using Speercs.Server.Services.Application;
using Speercs.Server.Services.Game;

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
            serverContext.log.writeLine($"Running game bootstrapper startup", SpeercsLogger.LogLevel.Information);
            var dataService = new PersistentDataService(serverContext);
            // Wake entities
            serverContext.appState.entities.serverContext = serverContext;
            var entityCount = 0;
            foreach (var entity in serverContext.appState.entities.enumerate()) {
                serverContext.appState.entities.wake(entity);
                var userData = dataService.get(entity.teamId);
                userData.team.addEntity(entity);
                entityCount++;
            }

            serverContext.log.writeLine($"woke {entityCount} entities", SpeercsLogger.LogLevel.Information);

            // start tick system
            var tickLoopTask = tickSystem.startAsync(tickSystemCancelToken);
        }

        public void onShutdown() {
            // shut down anything that needs to be
        }
    }
}