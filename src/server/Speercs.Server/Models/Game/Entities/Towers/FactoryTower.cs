using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities.Towers {
    public class FactoryTower : TowerEntity {
        /// <summary>
        /// Bson constructor
        /// </summary>
        public FactoryTower() { }

        public FactoryTower(RoomPosition pos, UserEmpire team) :
            base(pos, team) { }
    }
}