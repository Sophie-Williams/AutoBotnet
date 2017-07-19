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

        // Disable `Lack of Await` and `Avoid Async Void` compiler warnings
        // The only reason these functions are async is to avoid blocking
        // They also do not return anything, because there's nothing to return
        #pragma warning disable 1998, AvoidAsyncVoid
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
        #pragma warning restore 1998, AvoidAsyncVoid
    }
}