using System.Threading.Tasks;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting;

namespace Speercs.Server.Game.Subsystems
{
    public class GameTickHandler : DependencyObject {
        public GameTickHandler(ISContext context) : base(context) { }

        public async Task onTickAsync() {
            var batchExecutor = new BatchProgramExecutor(serverContext);
            await batchExecutor.executePlayerPrograms();
        }
    }
}