using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities {
    public class Bot : MobileEntity {
        public Bot(ISContext serverContext, RoomPosition pos, UserTeam team, int coreCapacity) : base(serverContext, pos, team) {
            this.team = team;
            this.coreCapacity = coreCapacity;
        }
        
        public int coreCapacity { get; } 

        private List<BotCore> _cores = new List<BotCore>();

        public BotCore[] getCores() {
            return _cores.ToArray();
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

    public class BotCore {
        
    }
}