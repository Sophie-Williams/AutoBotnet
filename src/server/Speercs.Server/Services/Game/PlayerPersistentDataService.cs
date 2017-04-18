using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;

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

        public UserPersistentData this[string ownerId] => persistentPlayerDataCollection.FindOne(x => x.OwnerId == ownerId);

        public async Task CreatePersistentData(string ownerId)
        {
            var persistentData = new UserPersistentData(ownerId);
            persistentPlayerDataCollection.Upsert(persistentData);
            persistentPlayerDataCollection.EnsureIndex(x => x.OwnerId);
        }
    }
}
