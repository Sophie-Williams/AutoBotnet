using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models {
    public class EntityBag {
        public Dictionary<string, GameEntity> entityData { get; set; } = new Dictionary<string, GameEntity>();

        public Dictionary<Point, List<GameEntity>> spatialHash { get; set; } =
            new Dictionary<Point, List<GameEntity>>();

        public void insert(GameEntity entity) {
            entityData.Add(entity.id, entity);
            insertSpatialHash(entity);
        }

        private void insertSpatialHash(GameEntity entity) {
            if (!spatialHash.ContainsKey(entity.position.roomPos)) {
                spatialHash[entity.position.roomPos] = new List<GameEntity>();
            }

            spatialHash[entity.position.roomPos].Add(entity);
        }

        public T get<T>(string id) where T : GameEntity {
            GameEntity entity = null;
            entityData.TryGetValue(id, out entity);
            return entity as T;
        }

        public IEnumerable<GameEntity> getByUser(UserTeam user) {
            return user.ownedEntities.Select(entityId => entityData[entityId]);
        }
    }
}