using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class ProductSizeKey
    {
        /// <summary>
        /// Возвращает строковое представление продукта с размером в виде
        /// "Продукт (Размер)", если размер не <c>null</c>; если размер
        /// <c>null</c>, то возвращается просто название продкута.
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <param name="productSize">Размер</param>
        public static string AsString([NotNull] Product product, [CanBeNull] ProductSize productSize)
        {
            return productSize == null
                ? product.NameLocal
                : string.Format("{0} ({1})", product.NameLocal, productSize.NameLocal);
        }
        
        /// <summary>
        /// Перегрузка <see cref="AsString(Data.Product, Data.ProductSize)"/>
        /// для строковых значений
        /// </summary>
        public static string AsString(string productName, string productSizeName)
        {
            return string.IsNullOrWhiteSpace(productSizeName)
                ? productName
                : string.Format("{0} ({1})", productName, productSizeName);
        }

        public override string ToString()
        {
            return AsString(product, productSize);
        }

        protected bool Equals(ProductSizeKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(product, other.product) &&
                   Equals(productSize, other.productSize);
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
            var key = obj as ProductSizeKey;
            return Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                var hashCode = product != null ? product.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (productSize != null ? productSize.GetHashCode() : 0);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                return hashCode;
            }
        }
    }
}