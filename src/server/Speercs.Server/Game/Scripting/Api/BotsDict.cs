using System.Collections.Generic;
using System.Linq;
using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime;
using IridiumJS.Runtime.Descriptors;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting.Api
{
    public class BotsDict : ObjectInstance
    {
        protected ISContext context;
        protected ScriptExecutor executor;
        protected UserTeam team;
        
        public BotsDict(JSEngine engine, ISContext context, string userID) : base(engine)
        {
            this.context = context;
            team = context.AppState.PlayerData[userID];
            
            Extensible = false;
        }
        
        protected ScriptExecutor getExecutor()
        {
            return executor ?? (executor = context.Executors.RetrieveExecutor(team.UserIdentifier));
        }
        
        public override string Class
        {
            get { return "BotsDict"; }
        }
        
        public override JsValue Get(string propertyName)
        {
            var bot = context.AppState.Entities.EntityData[propertyName] as Bot;
            if (bot?.Team == team)
                return getExecutor().GetBotObject(bot);
            return JsValue.Undefined;
        }
        
        public override void Put(string propertyName, JsValue value, bool throwOnError)
        {
            if (throwOnError)
                throw new JavaScriptException(Engine.TypeError, "Not allowed to put properties on Game.bots");
        }
        
        public override IEnumerable<KeyValuePair<string, PropertyDescriptor>> GetOwnProperties()
        {
            return context.AppState.Entities.GetAllByUser(team)
                    .OfType<Bot>()
                    .Select(bot => new KeyValuePair<string, PropertyDescriptor>(
                        bot.ID,
                        new PropertyDescriptor(getExecutor().GetBotObject(bot), false, true, false)
                    ));
        }
        
        public override PropertyDescriptor GetOwnProperty(string propertyName)
        {
            var value = Get(propertyName);
            return value == JsValue.Undefined?
                        PropertyDescriptor.Undefined :
                        new PropertyDescriptor(value, false, true, false);
        }
        
        protected override void SetOwnProperty(string propertyName, PropertyDescriptor desc)
        {
            throw new JavaScriptException(Engine.TypeError, "Not allowed to set properties on Game.bots");
        }
        
        public override bool HasOwnProperty(string propertyName)
        {
            return (context.AppState.Entities.EntityData[propertyName] as Bot)?.Team == team;
        }
        
        public override void RemoveOwnProperty(string propertyName)
        {
            throw new JavaScriptException(Engine.TypeError, "Not allowed to remove properties from Game.bots");
        }
        
        public override bool DefineOwnProperty(string propertyName, PropertyDescriptor desc, bool throwOnError)
        {
            if (throwOnError)
                throw new JavaScriptException(Engine.TypeError, "Not allowed to define properties on Game.bots");
            return false;
        }
    }
}