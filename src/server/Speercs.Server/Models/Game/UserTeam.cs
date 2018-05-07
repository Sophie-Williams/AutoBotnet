using System.Collections.Generic;
using LiteDB;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Models {
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam : DatabaseObject {
        [BsonIgnore]
        public List<GameEntity> entities { get; set; } = new List<GameEntity>();

        public string identifier { get; set; }
        public Dictionary<string, ulong> resources { get; set; } = new Dictionary<string, ulong>();
        public bool booted { get; set; } = false;

        public ulong getResource(string resourceId) {
            if (resources.ContainsKey(resourceId)) {
                return resources[resourceId];
            }

            return 0;
        }

        public GameEntity addEntity(GameEntity entity) {
            entities.Add(entity);
            return entity;
        }

        public void removeEntity(GameEntity entity) {
            entities.Remove(entity);
        }
    }
}