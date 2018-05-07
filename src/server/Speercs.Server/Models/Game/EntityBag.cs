using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models {
    public class EntityBag {
        public ISContext serverContext;
        
        public Dictionary<string, GameEntity> entityData { get; set; } = new Dictionary<string, GameEntity>();

        public Dictionary<Point, List<GameEntity>> spatialHash { get; set; } =
            new Dictionary<Point, List<GameEntity>>();

        public void wake(GameEntity entity) {
            entity.loadContext(serverContext);
            entity.wake();
        }

        public void insertNew(GameEntity entity) {
            insertAA(entity);
            wake(entity);
        }

        public void insertAA(GameEntity entity) {
            entityData.Add(entity.id, entity);
            insertSpatialHash(entity);
        }

        /// <summary>
        /// Insert an entity into the spatial hash automatically. You probably want to use insert() unless you want to only modify the spatial hash.
        /// </summary>
        /// <param name="entity"></param>
        public void insertSpatialHash(GameEntity entity) {
            if (!spatialHash.ContainsKey(entity.position.roomPos)) {
                spatialHash[entity.position.roomPos] = new List<GameEntity>();
            }

            spatialHash[entity.position.roomPos].Add(entity);
        }

        public void remove(GameEntity entity) {
            entityData.Remove(entity.id);
        }

        public T get<T>(string id) where T : GameEntity {
            entityData.TryGetValue(id, out var entity);
            return entity as T;
        }

        public static IEnumerable<GameEntity> getByUser(UserTeam user) {
            return user.entities;
        }

        public IEnumerable<GameEntity> enumerate() => entityData.Values;
    }
}