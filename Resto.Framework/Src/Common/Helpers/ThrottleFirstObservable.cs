using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Src;

namespace Resto.Framework.Common.Helpers
{
    // Обсуждение есть тут (https://github.com/dotnet/reactive/issues/395)
    internal sealed class ThrottleFirstObservable<T> : IObservable<T>
    {
        private readonly IObservable<T> source;
        private readonly IScheduler scheduler;
        private readonly TimeSpan dueTime;

        internal ThrottleFirstObservable([NotNull] IObservable<T> source, TimeSpan dueTime, [NotNull] IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime), dueTime, "Throttling interval cannot be nagative.");

            this.source = source;
            this.dueTime = dueTime;
            this.scheduler = scheduler;
        }

        public IDisposable Subscribe([NotNull] IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new ThrottleFirstObserver(source, observer, dueTime, scheduler);
        }

        private sealed class ThrottleFirstObserver : IObserver<T>, IDisposable
        {
            private readonly object syncObj = new object();
            private readonly IObserver<T> underlyingObserver;
            private readonly IScheduler scheduler;
            private readonly TimeSpan dueTime;
            private readonly SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
            private readonly SerialDisposable scheduledTask = new SerialDisposable();

            private TimeStamp latestEventTime = TimeStamp.GetOldestTime();
            private Maybe<T> latestValue = Maybe<T>.Empty;
            private ulong id;

            internal ThrottleFirstObserver([NotNull] IObservable<T> source, [NotNull] IObserver<T> underlyingObserver, TimeSpan dueTime, [NotNull] IScheduler scheduler)
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                if (underlyingObserver == null)
                    throw new ArgumentNullException(nameof(underlyingObserver));
                if (scheduler == null)
                    throw new ArgumentNullException(nameof(scheduler));

                this.underlyingObserver = underlyingObserver;
                this.scheduler = scheduler;
                this.dueTime = dueTime;

                if (CurrentThreadScheduler.IsScheduleRequired)
                    CurrentThreadScheduler.Instance.Schedule(() => subscription.Disposable = source.Subscribe(this));
                else
                    subscription.Disposable = source.Subscribe(this);
            }

            public void Dispose()
            {
                subscription.Dispose();
                scheduledTask.Dispose();
            }

            public void OnCompleted()
            {
                try
                {
                    lock (syncObj)
                    {
                        if (latestValue.HasValue)
                        {
                            underlyingObserver.OnNext(latestValue.Value);
                            latestValue = Maybe<T>.Empty;
                        }
                        underlyingObserver.OnCompleted();
                    }
                }
                finally
                {
                    Dispose();
                }
            }

            public void OnError(Exception error)
            {
                try
                {
                    lock (syncObj)
                    {
                        latestValue = Maybe<T>.Empty;
                        underlyingObserver.OnError(error);
                    }
                }
                finally
                {
                    Dispose();
                }
            }

            public void OnNext(T value)
            {
                var failed = true;
                try
                {
                    lock (syncObj)
                    {
                        var now = TimeStamp.Now();
                        if (now - latestEventTime >= dueTime)
                        {
                            latestValue = Maybe<T>.Empty;
                            latestEventTime = now;
                            id++;
                            underlyingObserver.OnNext(value);
                        }
                        else
                        {
                            var firstQueue = latestValue.IsEmpty;
                            latestValue = value;
                            if (firstQueue)
                            {
                                id++;
                                scheduledTask.Disposable = scheduler.Schedule(id, dueTime, SendLatestValue);
                            }
                        }
                    }

                    failed = false;
                }
                finally
                {
                    if (failed)
                        Dispose();
                }
            }

            private IDisposable SendLatestValue(IScheduler _, ulong scheduledTaskId)
            {
                lock (syncObj)
                {
                    if (id == scheduledTaskId && latestValue.HasValue)
                        OnNext(latestValue.Value);
                }
                return Disposable.Empty;
            }
        }
    }
}
