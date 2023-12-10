using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Бэковское дополнение ProductItemCategory
    /// </summary>
    public partial class ProductItemCategory
    {
        /// <summary>
        /// Возвращает признак расчета по <paramref name="type"/>
        /// </summary>
        /// <param name="type">Тип продукта</param>
        [CanBeNull]
        public static ProductItemCategory GetByProductType([CanBeNull] ProductType type)
        {
            var allCategories = GetAllNotDeleted();
            if (type == null)
            {
                return null;
            }

            if (type.In(ProductType.PREPARED, ProductType.DISH, ProductType.GOODS, ProductType.MODIFIER))
            {
                return allCategories.Single(category => category.Id.Equals(PredefinedGuids.PRODUCT_ITEM_CATEGORY_GOODS.Id));
            }

            return type.In(ProductType.SERVICE, ProductType.RATE)
                ? allCategories.Single(category => category.Id.Equals(PredefinedGuids.PRODUCT_ITEM_CATEGORY_SERVICE.Id))
                : allCategories.Single(category => category.Id.Equals(PredefinedGuids.PRODUCT_ITEM_CATEGORY_OTHER.Id));
        }

        /// <summary>
        /// Возвращает список всех признаков расчета
        /// </summary>
        public static IEnumerable<ProductItemCategory> GetAllNotDeleted()
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<ProductItemCategory>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
