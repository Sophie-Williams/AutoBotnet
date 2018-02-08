using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using IridiumJS.Runtime;
using System;

namespace Speercs.Server.Web.Realtime.Handlers {
    public class InteractiveConsoleHandler : RealtimeHandler {
        public InteractiveConsoleHandler(ISContext context) : base(context, "console") { }

        public override Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            JToken jsonResult = JValue.CreateNull();
            string error;
            try {
                var engine = serverContext.executors.retrieveExecutor(rtContext.userIdentifier).engine;
                var expression = data["expr"].ToString();
                var result = engine
                    .Execute(expression)
                    .GetCompletionValue()
                    .ToObject();
                if (result != null) {
                    jsonResult = JToken.FromObject(result);
                }

                error = null;
            } catch (JavaScriptException ex) {
                // there was an error
                error = ex.Message;
            } catch (Exception) {
                // for now no need to give debug info to clients
                error = null;
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