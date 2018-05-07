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
        public int usedCoreSpace => _bot.usedCoreSpace;
        public int coreDrain => _bot.coreDrain;

        public bool move(Direction direction) {
            return _bot.move(direction);
        }
    }
}