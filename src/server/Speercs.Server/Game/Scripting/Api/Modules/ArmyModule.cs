using System;
using System.Linq;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Models.Construction;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Mechanics;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ArmyModule : ScriptingApiModule {
        public ArmyModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            var userData = new PersistentDataService(context).get(userId);

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
                return (FactoryTower) userData.team.entities.Where(x => x is FactoryTower).ElementAt(seq);
            }

            bool constructBot(string templateName, FactoryTower factory) {
                var bot = RobotConstructor.construct(context, factory, templateName, userData.team);
                userData.team.addEntity(bot);
                return true;
            }

            defineFunction("boot", new Func<bool>(boot));
            defineFunction("getFactory", new Func<int, FactoryTower>(getFactory));
            defineFunction("constructBot", new Func<string, FactoryTower, bool>(constructBot));
        }
    }
}