using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Program;
using System.Threading.Tasks;

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
                var persistentData = new UserPersistentData(ownerId);
                persistentPlayerDataCollection.Upsert(persistentData);
                persistentPlayerDataCollection.EnsureIndex(x => x.OwnerId);
            });
        }

        public bool DeployProgram(string ownerId, UserProgram program)
        {
            var data = FindPersistentData(ownerId);
            data.Program = program;
            return persistentPlayerDataCollection.Update(data);
        }
    }
}
