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
            object result;
            try
            {
                var executor = ServerContext.Executors.RetrieveExecutor(rtContext.UserIdentifier);
                var command = data["command"].ToString();
                result = executor
                    .Execute(command)
                    .GetCompletionValue()
                    .ToObject();
            }
            catch
            {
                // there was an error
                result = null;
            }
            return Task.FromResult<JToken>(
                new JObject(
                    new JProperty("value", result),
                    new JProperty("error", result == null)
                )
            );
        }
    }
}
