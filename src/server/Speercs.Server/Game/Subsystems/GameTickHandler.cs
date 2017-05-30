using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Game.Subsystems
{
    public class GameTickHandler : DependencyObject
    {
        public GameTickHandler(ISContext context) : base(context)
        {
        }

        public async Task OnTickAsync()
        {
            ServerContext.AppState.TickCount++;
            
            foreach (var entry in ServerContext.AppState.PlayerData)
            {
                var engine = ServerContext.Executors.RetrieveExecutor(entry.Value.User.Identifier);
                await Task.Run(() => {
                    engine.Invoke("loop");
                });
            }
        }
    }
}