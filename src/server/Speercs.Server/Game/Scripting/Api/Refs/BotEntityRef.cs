using IridiumJS;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class BotEntityRef : GameEntityRef {
        private Bot _bot;

        public BotEntityRef(JSEngine engine, Bot bot) : base(engine, bot) {
            _bot = bot;
        }

        public BotCore[] cores => _bot.cores.ToArray();

        public int coreCapacity => _bot.coreCapacity;

        public bool move(Direction direction) {
            return _bot.move(direction);
        }
    }
}