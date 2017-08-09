using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Program;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Speercs.Server.Services.Game
{
    public class PlayerPersistentDataService : DependencyObject
    {
        public const string PlayerDataKey = "player_persistent_data";
        private LiteCollection<UserPersistentData> persistentPlayerDataCollection;

        public PlayerPersistentDataService(ISContext serverContext) : base(serverContext)
        {
            persistentPlayerDataCollection = serverContext.Database.GetCollection<UserPersistentData>(PlayerDataKey);
        }

        public UserPersistentData this[string ownerId] => FindPersistentData(ownerId);

        private UserPersistentData FindPersistentData(string ownerId)
        {
            return persistentPlayerDataCollection.FindOne(x => x.OwnerId == ownerId);
        }

        public async Task CreatePersistentDataAsync(string ownerId)
        {
            await Task.Run(() => 
            {
                var persistentData = new UserPersistentData(ownerId)
                {
                    Program = new UserProgram(string.Empty)
                };
                persistentPlayerDataCollection.Upsert(persistentData);
                persistentPlayerDataCollection.EnsureIndex(x => x.OwnerId);
            });
        }

        public async Task RemovePersistentDataAsync(string ownerId)
        {
            await Task.Run(() => 
            {
                persistentPlayerDataCollection.Delete(x => x.OwnerId == ownerId);
            });
        }

        public void DeployProgram(string ownerId, UserProgram program)
        {
            // update the user's code in the database
            var data = FindPersistentData(ownerId);
            data.Program = program;
            persistentPlayerDataCollection.Update(data);
            
            // reload the engine to apply changes
            ServerContext.Executors.ReloadExecutor(ownerId);
        }

        public Queue<JToken> RetrieveNotificationQueue(string userIdentifier) => this[userIdentifier].QueuedNotifications;
    }
}
