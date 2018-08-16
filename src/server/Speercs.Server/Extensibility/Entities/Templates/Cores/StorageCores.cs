using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities.Templates.Cores {
    public abstract class CoreStorageBase : BotCore {
        public CoreStorageBase(int storage) {
            qualities[nameof(storage)] = storage;
        }

        public override BotCoreFlags flags { get; } = BotCoreFlags.None;
    }

    public class CoreStorage1Template : IBotCoreTemplate {
        public (string, long)[] costs { get; } = {("iron", 4)};

        public string name { get; } = nameof(CoreStorage1);

        public BotCore construct() => new CoreStorage1();

        public class CoreStorage1 : CoreStorageBase {
            public override int drain { get; } = 1;
            public override int size { get; } = 1;

            public CoreStorage1() : base(10) { }
        }
    }
}