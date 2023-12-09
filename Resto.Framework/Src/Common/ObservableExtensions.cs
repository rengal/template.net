using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Helpers;

namespace Resto.Framework.Common
{
    public static class ObservableExtensions
    {
        [NotNull]
        public static IObservable<TObject> CreateObservableForPropertyChanged<TObject, TProperty>(
            [NotNull] this TObject trackingObject, [NotNull] Expression<Func<TObject, TProperty>> propertyGetterExpression)
            where TObject : class, INotifyPropertyChanged
        {
            if (trackingObject == null)
                throw new ArgumentNullException(nameof(trackingObject));
            if (propertyGetterExpression == null)
                throw new ArgumentNullException(nameof(propertyGetterExpression));

            var propertyName = ExpressionHelper.GetPropertyName(propertyGetterExpression);

            return Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    handler => trackingObject.PropertyChanged += handler,
                    handler => trackingObject.PropertyChanged -= handler)
                .Where(evt => evt.EventArgs.PropertyName == propertyName)
                .Select(_ => trackingObject);
        }

        [NotNull]
        public static IObservable<NotifyCollectionChangedEventArgs> CreateObservableForCollectionChanged([NotNull] this INotifyCollectionChanged collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    handler => collection.CollectionChanged += handler,
                    handler => collection.CollectionChanged -= handler)
                .Select(evt => evt.EventArgs);
        }

        [NotNull]
        public static IObservable<T> SkipAfter<T>([NotNull] this IObservable<T> source, [NotNull] Func<T, bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            return Observable.Create<T>(
                observer => source.Subscribe(
                    x =>
                    {
                        bool match;
                        try
                        {
                            match = condition(x);
                        }
                        catch (Exception e)
                        {
                            observer.OnError(e);
                            return;
                        }

                        observer.OnNext(x);
                        if (match)
                            observer.OnCompleted();
                    },
                    observer.OnError,
                    observer.OnCompleted));
        }

        [NotNull]
        public static IObservable<T> RetryWithDelay<T, TException>([NotNull] this IObservable<T> source, int retryCount, TimeSpan delay, CancellationToken token = default)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "retryCount must be nonnegative");

            if (delay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(delay), delay, "delay must be nonnegative");

            if (retryCount == 0)
                return Observable.Empty<T>();

            if (retryCount == 1)
                return source;

            return
                source.Catch<T, TException>(
                    exception =>
                    {
                        if (!token.IsCancellationRequested)
                            return Observable
                                .Timer(delay)
                                .SelectMany(_ => source.RetryWithDelay<T, TException>(retryCount - 1, delay, token));
                        throw exception.PrepareForRethrow();
                    });
        }
    }
}
