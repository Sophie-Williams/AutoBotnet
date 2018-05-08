using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using IridiumJS.Runtime;
using System;
using IridiumJS.Parser;

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
            } catch (ParserException ex) {
                error = $"parse: {ex}";
            } catch (JavaScriptException ex) {
                // there was an error
                error = ex.Message;
            } catch (Exception ex) {
                if (serverContext.configuration.sendInternalScriptErrors) {
                    error = ex.ToString();
                } else {
                    error = "true";
                }
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