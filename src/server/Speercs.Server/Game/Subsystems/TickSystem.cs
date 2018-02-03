using System;
using System.Threading;
using System.Threading.Tasks;

namespace Speercs.Server.Game.Subsystems {
    public class TickSystem {
        public int delay { get; }

        public bool autoScaleTicks { get; }

        public Func<Task> tickAction { get; }

        public TickSystem(int delay, bool autoScaleTicks, Func<Task> tickAction) {
            this.delay = delay;
            this.autoScaleTicks = autoScaleTicks;
            this.tickAction = tickAction;
        }

        public async Task startAsync(CancellationToken ctok) {
            while (!ctok.IsCancellationRequested) {
                // wait
                await Task.Delay(delay);
                // run action
                await tickAction();
            }
        }
    }
}