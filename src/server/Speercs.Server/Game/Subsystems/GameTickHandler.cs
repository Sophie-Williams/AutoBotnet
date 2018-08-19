using System.Linq;
using System.Threading.Tasks;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Game.Subsystems {
    public class GameTickHandler : DependencyObject {
        private UserManagerService _userManager;

        public GameTickHandler(ISContext context) : base(context) {
            _userManager = new UserManagerService(context);
        }

        public async Task onTickAsync() {
            var batchExecutor = new BatchProgramExecutor(serverContext);
            await batchExecutor.executePlayerProgramsAsync();

            // tick entities
            foreach (var user in _userManager.getUsers()) {
                var teamId = user.identifier;
                var teamEntities = serverContext.appState.entities.getByUser(teamId).ToList();
                foreach (var entity in teamEntities) {
                    var result = entity.tick();
                    
                    // remove dead entities
                    if (!result) {
                        serverContext.appState.entities.remove(entity);
                    }
                }
                // TODO: Game update logic (???, achievements, etc.)
            }
        }
    }
}