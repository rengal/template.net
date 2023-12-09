using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resto.Framework.Common
{
    public static partial class UiDispatcher
    {
        private static readonly Lazy<UiDispatcherTaskScheduler> TaskSchedulerInstance =
            new Lazy<UiDispatcherTaskScheduler>(() => new UiDispatcherTaskScheduler(), true);

        public static TaskScheduler TaskScheduler
        {
            get { return TaskSchedulerInstance.Value; }
        }

        private sealed class UiDispatcherTaskScheduler : TaskScheduler
        {
            protected override void QueueTask(Task task)
            {
                ExecuteAsync(() => TryExecuteTask(task));
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return CheckAccess() && TryExecuteTask(task);
            }

            protected override IEnumerable<Task> GetScheduledTasks()
            {
                throw new NotSupportedException();
            }

            public override int MaximumConcurrencyLevel
            {
                get { return 1; }
            }
        }
    }
}