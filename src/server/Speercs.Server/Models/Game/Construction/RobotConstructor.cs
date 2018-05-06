using System.Collections.Generic;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Models.Construction {
    public enum BotTemplates {
        Scout = 0
    }

    public static class RobotConstructor {
        public static Bot construct(BotTemplates templates, UserTeam team) {
            var costs = new List<(ResourceId, ulong)>();
            switch (templates) {
                case BotTemplates.Scout:
                    costs.Add((ResourceId.NRG, 40UL));
                    break;
            }

            if (!spendResources(team, costs)) return null;

            // TODO: create the bot
            throw new System.NotImplementedException();
        }

        private static bool spendResources(UserTeam team, IEnumerable<(ResourceId, ulong)> costs) {
            // ensure that the user can afford
            foreach (var cost in costs) {
                (ResourceId resource, ulong amount) = cost;
                if (team.resources[resource] < amount) {
                    return false;
                }
            }

            // spend the resources
            foreach (var cost in costs) {
                (ResourceId resource, ulong amount) = cost;
                team.resources[resource] -= amount;
            }

            return true;
        }
    }
}