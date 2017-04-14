using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime.Handlers
{
    public class MapFetchRealtimeHandler : RealtimeHandler
    {
        public MapFetchRealtimeHandler(ISContext context) : base(context, "fetchmap")
        {
        }

        public override Task<JToken> HandleRequestAsync(long id, JToken data)
        {
            var dataBundle = (JObject)data;
            var x = dataBundle["x"].ToObject<int>();
            var y = dataBundle["y"].ToObject<int>();
            var room = ServerContext.AppState.WorldMap[x, y];
            if (room == null) return Task.FromResult<JToken>(JValue.CreateNull());
            return Task.FromResult<JToken>(JObject.FromObject(room));
        }
    }
}
