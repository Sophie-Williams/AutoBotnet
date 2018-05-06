using System;
using System.Linq;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Construction;
using Speercs.Server.Models.Math;
using Speercs.Server.Models.Mechanics;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ArmyModule : ScriptingApiModule {
        public ArmyModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            var userData = new PersistentDataService(context).get(userId);

            bool boot() {
                // "boot up", or create the player's force
                if (userData.team.entities.Any()) {
                    // can't "boot" an existing team
                    return false;
                }

                var starter = new GameStarter(context);
                starter.bootTeam(userData.team);

                return true;
            }

            bool spawnUnit(int template) {
                var bot = RobotConstructor.construct((BotTemplates) template, userData.team);
                // TODO: Add the bot to the entity system
                context.appState.entities.insert(bot);
                userData.team.addEntity(bot);
                return true;
            }

            defineFunction("boot", new Func<bool>(boot));
            defineFunction("spawnUnit", new Func<int, bool>(spawnUnit));
        }
    }
}