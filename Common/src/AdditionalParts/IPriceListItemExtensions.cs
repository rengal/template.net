using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    /// <summary>
    ///  Класс-расширение для интерфейса <see cref="IPriceListItem" />,
    ///  содержит в себе общие методы для классов-реализаций PriceListItem и SimplePriceListItem.
    /// </summary>
// ReSharper disable InconsistentNaming
    public static class IPriceListItemExtensions
// ReSharper restore InconsistentNaming
    {
        /// <summary>
        ///  Если <paramref name="category" /> не Null,
        ///  возвращает цену продукта по данному элементу приказа об изменении прейскуранта для данной ценовой категории,
        ///  иначе возвращает базовую цену.
        /// </summary>
        public static decimal GetPriceForCategory(this IPriceListItem priceListItem,
                                                  [CanBeNull] ClientPriceCategory category)
        {
            if (category == null)
            {
                return priceListItem.Price.GetValueOrFakeDefault();
            }
            return priceListItem.PricesForCategories.ContainsKey(category)
                       ? priceListItem.PricesForCategories[category]
                       : priceListItem.Price.GetValueOrFakeDefault();
        }

        /// <summary>
        ///  Выясняет, включен ли продукт в прайс по данной ценовой категории
        /// </summary>
        /// <param name="priceListItem"></param>
        /// <param name="category">Ценовая категория</param>
        /// <returns>true, если в словаре соответствий "ценовая категория - признак включения" этот признак равен true, и false в противном случае</returns>
        public static bool IsIncludedForCategory(this IPriceListItem priceListItem,
                                                 [CanBeNull] ClientPriceCategory category)
        {
            bool isIncluded;

            if (category == null)
            {
                isIncluded = priceListItem.Included;
            }
            else
            {
                isIncluded = priceListItem.IncludeForCategories.ContainsKey(category)
                                 ? priceListItem.IncludeForCategories[category]
                                 : priceListItem.Included;
            }

            return isIncluded;
        }
    }
}
