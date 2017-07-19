using Speercs.Server.Configuration;
using Speercs.Server.Game.Subsystems;
using System.Threading;
using Speercs.Server.Game.Scripting;

namespace Speercs.Server.Game
{
    public class SGameBootstrapper : DependencyObject
    {
        public TickSystem TickSystem { get; }

        public CancellationToken TickSystemCancelToken { get; }
        
        public GameTickHandler TickHandler { get; }

        public SGameBootstrapper(ISContext context) : base(context)
        {
            // Create tick handler
            TickHandler = new GameTickHandler(context);
            // Create tick system
            TickSystem = new TickSystem(context.Configuration.TickRate,
                context.Configuration.UseDynamicTickRate,
                TickHandler.OnTickAsync);
            TickSystemCancelToken = new CancellationTokenSource().Token;
        }

        public void OnStartup()
        {
            // start tick system
            var tickLoopTask = TickSystem.StartAsync(TickSystemCancelToken);
        }
    }
}