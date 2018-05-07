using System;
using System.Collections.Generic;
using System.Linq;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Construction;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Mechanics;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ArmyModule : ScriptingApiModule {
        public ArmyModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            var dataService = new PersistentDataService(context);
            var userData = dataService.get(userId);

            bool boot() {
                // "boot up", or create the player's force
                if (userData.team.booted) {
                    // can't "boot" an already-booted team
                    return false;
                }

                var starter = new GameStarter(context);
                starter.bootTeam(userData.team);
                return true;
            }

            FactoryTower getFactory(int seq) {
                var factories = userData.team.entities.Where(x => x is FactoryTower).ToList();
                if (!factories.Any()) return null;
                return (FactoryTower) factories[seq];
            }

            bool constructBot(string templateName, FactoryTower factory) {
                var bot = RobotConstructor.construct(context, factory, templateName, userData.team);
                if (bot == null) return false;
                userData.team.addEntity(bot);
                return true;
            }

            GameEntity[] getUnits() {
                return userData.team.entities.ToArray();
            }

            ulong getResource(string resource) {
                return userData.team.getResource(resource);
            }

            defineFunction(nameof(boot), new Func<bool>(boot));
            defineFunction(nameof(getFactory), new Func<int, FactoryTower>(getFactory));
            defineFunction(nameof(constructBot), new Func<string, FactoryTower, bool>(constructBot));
            defineFunction(nameof(getUnits), new Func<GameEntity[]>(getUnits));
            defineFunction(nameof(getResource), new Func<string, ulong>(getResource));
        }
    }
}