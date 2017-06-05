using Speercs.Server.Models.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Speercs.Server.Models.Game
{
    public class EntityBag
    {
        public EntityBag()
        {
        }

        public Dictionary<string, GameEntity> EntityData { get; set; } = new Dictionary<string, GameEntity>();

        public void Insert(GameEntity entity)
        {
            this.EntityData.Add(entity.ID, entity);
        }
        
        public T Get<T>(string id) where T : GameEntity
        {
            GameEntity entity = null;
            EntityData.TryGetValue(id, out entity);
            return entity as T;
        }

        public List<GameEntity> GetAllByUser(UserTeam user)
        {
            return user.OwnedEntities.Select(entityID => EntityData[entityID]).ToList();
        }
    }
}
