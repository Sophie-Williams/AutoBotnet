using System;
using Speercs.Server.Game.Scripting.Api.Refs;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Entities.Buildings {
    public class FactoryBuilding : BuildingEntity {
        /// <summary>
        /// Bson constructor
        /// </summary>
        public FactoryBuilding() { }

        public FactoryBuilding(RoomPosition pos, UserEmpire team) :
            base(pos, team) {
            
            resetHealth(400);
            defineAction("repair", new Func<BotEntityRef, bool>(actionRepair));
        }

        private bool actionRepair(BotEntityRef botRef) {
            if (botRef == null) return false;
            // make sure they are adjacent
            if (!position.roomPos.equalTo(botRef.pos.roomPos)) return false;
            if (Point.chDist(position.pos, botRef.pos.pos) > 1) return false;
            // repair the botRef to max health
            var bot = ((Bot) botRef.target);
            bot.health = bot.maxhealth;
            return true;
        }
    }
}