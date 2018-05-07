using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public class CoreStorage1Template : IBotCoreTemplate {
        public (string, ulong)[] costs { get; } = {("iron", 5)};

        public string name { get; } = nameof(CoreStorage1);

        public BotCore construct() => new CoreStorage1();

        public class CoreStorage1 : BotCore {
            public override int drain { get; } = 10;
            public override BotCoreFlags flags { get; } = BotCoreFlags.None;
            public override int size { get; } = 1;
        }
    }
}