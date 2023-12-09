using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static partial class UiDispatcher
    {
        public static TResult ExecuteWithMessagePumping<TResult>([InstantHandle] this Func<TResult> func)
        {
            VerifyAccess();

            return Task<TResult>.Factory.StartNew(func, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ExecuteWithMessagePumping();
        }

        public static void ExecuteWithMessagePumping([InstantHandle] this Action action)
        {
            VerifyAccess();

            Task.Run(action).ExecuteWithMessagePumping();
        }

        public static void ExecuteWithMessagePumping(this Task task)
        {
            ExecuteWithMessagePumpingCore(task);
        }

        public static TResult ExecuteWithMessagePumping<TResult>(this Task<TResult> task)
        {
            ExecuteWithMessagePumpingCore(task);
            return task.Result;
        }

        private static void ExecuteWithMessagePumpingCore(Task task)
        {
            VerifyAccess();

            var frame = new DispatcherFrame();
            task.ContinueWith(_ => frame.Continue = false);
            Dispatcher.PushFrame(frame);

            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                if (task.IsFaulted)
                    throw e.Flatten().InnerException.PrepareForRethrow();
            }
        }
    }
}
