using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Game;
using Speercs.Server.Utilities;

namespace Speercs.Server.Infrastructure.Push {
    public class NotificationPipeline : DependencyObject {
        protected ConcurrentDictionary<string, Pipelines<JObject, bool>> userPipelines { get; set; } =
            new ConcurrentDictionary<string, Pipelines<JObject, bool>>();

        public NotificationPipeline(ISContext context) : base(context) { }

        public async Task pushMessageAsync(JToken data, string source, string userIdentifier, bool queue = false) {
            var dataBundle = new JObject(
                new JProperty("data", data),
                new JProperty("type", "push"),
                new JProperty("source", source)
            );
            var userPipeline = retrieveUserPipeline(userIdentifier);
            // make sure handlers are available to process the message
            if (userPipeline.handlerCount > 0) {
                foreach (var handler in userPipeline.GetHandlers()) {
                    if (await handler.Invoke(dataBundle)) break;
                }
            } else if (queue) {
                // No handlers are currently available. Message should be added to persistent queue of undelivered messages.
                // get player data service, and retrieve queued notifications container
                var userNotificationQueue =
                    new PersistentDataService(serverContext).retrieveNotificationQueue(userIdentifier);
                if (userNotificationQueue.Count >= serverContext.configuration.notificationQueueMaxSize) {

                } else {
                    // Enqueue the data.
                    userNotificationQueue.Enqueue(data);
                }
            }
        }

        public Pipelines<JObject, bool> retrieveUserPipeline(string userIdentifier) {
            return userPipelines.GetOrAdd(userIdentifier, key => {
                return new Pipelines<JObject, bool>();
            });
        }
    }
}