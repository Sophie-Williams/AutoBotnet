using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Buildings;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public class ScoutBotTemplate : IBotTemplate {
        public (string, long)[] costs { get; } = {("nrg", 40)};
        public string name => "scout";
        public int coreCapacity => 4;
        public int reactorPower => 20;
        public int memorySize => 32;
        public int health => 10;
        public int movecost => 0;

        public Bot construct(FactoryBuilding factory, UserEmpire team) =>
            DefaultBotTemplateConstructor.construct(factory, team, this);
    }

    public class RangerBotTemplate : IBotTemplate {
        public (string, long)[] costs { get; } = {("nrg", 80)};
        public string name => "ranger";
        public int coreCapacity => 6;
        public int reactorPower => 30;
        public int memorySize => 32;
        public int health => 20;
        public int movecost => 0;

        public Bot construct(FactoryBuilding factory, UserEmpire team) =>
            DefaultBotTemplateConstructor.construct(factory, team, this);
    }
}