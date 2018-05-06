using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities {
    public class Bot : MobileEntity {
        public Bot(ISContext serverContext, RoomPosition pos, UserTeam team) : base(serverContext, pos, team) {
            this.team = team;
        }

        protected override bool moveRelative(Direction direction) {
            if (serverContext.appState.tickCount <= lastMoveTime)
                return false; // already moved this tick

            if (base.moveRelative(direction)) {
                lastMoveTime = serverContext.appState.tickCount;
                return true;
            }

            return false;
        }

        protected ulong lastMoveTime;

        public UserTeam team { get; }
    }
}