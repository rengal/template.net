using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public sealed class EqualityComparerByKey<TObject, TKey> : IEqualityComparer<TObject>
    {
        private readonly Func<TObject, TKey> getKey;
        private static readonly EqualityComparer<TKey> KeysComparer = EqualityComparer<TKey>.Default;

        public EqualityComparerByKey([NotNull] Func<TObject, TKey> getKey)
        {
            if (getKey == null)
                throw new ArgumentNullException(nameof(getKey));

            this.getKey = getKey;
        }

        public bool Equals(TObject x, TObject y)
        {
            return KeysComparer.Equals(getKey(x), getKey(y));
        }

        public int GetHashCode(TObject obj)
        {
            return KeysComparer.GetHashCode(getKey(obj));
        }
    }

    public static class EqualityComparerByKey<TObject>
    {
        [NotNull, Pure]
        public static IEqualityComparer<TObject> Create<TKey>([NotNull] Func<TObject, TKey> getKey)
        {
            return new EqualityComparerByKey<TObject, TKey>(getKey);
        }
    }
}
