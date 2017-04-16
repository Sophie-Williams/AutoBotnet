using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speercs.Server.Utilities
{
    /// <summary>
    /// Represents a series of handlers that run in a specified order.
    /// Handlers will be called in order until one signals that the parameter has been handled.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class Pipelines<TInput, TResult>
    {
        protected List<Func<TInput, Task<TResult>>> Handlers { get; } = new List<Func<TInput, Task<TResult>>>();

        public void AddItemToStart(Func<TInput, Task<TResult>> handler)
        {
            lock (Handlers)
            {
                Handlers.Insert(0, handler);
            }
        }

        public void AddItemToEnd(Func<TInput, Task<TResult>> handler)
        {
            lock (Handlers)
            {
                Handlers.Add(handler);
            }
        }

        public IEnumerable<Func<TInput, Task<TResult>>> GetHandlers()
        {
            lock (Handlers)
            {
                foreach (var handler in Handlers)
                {
                    yield return handler;
                }
            }
        }
    }
}
