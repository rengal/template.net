using System;
using System.Windows;

namespace Resto.Framework
{
    public class DpChangedEventArgs : EventArgs
    {
        public object OldValueUntyped { get; private set; }
        public object NewValueUntyped { get; private set; }
        public DependencyProperty Property { get; private set; }

        public DpChangedEventArgs(DependencyPropertyChangedEventArgs baseArgs)
        {
            OldValueUntyped = baseArgs.OldValue;
            NewValueUntyped = baseArgs.NewValue;
            Property = baseArgs.Property;
        }
    }

    public sealed class DpChangedEventArgs<TProp> : DpChangedEventArgs
    {
        public DpChangedEventArgs(DependencyPropertyChangedEventArgs baseArgs)
            : base(baseArgs)
        {
            OldValue = (TProp)baseArgs.OldValue;
            NewValue = (TProp)baseArgs.NewValue;
        }

        public TProp OldValue { get; private set; }
        public TProp NewValue { get; private set; }
    }

    public delegate void DpChangedEventHandler<TProp>(object sender, DpChangedEventArgs<TProp> args);
}
