using System.Linq;
using IridiumJS;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class BotEntityRef : GameEntityRef {
        private Bot _bot;

        public BotEntityRef(Bot bot) : base(bot) {
            _bot = bot;
        }

        public BotCoreRef[] cores => _bot.cores.Select(x => new BotCoreRef(x)).ToArray();
        public int coreCapacity => _bot.coreCapacity;
        public int reactorPower => _bot.reactorPower;
        public int usedCoreSpace => _bot.usedCoreSpace;
        public int coreDrain => _bot.coreDrain;
        public string model => _bot.model;

        public bool move(Direction direction) {
            return _bot.move(direction);
        }

        public override void destroy() {
            base.destroy();
            foreach (var coreRef in cores) coreRef.destroy();
            _bot = null;
        }
    }
}