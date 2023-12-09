using System;
using System.Collections.Generic;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public sealed class StackDisposable : IDisposable
    {
        [CanBeNull]
        private Stack<IDisposable> disposables = new Stack<IDisposable>();

        public bool IsDisposed
        {
            get { return disposables == null; }
        }

        public void Push([NotNull] IDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));
            if (disposables == null)
                throw new ObjectDisposedException(nameof(StackDisposable));

            disposables.Push(disposable);
        }

        public void Dispose()
        {
            if (disposables == null)
                return;

            var temp = disposables;
            disposables = null;

            while (temp.Count > 0)
                temp.Pop().Dispose();
        }
    }
}