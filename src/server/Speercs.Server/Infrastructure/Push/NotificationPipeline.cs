using Speercs.Server.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using Speercs.Server.Utilities;

namespace Speercs.Server.Infrastructure.Push
{
    public class NotificationPipeline : DependencyObject
    {
        protected ConcurrentDictionary<string, Pipelines<JObject, bool>> UserPipelines { get; set; } = new ConcurrentDictionary<string, Pipelines<JObject, bool>>();

        public NotificationPipeline(ISContext context) : base(context)
        {
        }

        public async Task PushMessageAsync(JToken data, string userIdentifier)
        {
            var dataBundle = new JObject(
                new JProperty("data", data),
                new JProperty("type", "push")
            );
            var userPipeline = RetrieveUserPipeline(userIdentifier);
            // make sure handlers are available to process the message
            if (userPipeline.HandlerCount > 0)
            {
                foreach (var handler in userPipeline.GetHandlers())
                {
                    if (await handler.Invoke(dataBundle)) break;
                }
            }
            else
            {
                // No handlers are currently available. Message should be added to persistent queue
                // TODO: Queue message
            }
        }

        public Pipelines<JObject, bool> RetrieveUserPipeline(string userIdentifier)
        {
            return UserPipelines.GetOrAdd(userIdentifier, key => {
                return new Pipelines<JObject, bool>();
            });
        }
    }
}
