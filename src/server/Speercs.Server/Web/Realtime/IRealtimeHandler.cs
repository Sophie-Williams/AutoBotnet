using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime {
    public interface IRealtimeHandler {
        string path { get; }

        Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext);
    }
}