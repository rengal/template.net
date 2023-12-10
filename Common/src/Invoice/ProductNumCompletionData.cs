using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    #region ProductNumCompletionData

    /// <summary>
    /// Класс описывает данные для модели ProductNumCompletionModel.
    /// </summary>
    public sealed class ProductNumCompletionData : IComparable, IDeletable, IComparable<ProductNumCompletionData>
    {
        private Product product;

        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        public override bool Equals(object obj)
        {
            ProductNumCompletionData data = obj as ProductNumCompletionData;
            return data != null && data.Product != null && product != null ? data.Product.Id == product.Id : false;
        }

        public override int GetHashCode()
        {
            return product != null ? product.Id.GetHashCode() : 0;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is ProductNumCompletionData))
            {
                throw new ArgumentException("Object is not a ProductNumCompletionData");
            }
            return CompareTo((ProductNumCompletionData)obj);
        }

        public int CompareTo(ProductNumCompletionData other)
        {
            if (other == null || other.Product == null)
            {
                return 1;
            }
            
            if(Product == null)
            {
                return -1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            int result = Comparer<string>.Default.Compare(Product.Num, other.Product.Num);
            
            if (result == 0)
            {
                result = Product.CompareTo(other.Product);
            }

            return result;

        }

        public override string ToString()
        {
            return product != null ? product.Num : string.Empty;
        }

        public bool Deleted
        {
            get { return product != null && product.Deleted; }
        }

        /// <summary>
        /// Проверяет, совпадает ли продукт в переданной <paramref name="completionData"/>
        /// с продуктом <paramref name="product"/>. Если совпадает, то возвращает <paramref name="completionData"/>,
        /// иначе создаёт и возвращает новый инстанс <see cref="ProductNumCompletionData"/> для указанного продукта.
        /// </summary>
        /// <remarks>
        /// Нужно для использования в геттерах свойств, к которым происходят частые обращения
        /// (например, значение свойства используется в гриде), чтобы не создавать лишних
        /// <see cref="ProductNumCompletionData"/>, когда этого можно избежать.
        /// </remarks>
        [CanBeNull]
        public static ProductNumCompletionData SameOrNew([CanBeNull] ProductNumCompletionData completionData,
            [CanBeNull] Product product)
        {
            if (product == null)
            {
                return null;
            }

            if (completionData == null || !Equals(completionData.Product, product))
            {
                return new ProductNumCompletionData { Product = product };
            }

            return completionData;
        }
    }
    #endregion BarcodeCompletionData
}
