using System.Threading.Tasks;
using Speercs.Server.Configuration;

namespace Speercs.Server.Infrastructure.Push
{
    public class NotificationPipeline : DependencyObject
    {
        public NotificationPipeline(ISContext context) : base(context)
        {
        }

        public async Task PushMessage()
        {
            await Task.Delay(0);
        }
    }
}
