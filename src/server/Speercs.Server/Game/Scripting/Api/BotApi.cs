using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors.Specialized;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting.Api {
    public class BotApi : ObjectInstance {
        public BotApi(ScriptExecutor executor, Bot bot) : base(executor.Engine) {
            Bot = bot;

            FastSetProperty("pos", new ClrAccessDescriptor(Engine, thisObj => {
                return new RoomPositionApi(executor, Bot.Position);
            }));
            FastAddProperty("id", Bot.ID, false, false, false);
            GameApi.SetDefaultToString(this);
        }

        public override string Class {
            get { return "Bot"; }
        }

        public Bot Bot { get; }
    }
}