using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Набор методов для работы с <see cref="Task"/>
    /// </summary>
    public static class TaskUtils
    {
        /// <summary>
        /// Создаёт консолидированную задачу, объединяющую все задачи из <paramref name="tasks"/>.
        /// Консолидирующая задача заканчивается после завершения всех задач из <paramref name="tasks"/>
        /// с «наихудшим» результатом (ошибка, отмена, успешное выполнение).
        /// </summary>
        /// <param name="tasks">
        /// Задачи, которые консолидируются.
        /// </param>
        /// <returns>
        /// Консолидированная задача.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tasks"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="tasks"/> не содержит ни одной задачи.
        /// </exception>
        [NotNull]
        public static Task Consolidate([NotNull] params Task[] tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));
            if (tasks.Length == 0)
                throw new ArgumentException("Tasks is empty", nameof(tasks));

            var tcs = new TaskCompletionSource<object>();

            Task.Factory.ContinueWhenAll(tasks, _ =>
            {
                try
                {
                    Task.WaitAll(tasks);
                    tcs.SetResult(null);
                }
                catch (AggregateException ae)
                {
                    if (ae.Flatten().InnerExceptions.All(ex => ex is TaskCanceledException || ex is OperationCanceledException))
                        tcs.SetCanceled();
                    else
                        tcs.SetException(ae);
                }

            });

            return tcs.Task;
        }

        [NotNull]
        public static Task<TResult> Consolidate<TSource, TResult>([NotNull] Func<IEnumerable<TSource>, TResult> continuationFunction, [NotNull] params Task<TSource>[] tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));
            if (tasks.Length == 0)
                throw new ArgumentException("Tasks is empty", nameof(tasks));

            var tcs = new TaskCompletionSource<TResult>();
            Task.Factory.ContinueWhenAll(tasks, results =>
            {
                try
                {
                    Task.WaitAll(tasks);
                    tcs.SetResult(continuationFunction(results.Select(x => x.Result)));
                }
                catch (AggregateException)
                {
                    var flattenedExceptions = results
                        .Where(result => result.Exception != null)
                        .SelectMany(result => result.Exception.Flatten().InnerExceptions);
                    if (flattenedExceptions.All(ex => ex is TaskCanceledException || ex is OperationCanceledException))
                        tcs.SetCanceled();
                    else
                        tcs.SetException(flattenedExceptions);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// При создании продолжений с помощью ContinueWith невозможно передать состояние, состояние родительской задачи теряется.
        /// Перегрузки ContinueWith, принимающие state, появились в .NET 4.5, соответственно, с переходом на 4.5 этот метод следует удалить.
        /// </summary>
        [NotNull]
        public static Task<T> WrapWithState<T>([NotNull] this Task<T> task, [CanBeNull] object state)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (ReferenceEquals(task.AsyncState, state))
                return task;

            var tcs = new TaskCompletionSource<T>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.SetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.SetCanceled();
                else
                    tcs.SetResult(t.Result);
            }, TaskScheduler.Default);
            return tcs.Task;
        }

        /// <summary>
        /// При создании продолжений с помощью ContinueWith невозможно передать состояние, состояние родительской задачи теряется.
        /// Перегрузки ContinueWith, принимающие state, появились в .NET 4.5, соответственно, с переходом на 4.5 этот метод следует удалить.
        /// </summary>
        [NotNull]
        public static Task WrapWithState([NotNull] this Task task, [CanBeNull] object state)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (ReferenceEquals(task.AsyncState, state))
                return task;

            var tcs = new TaskCompletionSource<object>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.SetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.SetCanceled();
                else
                    tcs.SetResult(null);
            }, TaskScheduler.Default);
            return tcs.Task;
        }

        [NotNull]
        public static Task HandleExceptions(this Task task, [NotNull] Action<AggregateException> exceptionHandler, TaskScheduler scheduler = null)
        {
            if (exceptionHandler == null)
                throw new ArgumentNullException(nameof(exceptionHandler));

            return task.HandleExceptions((e, _) => exceptionHandler(e), scheduler);
        }

        [NotNull]
        public static Task HandleExceptions(this Task task, [NotNull] Action<AggregateException, object> exceptionHandler, TaskScheduler scheduler = null)
        {
            if (exceptionHandler == null)
                throw new ArgumentNullException(nameof(exceptionHandler));

            // важно потрогать свойство Exception любой упавшей задачи, поэтому CancellationToken.None
            return task.ContinueWith(x => exceptionHandler(x.Exception, x.AsyncState), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, scheduler ?? TaskScheduler.Default);
        }

        /// <summary>
        /// Обёртывает <paramref name="result"/> в <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="result">Результат возвращаемой задачи.</param>
        /// <param name="state">Состояние, которое будет использования для создания задачи (<see cref="Task.AsyncState"/>).</param>
        /// <returns>Завершённая задача в статусе <see cref="TaskStatus.RanToCompletion"/>
        /// c <see cref="Task{TResult}.Result"/> равным <paramref name="result"/>.</returns>
        [NotNull]
        [Pure]
        public static Task<T> ToCompletedTask<T>(this T result, [CanBeNull] object state = null)
        {
            var tcs = new TaskCompletionSource<T>(state);
            tcs.SetResult(result);
            return tcs.Task;
        }
    }
}