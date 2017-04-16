using Speercs.Server.Configuration;
using Speercs.Server.Game.Subsystems;
using System.Threading;

namespace Speercs.Server.Game
{
    public class SGameBootstrapper : DependencyObject
    {
        public TickSystem TickSystem { get; }

        public CancellationToken TickSystemCancelToken { get; }
        
        public GameTickHandler TickHandler { get; }

        public SGameBootstrapper(ISContext context) : base(context)
        {
            // create tick handler
            TickHandler = new GameTickHandler(context);
            // create tick system
            TickSystem = new TickSystem(context.Configuration.TickRate,
                context.Configuration.UseDynamicTickRate,
                TickHandler.OnTick);
            TickSystemCancelToken = new CancellationTokenSource().Token;
        }

        public void OnStartup()
        {
            // start tick system
            var tickLoopTask = TickSystem.Start(TickSystemCancelToken);
        }
    }
}