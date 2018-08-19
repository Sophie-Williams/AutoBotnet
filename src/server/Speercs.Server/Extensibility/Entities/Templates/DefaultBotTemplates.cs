﻿using Speercs.Server.Models;
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

        public Bot construct(FactoryBuilding factory, UserEmpire team) {
            var bot = new Bot(factory.position, team) {
                model = name,
                coreCapacity = coreCapacity,
                reactorPower = reactorPower,
                memory = new int[memorySize],
                moveCost = movecost
            };
            bot.resetHealth(health);
            return bot;
        }
    }
}