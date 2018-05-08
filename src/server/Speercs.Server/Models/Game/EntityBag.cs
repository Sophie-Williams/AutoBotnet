using System.Collections.Generic;
using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Math;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Models {
    public class EntityBag {
        [BsonField("entityData")]
        public Dictionary<string, GameEntity> entityData { get; set; } = new Dictionary<string, GameEntity>();

        [BsonField("spatialHash")]
        public Dictionary<string, List<GameEntity>> spatialHash { get; set; } =
            new Dictionary<string, List<GameEntity>>();

        [BsonIgnore]
        protected ISContext serverContext;

        [BsonIgnore]
        protected PersistentDataService dataService;

        public void initialize(ISContext context) {
            serverContext = context;
            dataService = new PersistentDataService(serverContext);
        }
        

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

        public IEnumerable<GameEntity> getByUser(string userId) {
            return dataService.get(userId).team.entities;
        }

        public IEnumerable<GameEntity> enumerate() => entityData.Values;
    }
}