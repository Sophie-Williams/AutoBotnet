using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Program;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Speercs.Server.Services.Game {
    public class PlayerPersistentDataService : DependencyObject {
        public const string PLAYER_DATA_KEY = "player_persistent_data";
        private readonly LiteCollection<UserPersistentData> _persistentPlayerDataCollection;

        public PlayerPersistentDataService(ISContext serverContext) : base(serverContext) {
            _persistentPlayerDataCollection = serverContext.database.GetCollection<UserPersistentData>(PLAYER_DATA_KEY);
        }

        public UserPersistentData this[string ownerId] => findPersistentData(ownerId);

        private UserPersistentData findPersistentData(string ownerId) {
            return _persistentPlayerDataCollection.FindOne(x => x.ownerId == ownerId);
        }

        public async Task createPersistentDataAsync(string ownerId) {
            await Task.Run(() => {
                var persistentData = new UserPersistentData(ownerId) {
                    program = new UserProgram(string.Empty)
                };
                _persistentPlayerDataCollection.Upsert(persistentData);
                _persistentPlayerDataCollection.EnsureIndex(x => x.ownerId);
            });
        }

        public async Task removePersistentDataAsync(string ownerId) {
            await Task.Run(() => {
                _persistentPlayerDataCollection.Delete(x => x.ownerId == ownerId);
            });
        }

        public void deployProgram(string ownerId, UserProgram program) {
            // update the user's code in the database
            var data = findPersistentData(ownerId);
            data.program = program;
            _persistentPlayerDataCollection.Update(data);

            // reload the engine to apply changes
            serverContext.executors.reloadExecutor(ownerId);
        }

        public Queue<JToken> retrieveNotificationQueue(string userIdentifier) =>
            this[userIdentifier].queuedNotifications;
    }
}