using Speercs.Server.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Speercs.Server.Infrastructure.Push
{
    public class NotificationPipeline : DependencyObject
    {
        public NotificationPipeline(ISContext context) : base(context)
        {
        }

        public async Task PushMessage(JToken data)
        {
            var dataBundle = new JObject(
                new JProperty("data", data),
                new JProperty("type", "push")
            );
        }
    }
}
