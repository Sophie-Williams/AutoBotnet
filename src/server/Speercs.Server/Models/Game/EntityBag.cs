using Speercs.Server.Models.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Speercs.Server.Models.Game {
    public class EntityBag {
        public EntityBag() { }

        public Dictionary<string, GameEntity> entityData { get; set; } = new Dictionary<string, GameEntity>();

        public void insert(GameEntity entity) {
            this.entityData.Add(entity.id, entity);
        }

        public T get<T>(string id) where T : GameEntity {
            GameEntity entity = null;
            entityData.TryGetValue(id, out entity);
            return entity as T;
        }

        public List<GameEntity> getAllByUser(UserTeam user) {
            return user.ownedEntities.Select(entityId => entityData[entityId]).ToList();
        }
    }
}