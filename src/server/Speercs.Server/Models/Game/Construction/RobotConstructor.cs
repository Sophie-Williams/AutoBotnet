using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Models.Construction {
    public enum BotTemplates {
        Scout = 0
    }

    public static class RobotConstructor {
        public static Bot construct(ISContext context, FactoryTower factory, BotTemplates template, UserTeam team) {
            var costs = new List<(ResourceId, ulong)>();
            switch (template) {
                case BotTemplates.Scout:
                    costs.Add((ResourceId.NRG, 40UL));
                    break;
            }

            if (!spendResources(team, costs)) return null;

            // build the bot
            var bot = default(Bot);
            switch (template) {
                case BotTemplates.Scout:
//                    bot = new Bot(context, );
                    break;
            }

            // TODO: create the bot
            // TODO: bot components/module system
            throw new System.NotImplementedException();
        }

        private static bool spendResources(UserTeam team, IEnumerable<(ResourceId, ulong)> costs) {
            // ensure that the user can afford
            var costList = costs.ToList();
            foreach (var cost in costList) {
                (ResourceId resource, ulong amount) = cost;
                if (team.resources[resource] < amount) {
                    return false;
                }
            }

            // spend the resources
            foreach (var cost in costList) {
                (ResourceId resource, ulong amount) = cost;
                team.resources[resource] -= amount;
            }

            return true;
        }
    }
}