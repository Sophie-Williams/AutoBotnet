using Speercs.Server.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using Speercs.Server.Utilities;

namespace Speercs.Server.Infrastructure.Push
{
    public class NotificationPipeline : DependencyObject
    {
        protected ConcurrentDictionary<string, Pipelines<JObject, bool>> UserPipelines { get; set; }

        public NotificationPipeline(ISContext context) : base(context)
        {
        }

        public async Task PushMessage(JToken data, string userIdentifier)
        {
            var dataBundle = new JObject(
                new JProperty("data", data),
                new JProperty("type", "push")
            );
            foreach (var handler in RetrieveUserPipeline(userIdentifier).GetHandlers())
            {
                if (await handler.Invoke(dataBundle)) break;
            }
        }

        public Pipelines<JObject, bool> RetrieveUserPipeline(string userIdentifier)
        {
            if (!UserPipelines.ContainsKey(userIdentifier)) UserPipelines.TryAdd(userIdentifier, new Pipelines<JObject, bool>());
            return UserPipelines[userIdentifier];
        }
    }
}
