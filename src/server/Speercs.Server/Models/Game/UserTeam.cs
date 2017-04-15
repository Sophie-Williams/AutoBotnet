using System;
using System.Collections.Generic;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Models.Game
{
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam
    {
        public Dictionary<string, GameEntity> EntityData { get; set; } = new Dictionary<string, GameEntity>();

        public string InsertEntity(GameEntity entity)
        {
            string newGuid = Guid.NewGuid().ToString();
            this.EntityData.Add(newGuid, entity);
            return newGuid;
        }

        public GameEntity this[string guid]
        {
            get
            {
                if (!EntityData.ContainsKey(guid)) return null;
                return EntityData[guid];
            }
        }
    }
}
