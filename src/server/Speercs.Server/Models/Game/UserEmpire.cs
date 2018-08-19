using System.Collections.Generic;
using LiteDB;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Models {
    /// <summary>
    /// Data model for storing information about an empire
    /// </summary>
    public class UserEmpire : DatabaseObject {
        [BsonIgnore]
        public List<GameEntity> entities { get; set; } = new List<GameEntity>();

        public string identifier { get; set; }
        public Dictionary<string, long> resources { get; set; } = new Dictionary<string, long>();
        public bool booted { get; set; } = false;
        public long bootTime { get; set; } = -1;

        public long getResource(string resourceId) {
            if (resources.ContainsKey(resourceId)) {
                return resources[resourceId];
            }

            return 0;
        }

        public void addResource(string resourceId, long amount) {
            if (!resources.ContainsKey(resourceId)) resources[resourceId] = 0;
            resources[resourceId] = resources[resourceId] + amount;
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