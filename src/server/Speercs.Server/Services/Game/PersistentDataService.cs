using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Models.Program;

namespace Speercs.Server.Services.Game {
    public class PersistentDataService : DependencyObject {

        public PersistentDataService(ISContext serverContext) : base(serverContext) { }

        public UserPersistentData get(string ownerId) => findPersistentData(ownerId);

        private UserPersistentData findPersistentData(string ownerId) {
            return serverContext.appState.persistentData[ownerId];
        }

        public void createPersistentData(string ownerId) {
            var persistentData = new UserPersistentData(ownerId) {
                program = new UserProgram("\nfunction loop () {\n  // your code\n}\n"),
                team = new UserEmpire {
                    identifier = ownerId
                }
            };
            serverContext.appState.persistentData[ownerId] = persistentData;
        }

        public void removePersistentData(string ownerId) {
            serverContext.appState.persistentData.Remove(ownerId);
        }

        public void deployProgram(string ownerId, UserProgram program) {
            // update the user's code in the database
            var data = findPersistentData(ownerId);
            data.program = program;

            // reload the engine to apply changes
            serverContext.executors.reloadExecutor(ownerId);
        }

        public Queue<JToken> retrieveNotificationQueue(string userIdentifier) =>
            get(userIdentifier).queuedNotifications;
    }
}