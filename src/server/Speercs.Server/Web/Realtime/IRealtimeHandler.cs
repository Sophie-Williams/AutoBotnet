using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Speercs.Server.Web.Realtime {
    public interface IRealtimeHandler {
        string path { get; }

        Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext);
    }
}