using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Extensibility.Entities {
    public interface IBotTemplate {
        (ResourceId, ulong)[] costs { get; }
        string name { get; }
        Bot construct(ISContext context, FactoryTower factory, UserTeam team);
    }
}