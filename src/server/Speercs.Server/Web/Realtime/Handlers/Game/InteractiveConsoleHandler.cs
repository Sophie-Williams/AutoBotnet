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
            var result = default(object);
            try
            {
                var executor = ServerContext.Executors.RetrieveExecutor(rtContext.UserIdentifier);
                var command = data["command"].ToString();
                var result = executor
                    .Execute(command)
                    .GetCompletionValue()
                    .ToObject();
            }
            return Task.FromResult<JToken>(
                JValue.FromObject(
                    new JProperty("value", JValue.FromObject(result)),
                    new JProperty("error", result == null)
                )
            );
        }
    }
}
