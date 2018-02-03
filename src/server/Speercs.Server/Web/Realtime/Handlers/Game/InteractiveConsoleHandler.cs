using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime.Handlers {
    public class InteractiveConsoleHandler : RealtimeHandler {
        public InteractiveConsoleHandler(ISContext context) : base(context, "console") { }

        public override Task<JToken> HandleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            JToken jsonResult = JValue.CreateNull();
            bool error;
            try {
                var engine = ServerContext.Executors.RetrieveExecutor(rtContext.UserIdentifier).Engine;
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