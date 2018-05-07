using System;
using System.Collections.Generic;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public class CoreStorage1Template : IBotCoreTemplate {
        public (string, ulong)[] costs { get; } = {("iron", 4)};

        public string name { get; } = nameof(CoreStorage1);

        public BotCore construct() => new CoreStorage1();

        public class CoreStorage1 : BotCore {
            public override Dictionary<string, long> qualities { get; } = new Dictionary<string, long> {
                ["storage"] = 10
            };

            public override int drain { get; } = 1;
            public override BotCoreFlags flags { get; } = BotCoreFlags.None;
            public override int size { get; } = 1;
        }
    }

    public class CoreDrill1Template : IBotCoreTemplate {
        public (string, ulong)[] costs { get; } = {("iron", 4)};

        public string name { get; } = nameof(CoreDrill1);

        public BotCore construct() => new CoreDrill1();

        public class CoreDrill1 : BotCore {
            public override Dictionary<string, long> qualities { get; } = new Dictionary<string, long> {
                ["mining"] = 1
            };

            public override int drain { get; } = 10;
            public override BotCoreFlags flags { get; } = BotCoreFlags.Switchable;
            public override int size { get; } = 1;

            public CoreDrill1() {
                defineFunction("drill", new Action(actionDrill));
            }

            public void actionDrill() {
                throw new NotImplementedException();
            }
        }
    }
}