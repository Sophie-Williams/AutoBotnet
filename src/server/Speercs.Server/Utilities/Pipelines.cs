using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Speercs.Server.Utilities {
    /// <summary>
    /// Represents a series of handlers that run in a specified order.
    /// Handlers will be called in order until one signals that the parameter has been handled.
    /// </summary>
    /// <typeparam name="TInput">The input type to each pipeline handler</typeparam>
    /// <typeparam name="TResult">The result of the pipeline handler</typeparam>
    public class Pipelines<TInput, TResult> {
        protected List<Func<TInput, Task<TResult>>> handlers { get; } = new List<Func<TInput, Task<TResult>>>();

        public void addStart(Func<TInput, Task<TResult>> handler) {
            lock (handlers) {
                handlers.Insert(0, handler);
            }
        }

        public void addEnd(Func<TInput, Task<TResult>> handler) {
            lock (handlers) {
                handlers.Add(handler);
            }
        }

        public IEnumerable<Func<TInput, Task<TResult>>> getHandlers() {
            lock (handlers) {
                foreach (var handler in handlers) {
                    yield return handler;
                }
            }
        }

        public int handlerCount => handlers.Count;

        public bool unregisterHandler(Func<TInput, Task<TResult>> pipelineHandler) => handlers.Remove(pipelineHandler);
    }
}