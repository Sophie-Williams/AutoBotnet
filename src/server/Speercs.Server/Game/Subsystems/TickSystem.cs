using System;
using System.Threading;
using System.Threading.Tasks;

namespace Speercs.Server.Game.Subsystems {
    public class TickSystem {
        public int Delay { get; }

        public bool AutoScaleTicks { get; }

        public Func<Task> TickAction { get; }

        public TickSystem(int delay, bool autoScaleTicks, Func<Task> tickAction) {
            Delay = delay;
            AutoScaleTicks = autoScaleTicks;
            TickAction = tickAction;
        }

        public async Task StartAsync(CancellationToken ctok) {
            while (!ctok.IsCancellationRequested) {
                // wait
                await Task.Delay(Delay);
                // run action
                await TickAction();
            }
        }
    }
}