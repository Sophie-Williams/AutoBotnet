using System;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities.Templates.Cores {
    public abstract class CoreDrillBase : BotCore {
        public CoreDrillBase(int mining) {
            qualities[nameof(mining)] = mining;

            defineFunction("drill", new Func<DrillResult>(actionDrill));
        }

        public DrillResult actionDrill() {
            return new DrillResult(false, amount: 0);
//            throw new NotImplementedException();
        }

        public struct DrillResult {
            public bool result;
            public int amount;

            public DrillResult(bool result, int amount) {
                this.result = result;
                this.amount = amount;
            }
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