using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Models.Game.Entities {
    public class Bot : GameEntity {
        public Bot(ISContext serverContext, RoomPosition pos, UserTeam team) : base(serverContext, pos) {
            this.team = team;
        }

        public override bool moveRelative(Direction direction) {
            if (serverContext.appState.tickCount <= lastMoveTime)
                return false; // already moved this tick

            if (base.moveRelative(direction)) {
                lastMoveTime = serverContext.appState.tickCount;
                return true;
            } else return false;
        }

        protected ulong lastMoveTime = 0;

        public UserTeam team { get; }
    }
}