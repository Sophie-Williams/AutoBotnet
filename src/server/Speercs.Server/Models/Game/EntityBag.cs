using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models {
    public class EntityBag {
        public ISContext serverContext;

        public Dictionary<string, GameEntity> entityData { get; set; } = new Dictionary<string, GameEntity>();

        public Dictionary<string, List<GameEntity>> spatialHash { get; set; } =
            new Dictionary<string, List<GameEntity>>();

        public void wake(GameEntity entity) {
            entity.loadContext(serverContext);
            entity.wake();
        }

        public void insertNew(GameEntity entity) {
            register(entity);
            wake(entity);
        }

        public void register(GameEntity entity) {
            entityData.Add(entity.id, entity);
            insertSpatialHash(entity);
        }

        /// <summary>
        /// Insert an entity into the spatial hash automatically. You probably want to use register() unless you want to only modify the spatial hash.
        /// </summary>
        /// <param name="entity"></param>
        public void insertSpatialHash(GameEntity entity) {
            if (!spatialHash.ContainsKey(entity.position.roomPos.ToString())) {
                spatialHash[entity.position.roomPos.ToString()] = new List<GameEntity>();
            }

            spatialHash[entity.position.roomPos.ToString()].Add(entity);
        }

        public List<GameEntity> getByRoom(Point roomPos) {
            return spatialHash[roomPos.ToString()] ?? new List<GameEntity>();
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