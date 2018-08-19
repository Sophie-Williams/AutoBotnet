using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Models.Program;

namespace Speercs.Server.Services.Game {
    public class PersistentDataService : DependencyObject {
        private LiteCollection<UserPersistentData> _persistentDataCollection;

        public PersistentDataService(ISContext serverContext) : base(serverContext) {
            _persistentDataCollection =
                serverContext.database.GetCollection<UserPersistentData>(DatabaseKeys.COLLECTION_USERPERSISTENTDATA);
        }

        public UserPersistentData get(string ownerId) => findPersistentData(ownerId);

        private UserPersistentData findPersistentData(string ownerId) {
            return _persistentDataCollection.FindOne(x => x.ownerId == ownerId);
        }

        public void createPersistentData(string ownerId) {
            var persistentData = new UserPersistentData(ownerId) {
                program = new UserProgram("\nfunction loop () {\n  // your code\n}\n"),
                team = new UserEmpire {
                    identifier = ownerId
                }
            };
            _persistentDataCollection.Insert(persistentData);
            _persistentDataCollection.EnsureIndex(x => x.ownerId);
        }

        public void removePersistentData(string ownerId) {
            _persistentDataCollection.Delete(x => x.ownerId == ownerId);
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