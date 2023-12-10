using System;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class StoreProductPair
    {
        protected bool Equals(StoreProductPair other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(store, other.store) &&
                   Equals(product, other.product);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as StoreProductPair;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                var hashCode = store != null ? store.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (product != null ? product.GetHashCode() : 0);
                return hashCode;
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
            }
        }

        public override string ToString()
        {
            return string.Format(Resources.StoreProductPairStoreProduct,
                Convert.ToString(store),
                Convert.ToString(product));
        }
    }
}