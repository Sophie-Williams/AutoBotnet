using Newtonsoft.Json.Linq;
using  System.Collections.Generic;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game.Scripting
{
    public class EventQueue : DependencyObject
    {
        private List<JObject> Queue { get; set; }

        public EventQueue(ISContext serverContext) : base(serverContext)
        {
        }

        public async void QueuePush(JObject push, string userId) {
            Queue.Add(JObject.FromObject(new {
                userId = userId,
                data = push
            }));
            RunQueue();
        }

        public async void RunQueue() {
            foreach (var evt in Queue) {
                await ServerContext.NotificationPipeline.PushMessageAsync(evt["userId"], evt["data"].ToString());
                Queue.Remove(evt);
            }
        }
    }
}