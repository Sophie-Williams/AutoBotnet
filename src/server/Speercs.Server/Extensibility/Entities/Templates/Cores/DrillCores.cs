using System;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Mechanics;

namespace Speercs.Server.Extensibility.Entities.Templates.Cores {
    public abstract class CoreDrillBase : BotCore {
        public const string QUALITY_MINING = "mining";

        public CoreDrillBase(int mining) {
            qualities[QUALITY_MINING] = mining;

            defineAction("drill", new Func<double, bool>(actionDrill));
        }

        public bool actionDrill(double dir) {
            var direction = (Direction) ((int) dir);
            var interaction = new RoomTileInteraction(bot.context, bot.team);
            var targetTile = bot.position.move(direction);
            // drill target must be in the same room
            if (!bot.position.roomPos.equalTo(targetTile.roomPos)) return false;
            return interaction.drill(targetTile, (int) qualities[QUALITY_MINING]);
        }

        public override BotCoreFlags flags { get; } = BotCoreFlags.Switchable;
    }

    public class CoreDrill1Template : IBotCoreTemplate {
        public (string, long)[] costs { get; } = {("iron", 4)};

        public string name { get; } = nameof(CoreDrill1);

        public BotCore construct() => new CoreDrill1();

        public class CoreDrill1 : CoreDrillBase {
            public override int drain { get; } = 10;
            public override int size { get; } = 1;

            public CoreDrill1() : base(1) { }
        }
    }
}