using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipelines
{
    internal class PipeSchedulerDemo
    {
        public static void Main()
        {
            var writeScheduler = new SingleThreadPipeScheduler();
            var readScheduler = new SingleThreadPipeScheduler();

            var options = new PipeOptions(readerScheduler: readScheduler,
                                  writerScheduler: writeScheduler,
                                  useSynchronizationContext: false);
            var pipe = new Pipe(options);
        }
    }

    internal class SingleThreadPipeScheduler : PipeScheduler
    {
        private readonly BlockingCollection<(Action<object> Action, object State)> _queue = new BlockingCollection<(Action<object> Action, object State)>();
        private readonly Thread _thread;
        public SingleThreadPipeScheduler()
        {
            _thread = new Thread(DoWord);
            _thread.Start();
        }

        private void DoWord()
        {
            foreach (var item in _queue.GetConsumingEnumerable())
            {
                item.Action(item.State);
            }
        }

        public override void Schedule(Action<object?> action, object? state)
        {
            if (state is not null)
            {
                _queue.Add((action, state));
            }
        }
    }
}
