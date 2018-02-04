using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;

namespace Speercs.Server.Web.Realtime.Handlers {
    public class InteractiveConsoleHandler : RealtimeHandler {
        public InteractiveConsoleHandler(ISContext context) : base(context, "console") { }

        public override Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            JToken jsonResult = JValue.CreateNull();
            bool error;
            try {
                var engine = serverContext.executors.retrieveExecutor(rtContext.userIdentifier).engine;
                var command = data["command"].ToString();
                var result = engine
                    .Execute(command)
                    .GetCompletionValue()
                    .ToObject();
                if (result != null) {
                    jsonResult = JToken.FromObject(result);
                }

                error = false;
            } catch {
                // there was an error
                error = true;
            }

            return Task.FromResult<JToken>(
                new JObject(
                    new JProperty("value", jsonResult),
                    new JProperty("error", error)
                )
            );
        }
    }
}