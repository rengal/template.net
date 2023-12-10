using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    partial class PriceListItemDto
    {
        /// <summary>
        /// Если <paramref name="category"/> не <c>null</c>,
        /// возвращает цену продукта по данному элементу приказа об изменении прейскуранта для данной ценовой категории,
        /// иначе возвращает базовую цену.
        /// </summary>
        public decimal GetPriceForCategory([CanBeNull] ClientPriceCategory category)
        {
            if (category == null)
            {
                return Price.GetValueOrFakeDefault();
            }

            return PricesForCategories.ContainsKey(category.Id) ? PricesForCategories[category.Id] : Price.GetValueOrFakeDefault();
        }

        /// <summary>
        /// Выясняет, включен ли продукт в прайс по данной ценовой категории
        /// </summary>
        /// <param name="category">Ценовая категория</param>
        /// <returns>true, если в словаре соответствий "ценовая категория - признак включения" этот признак равен true, и false в противном случае</returns>
        public bool IsIncludedForCategory([CanBeNull] ClientPriceCategory category)
        {
            bool isIncluded;

            if (category == null)
            {
                isIncluded = Included;
            }
            else
            {
                isIncluded = IncludedForCategories.ContainsKey(category.Id) ? IncludedForCategories[category.Id] : Included;
            }

            return isIncluded;
        }
    }
}
