using IridiumJS;
using Newtonsoft.Json;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class GameEntityRef : GameObjectRef {
        private GameEntity _entity;

        internal GameEntity target => _entity;

        public GameEntityRef(JSEngine engine, GameEntity entity) : base(engine) {
            _entity = entity;
        }

        public string id => _entity.id;
        public string type => _entity.type;

        [JsonIgnore]
        public string teamId => _entity.teamId;

        public RoomPosition pos => _entity.position;
    }
}