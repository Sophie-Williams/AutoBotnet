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
            this.coreCapacity = coreCapacity;
        }

        [BsonField("coreCapacity")]
        public int coreCapacity { get; set; }

        [JsonIgnore]
        [BsonField("cores")]
        public List<BotCore> cores { get; set; } = new List<BotCore>();

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