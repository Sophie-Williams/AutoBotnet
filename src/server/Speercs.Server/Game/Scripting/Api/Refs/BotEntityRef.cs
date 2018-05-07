using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class BotEntityRef : GameEntityRef {
        private Bot _bot;

        public BotEntityRef(Bot bot) : base(bot) {
            _bot = bot;
        }

        public BotCore[] getCores() => _bot.cores.ToArray();

        public int coreCapacity => _bot.coreCapacity;
    }
}