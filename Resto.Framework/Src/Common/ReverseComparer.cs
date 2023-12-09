using System;
using System.Collections.Generic;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Src.Common
{
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> comparer;

        public ReverseComparer([NotNull] IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return comparer.Compare(y, x);
        }
    }
}