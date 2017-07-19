using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Game.Scripting
{
    public class EventQueue : DependencyObject
    {
        private Queue<JObject> EventBuffer { get; set; } = new Queue<JObject>();

        public EventQueue(ISContext serverContext) : base(serverContext)
        {
        }

        public async Task QueuePushAsync(JObject data, string userId)
        {
            EventBuffer.Enqueue(JObject.FromObject(new {
                userId = userId,
                data = data
            }));
            await SendQueuedEventsAsync();
        }

        public async Task SendQueuedEventsAsync()
        {
            while (EventBuffer.Count > 0)
            {
                var evt = EventBuffer.Dequeue();
                await ServerContext.NotificationPipeline.PushMessageAsync(evt["data"], evt["userId"].ToString());
            }
        }
    }
}