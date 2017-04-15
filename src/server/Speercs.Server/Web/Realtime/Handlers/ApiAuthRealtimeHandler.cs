using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime.Handlers
{
    public class ApiAuthRealtimeHandler : RealtimeHandler
    {
        public ApiAuthRealtimeHandler(ISContext context) : base(context, "auth")
        {
        }

        public override Task<JToken> HandleRequestAsync(long id, JToken data, RealtimeContext rtContext)
        {
            // take apikey
            var apikey = data.ToObject<string>();
            var result = rtContext.AuthenticateWith(apikey);

            return Task.FromResult<JToken>(JObject.FromObject(result));
        }
    }
}
