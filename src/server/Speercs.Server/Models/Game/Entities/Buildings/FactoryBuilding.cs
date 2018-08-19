using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities.Buildings {
    public class FactoryBuilding : BuildingEntity {
        /// <summary>
        /// Bson constructor
        /// </summary>
        public FactoryBuilding() { }

        public FactoryBuilding(RoomPosition pos, UserEmpire team) :
            base(pos, team) { }
    }
}