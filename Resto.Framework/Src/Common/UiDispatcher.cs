using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static partial class UiDispatcher
    {
        private static Dispatcher appDispatcher;
        private static readonly object SyncObject = new object();

        private static Dispatcher AppDispatcher
        {
            get
            {
                if (appDispatcher != null)
                    return appDispatcher;

                lock (SyncObject)
                {
                    if (appDispatcher == null)
                    {
                        Interlocked.Exchange(ref appDispatcher, Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher);
                    }
                }
                return appDispatcher;
            }
        }

        public static void VerifyAccess()
        {
            AppDispatcher.VerifyAccess();
        }

        public static bool CheckAccess()
        {
            return AppDispatcher.CheckAccess();
        }

        private static readonly Action DoNothing = () => { };

        public static void WaitForIdle()
        {
            AppDispatcher.Invoke(DispatcherPriority.ApplicationIdle, DoNothing);
        }

        #region ExecuteInUiDispatcherAsync
        public static void ExecuteAsync(this Action action)
        {
            AppDispatcher.BeginInvoke(DispatcherPriority.Send, action);
        }

        public static void ExecuteAsync(this Action action, DispatcherPriority priority)
        {
            AppDispatcher.BeginInvoke(priority, action);
        }

        public static void ExecuteAsync<T1>(this Action<T1> action, T1 arg1)
        {
            AppDispatcher.BeginInvoke(DispatcherPriority.Send, action, arg1);
        }

        public static void ExecuteAsync<T1>(this Action<T1> action, T1 arg1, DispatcherPriority priority)
        {
            AppDispatcher.BeginInvoke(priority, action, arg1);
        }

        public static void ExecuteAsync<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            AppDispatcher.BeginInvoke(DispatcherPriority.Send, action, arg1, arg2);
        }

        public static void ExecuteAsync<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            AppDispatcher.BeginInvoke(DispatcherPriority.Send, action, arg1, arg2, arg3);
        }

        #endregion

        #region Execute Action
        public static void Execute<T1, T2, T3>([InstantHandle] this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            Execute(() => action(arg1, arg2, arg3));
        }

        public static void Execute<T1, T2>([InstantHandle] this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            Execute(() => action(arg1, arg2));
        }

        public static void Execute<T1>([InstantHandle] this Action<T1> action, T1 arg1)
        {
            Execute(() => action(arg1));
        }

        public static void Execute([InstantHandle] this Action action)
        {
            Execute(action, DispatcherPriority.Send);
        }

        public static void Execute([InstantHandle] this Action action, DispatcherPriority priority)
        {
            if (AppDispatcher.CheckAccess())
            {
                action();
                return;
            }

            var exception = AppDispatcher.Invoke(() =>
            {
                try
                {
                    action();
                    return null;
                }
                catch (TargetInvocationException e)
                {
                    return ExceptionDispatchInfo.Capture(e.InnerException);
                }
                catch (Exception e)
                {
                    return ExceptionDispatchInfo.Capture(e);
                }
            }, priority);

            if (exception != null)
                exception.Throw();
        }
        #endregion

        #region Execute Func
        public static TResult Execute<T1, T2, T3, T4, TResult>([InstantHandle] this Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Execute(() => func(arg1, arg2, arg3, arg4));
        }

        public static TResult Execute<T1, T2, T3, TResult>([InstantHandle] this Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            return Execute(() => func(arg1, arg2, arg3));
        }

        public static TResult Execute<T1, T2, TResult>([InstantHandle] this Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            return Execute(() => func(arg1, arg2));
        }

        public static TResult Execute<T1, TResult>([InstantHandle] this Func<T1, TResult> func, T1 arg1)
        {
            return Execute(() => func(arg1));
        }

        public static TResult Execute<TResult>([InstantHandle] this Func<TResult> func)
        {
            return Execute(func, DispatcherPriority.Send);
        }

        public static TResult Execute<TResult>([InstantHandle] this Func<TResult> func, DispatcherPriority priority)
        {
            if (AppDispatcher.CheckAccess())
                return func();

            return AppDispatcher.Invoke(() =>
            {
                try
                {
                    return Either<TResult, ExceptionDispatchInfo>.CreateLeft(func());
                }
                catch (TargetInvocationException e)
                {
                    return Either<TResult, ExceptionDispatchInfo>.CreateRight(ExceptionDispatchInfo.Capture(e.InnerException));
                }
                catch (Exception e)
                {
                    return Either<TResult, ExceptionDispatchInfo>.CreateRight(ExceptionDispatchInfo.Capture(e));
                }
            }, priority)
            .Case(result => result, exception => { exception.Throw(); return default(TResult); });
        }
        #endregion
    }
}