using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ProductScale : IProductScaleTreeItem
    {
        Guid? IProductScaleTreeItem.ParentId
        {
            get { return null; }
        }

        string IProductScaleTreeItem.Name
        {
            get { return NameLocal; }
        }

        string IProductScaleTreeItem.ShortName
        {
            get { return string.Empty; }
        }

        bool? IProductScaleTreeItem.IsDefault
        {
            get { return null; }
        }

        ProductScale IProductScaleTreeItem.Scale
        {
            get { return this; }
        }

        bool IProductScaleTreeItem.IsParentDeleted
        {
            get { return false; }
        }

        /// <summary>
        /// Возвращает размеры шкалы
        /// </summary>
        /// <param name="product">Продукт, для которого нужно возвращать размеры</param>
        /// <returns>Если <paramref name="product"/> не <c>null</c>, то возвращаются только те
        /// размеры шкалы, которые доступны для этого продукта; иначе - все размеры шкалы.
        /// В обоих случаях возвращаются только неудалённые размеры.</returns>
        public IEnumerable<ProductSize> GetSizes(Product product = null)
        {
            Func<ProductSize, bool> predicate = product == null
                ? (Func<ProductSize, bool>)(productSize => Equals(productSize.ProductScale))
                : (productSize => Equals(productSize.ProductScale) && !product.DisabledSizes.Contains(productSize));
            return EntityManager.INSTANCE
                .GetAllNotDeleted(predicate)
                .OrderBy(productSize => productSize.Priority);
        }
    }
}