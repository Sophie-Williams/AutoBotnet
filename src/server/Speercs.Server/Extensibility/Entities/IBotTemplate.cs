using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;

namespace Speercs.Server.Extensibility.Entities {
    public interface IBotTemplate : IBotMetaTemplate {
        (string, long)[] costs { get; }
        Bot construct(FactoryTower factory, UserTeam team);
    }
}