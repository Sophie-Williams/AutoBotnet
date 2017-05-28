using Speercs.Server.Models.Game.Entities;
using System;
using System.Collections.Generic;

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

        public List<GameEntity> GetAllByUser(UserTeam user)
        {
            var entities = new List<GameEntity>();
            user.ownedEntities.ForEach((entId) =>
            {
                entities.Add(this.EntityData[entId]);
            });
            return entities;
        }
    }
}
