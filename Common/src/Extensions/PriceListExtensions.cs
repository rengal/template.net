using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Extensions
{
    public static class PriceListExtensions
    {
        /// <summary>
        /// Возвращает коллекцию всех не удаленных связок прайс-листа
        /// </summary>
        [NotNull]
        public static IReadOnlyCollection<TItem> AllNotDeletedItems<TItem>([NotNull] this IPriceList<TItem> priceList) 
            where TItem : AbstractIncomingPriceListItem
        {
            return priceList.AllItems.Where(i => !i.Deleted).ToArray();
        }

        /// <summary>
        /// Возвращает список айтемов для <paramref name="nativeProduct"/>
        /// </summary>
        [NotNull]
        public static IEnumerable<TItem> GetItemsByNativeProduct<TItem>([NotNull] this IPriceList<TItem> priceList, [NotNull] Product nativeProduct)
            where TItem : AbstractIncomingPriceListItem
        {
            return priceList.AllNotDeletedItems().Where(i => i.NativeProduct.Equals(nativeProduct));
        }
    }
}
