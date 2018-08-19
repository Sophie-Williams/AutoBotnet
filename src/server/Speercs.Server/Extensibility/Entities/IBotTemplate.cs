using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Buildings;

namespace Speercs.Server.Extensibility.Entities {
    public interface IBotTemplate : IBotMetaTemplate {
        (string, long)[] costs { get; }
        Bot construct(FactoryBuilding factory, UserEmpire team);
        int coreCapacity { get; }
        int reactorPower { get; }
        int memorySize { get; }
        int health { get; }
        int movecost { get; }
    }
}