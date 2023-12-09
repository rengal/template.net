using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Resto.Framework.Common
{
    [DebuggerStepThrough]
    public class ErrorEventArgs<TValue> : EventArgs
    {
        public ErrorEventArgs(TValue value, bool result)
        {
            Value = value;
            Result = result;
        }

        public TValue Value { get; }
        public bool Result { get; }
    }

    [Serializable]
    [DebuggerStepThrough]
    public class EventArgs<TValue> : EventArgs
    {
        public EventArgs(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }

    [DebuggerStepThrough]
    public class EventArgsWithResult<TValue, TResult> : EventArgs
    {
        public EventArgsWithResult(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
        public TResult Result { get; set; }
    }

    [DebuggerStepThrough]
    public class EventArgs<TSender, TValue> : EventArgs<TValue>
    {
        public EventArgs(TSender sender, TValue value)
            : base(value)
        {
            Sender = sender;
        }

        public TSender Sender { get; }
    }

    [DebuggerStepThrough]
    public class EmptyEventArgs<TSender> : EventArgs<TSender, object>
    {
        public EmptyEventArgs(TSender sender)
            : base(sender, null)
        {
        }
    }

    [DebuggerStepThrough]
    public class PropertyChangedEventArgs<T> : EventArgs<T>
    {
        public T OldValue { get; }

        public PropertyChangedEventArgs(T value, T oldValue) : base(value)
        {
            OldValue = oldValue;
        }
    }

    public static class NotifyCollectionChanged
    {
        public static readonly NotifyCollectionChangedEventArgs Reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}