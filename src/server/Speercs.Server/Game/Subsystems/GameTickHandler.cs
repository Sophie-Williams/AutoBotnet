using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Game.Subsystems
{
    public class GameTickHandler : DependencyObject
    {
        public GameTickHandler(ISContext context) : base(context)
        {
        }

        public async Task OnTick()
        {
            await Task.Delay(0);
        }
    }
}