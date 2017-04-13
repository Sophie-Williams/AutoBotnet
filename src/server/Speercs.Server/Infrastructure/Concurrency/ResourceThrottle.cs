using System;
using System.Threading;
using System.Threading.Tasks;

namespace Speercs.Server.Infrastructure.Concurrency
{
    public class ResourceThrottle
    {
        private readonly Semaphore _throttle;

        public ResourceThrottle(int maxConcurrent)
        {
            _throttle = new Semaphore(maxConcurrent, maxConcurrent);
        }

        public void Acquire()
        {
            _throttle.WaitOne();
        }

        public async Task AcquireAsync()
        {
            await Task.Run(() => Acquire());
        }

        public void Release()
        {
            _throttle.Release();
        }

        public async Task WithResourceAsync(Func<Task> action)
        {
            await AcquireAsync();
            await action();
            Release();
        }
    }
}
