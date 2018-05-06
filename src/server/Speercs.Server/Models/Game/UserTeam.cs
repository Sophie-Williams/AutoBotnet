using System.Collections.Generic;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Models {
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam {
        public List<GameEntity> entities { get; set; } = new List<GameEntity>();
        public Dictionary<ResourceId, ulong> resources { get; set; } = new Dictionary<ResourceId, ulong>();
        public bool booted = false;

        public ulong getResource(ResourceId resourceId) {
            if (resources.ContainsKey(resourceId)) {
                return resources[resourceId];
            }

            return 0;
        }

        public GameEntity addEntity(GameEntity entity) {
            entities.Add(entity);
            return entity;
        }
    }
}