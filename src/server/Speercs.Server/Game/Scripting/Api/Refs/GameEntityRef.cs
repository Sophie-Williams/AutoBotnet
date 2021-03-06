﻿using IridiumJS;
using Newtonsoft.Json;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class GameEntityRef : GameObjectRef {
        private GameEntity _entity;

        internal GameEntity target => _entity;

        public GameEntityRef(GameEntity entity) {
            _entity = entity;
        }

        public string id => _entity.id;
        public string type => _entity.type;
        public string teamId => _entity.teamId;
        public int health => _entity.health;
        [JsonIgnore] public int maxhealth => _entity.maxhealth;

        public RoomPosition pos => _entity.position;

        public virtual void destroy() {
            _entity = null;
        }
    }
}