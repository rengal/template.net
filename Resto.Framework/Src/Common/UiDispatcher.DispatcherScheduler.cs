using System;
using System.Reactive.Concurrency;

namespace Resto.Framework.Common
{
    public static partial class UiDispatcher
    {
        private static readonly Lazy<DispatcherScheduler> DispatcherSchedulerInstance =
            new Lazy<DispatcherScheduler>(() => new DispatcherScheduler(AppDispatcher), true);

        public static IScheduler DispatcherScheduler
        {
            get { return DispatcherSchedulerInstance.Value; }
        }
    }
}