using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities {
    public interface IBotCoreTemplate {
        (string, ulong)[] costs { get; }
        string name { get; }
        BotCore construct();
    }
}