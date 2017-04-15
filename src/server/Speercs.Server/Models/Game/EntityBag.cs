using System;
using System.Collections.Generic;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Models.Game
{
    public class EntityBag
    {
        public Dictionary<string, GameEntity> EntityData { get; set; } = new Dictionary<string, GameEntity>();

        public string Insert(GameEntity entity)
        {
            string newGuid = Guid.NewGuid().ToString("N");
            this.EntityData.Add(newGuid, entity);
            return newGuid;
        }
    }
}
