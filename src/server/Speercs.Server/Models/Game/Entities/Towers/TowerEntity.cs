using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities.Towers {
    public abstract class TowerEntity : GameEntity {
        protected TowerEntity(ISContext serverContext, RoomPosition pos, UserTeam team) :
            base(serverContext, pos, team) { }
    }
}