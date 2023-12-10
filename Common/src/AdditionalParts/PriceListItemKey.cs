using System;

namespace Resto.Data
{
    public partial class PriceListItemKey : IEquatable<PriceListItemKey>
    {
        /// <summary>
        /// Продукт с размером
        /// </summary>
        public ProductSizeKey ProductSizeKey
        {
            get { return new ProductSizeKey(product, productSize); }
        }

        public bool Equals(PriceListItemKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.product, product)
                && Equals(other.productSize, productSize)
                && Equals(other.department, department)
                && Equals(other.scheduledDocumentId, scheduledDocumentId);
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

            if (obj.GetType() != typeof (PriceListItemKey))
            {
                return false;
            }

            return Equals((PriceListItemKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = department.GetHashCode();
                hashCode = (hashCode*397) ^ product.GetHashCode();
                hashCode = (hashCode*397) ^ (productSize != null ? productSize.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ scheduledDocumentId.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(PriceListItemKey left, PriceListItemKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PriceListItemKey left, PriceListItemKey right)
        {
            return !Equals(left, right);
        }
    }
}