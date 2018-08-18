using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities.Towers {
    public abstract class TowerEntity : GameEntity {
        /// <summary>
        /// Bson constructor
        /// </summary>
        public TowerEntity() { }

        protected TowerEntity(RoomPosition pos, UserEmpire team) :
            base(pos, team) { }
    }
}