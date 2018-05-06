using System;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Construction;
using Speercs.Server.Models.Math;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ArmyModule : ScriptingApiModule {
        public ArmyModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            var userData = new PersistentDataService(context).get(userId);

            bool spawnUnit(int template) {
                var bot = RobotConstructor.construct((BotTemplates) template, userData.team);
                // TODO: Add the bot to the entity system
                throw new System.NotImplementedException();
            }

            defineFunction("spawnUnit", new Func<int, bool>(spawnUnit));
        }
    }
}