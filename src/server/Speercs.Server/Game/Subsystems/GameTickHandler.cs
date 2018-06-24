using System.Linq;
using System.Threading.Tasks;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting;

namespace Speercs.Server.Game.Subsystems {
    public class GameTickHandler : DependencyObject {
        public GameTickHandler(ISContext context) : base(context) { }

        public async Task onTickAsync() {
            var batchExecutor = new BatchProgramExecutor(serverContext);
            await batchExecutor.executePlayerProgramsAsync();

            // tick entities
            var teamIds = serverContext.appState.userMetrics.Select(x => x.Key);
            foreach (var teamId in teamIds) {
                var teamEntityList = serverContext.appState.entities.getByUser(teamId);
                teamEntityList.Select(x => x.tick());
            }
        }
    }
}