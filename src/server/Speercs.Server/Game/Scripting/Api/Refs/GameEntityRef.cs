using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class GameEntityRef {
        private GameEntity _entity;

        internal GameEntity target => _entity;

        public GameEntityRef(GameEntity entity) {
            _entity = entity;
        }

        public string id => _entity.id;
    }
}