using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities {
    public interface IBotCoreTemplate {
        (string, long)[] costs { get; }
        string name { get; }
        BotCore construct();
    }
}