using System;
using System.Collections.Generic;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Src.Common
{
    public sealed class ByKeyComparer<TItem, TKey> : IComparer<TItem>
    {
        private readonly Func<TItem, TKey> getKey;
        private readonly IComparer<TKey> keyComparer;

        public ByKeyComparer([NotNull] Func<TItem, TKey> getKey)
            : this(getKey, Comparer<TKey>.Default)
        {}

        public ByKeyComparer([NotNull] Func<TItem, TKey> getKey, [NotNull] IComparer<TKey> keyComparer)
        {
            if (getKey == null)
                throw new ArgumentNullException(nameof(getKey));
            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));

            this.getKey = getKey;
            this.keyComparer = keyComparer;
        }

        public int Compare(TItem x, TItem y)
        {
            return keyComparer.Compare(getKey(x), getKey(y));
        }
    }
}