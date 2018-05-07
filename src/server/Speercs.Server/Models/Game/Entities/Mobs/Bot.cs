using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities {
    public class Bot : MobileEntity {
        /// <summary>
        /// BsonConstructor
        /// </summary>
        public Bot() { }

        public Bot(RoomPosition pos, UserTeam team, int coreCapacity) : base(pos, team) {
            _coreCapacity = coreCapacity;
        }

        [BsonField("coreCapacity")] private int _coreCapacity { get; set; }

        public int coreCapacity => _coreCapacity;

        [JsonIgnore]
        [BsonField("cores")]
        private List<BotCore> _cores { get; set; } = new List<BotCore>();

        public BotCore[] getCores() {
            return _cores.ToArray();
        }

        protected override bool moveRelative(Direction direction) {
            if (_context.appState.tickCount <= lastMoveTime)
                return false; // already moved this tick

            if (base.moveRelative(direction)) {
                lastMoveTime = _context.appState.tickCount;
                return true;
            }

            return false;
        }

        protected ulong lastMoveTime;
    }

    public class BotCore { }
}