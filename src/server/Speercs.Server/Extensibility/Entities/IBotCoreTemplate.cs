using Speercs.Server.Models.Entities;

namespace Speercs.Server.Extensibility.Entities {
    public interface IBotCoreTemplate : IBotMetaTemplate {
        (string, long)[] costs { get; }
        BotCore construct();
    }
}