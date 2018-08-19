using System;
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

        public Bot(RoomPosition pos, UserEmpire team) : base(pos, team) { }

        [BsonField("coreCapacity")]
        public int coreCapacity { get; set; }

        [BsonField("reactorPower")]
        public int reactorPower { get; set; }

        [BsonField("model")]
        public string model { get; set; }

        [JsonIgnore]
        [BsonField("cores")]
        public List<BotCore> cores { get; set; } = new List<BotCore>();

        [JsonIgnore]
        [BsonField("memory")] public int[] memory { get; set; }

        [BsonIgnore]
        public int usedCoreSpace => cores.Sum(x => x.size);

        [BsonIgnore]
        public int coreDrain => cores.Sum(x => x.drain);

        protected override bool moveRelative(Direction direction) {
            if (context.appState.tickCount <= lastMoveTime)
                return false; // already moved this tick

            if (base.moveRelative(direction)) {
                lastMoveTime = context.appState.tickCount;
                return true;
            }

            return false;
        }

        public override void wake() {
            base.wake();
            
            // set core properties
            foreach (var core in cores) {
                core.bot = this;
            }
        }

        [BsonIgnore] protected ulong lastMoveTime;
    }

    [Flags]
    public enum BotCoreFlags {
        None = 0,
        Switchable = 1 << 0,
    }

    public abstract class BotCore {
        [BsonIgnore] [JsonIgnore] public Bot bot;

        [BsonIgnore]
        public string type => this.GetType().Name;

        public Dictionary<string, long> qualities { get; } = new Dictionary<string, long>();
        public abstract int drain { get; }
        public abstract BotCoreFlags flags { get; }
        public abstract int size { get; }

        [BsonIgnore]
        public Dictionary<string, Delegate> actions { get; } = new Dictionary<string, Delegate>();

        protected void defineFunction(string name, Delegate func) {
            actions[name] = func;
        }
    }
}