using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using System.Threading.Tasks;
using Speercs.Server.Game.Scripting;

namespace Speercs.Server.Web.Realtime.Handlers
{
    public class InteractiveConsoleHandler : RealtimeHandler
    {
        public InteractiveConsoleHandler(ISContext context) : base(context, "console")
        {
        }

        public override Task<JToken> HandleRequestAsync(long id, JToken data, RealtimeContext rtContext)
        {
            return Task.FromResult<JToken>(
                new JValue(
                    ServerContext.Executors.RetrieveExecutor(rtContext.UserIdentifier)
                        .Execute(data["command"]
                        .ToString())
                )
            );
        }
    }
}
