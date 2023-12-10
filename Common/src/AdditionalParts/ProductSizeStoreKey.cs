namespace Resto.Data
{
    public partial class ProductSizeStoreKey
    {
        protected bool Equals(ProductSizeStoreKey other)
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
                   Equals(product, other.product) &&
                   Equals(productSize, other.productSize) &&
                   amountFactor == other.amountFactor;
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
            var key = obj as ProductSizeStoreKey;
            return Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = store != null ? store.GetHashCode() : 0;
                // ReSharper disable ConditionIsAlwaysTrueOrFalse Откуда-то бекофис берет ключи с null-ами вместо product, не было времени разбираться.
                hashCode = (hashCode * 397) ^ (product != null ? product.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (productSize != null ? productSize.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (amountFactor != null ? amountFactor.GetHashCode() : 0);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                return hashCode;
            }
        }
    }
}