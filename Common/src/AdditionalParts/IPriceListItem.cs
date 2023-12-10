using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс для прайс-листов.
    /// Также имеет класс-расширение <see cref="IPriceListItemExtensions"/> с вынесенными общими методами.
    /// </summary>
    public interface IPriceListItem
    {
        /// <summary>
        /// Точка продаж.
        /// </summary>
        [NotNull]
        DepartmentEntity Department { get; }

        /// <summary>
        /// Продукт для продажи.
        /// </summary>
        [NotNull]
        Product Product { get; }

        /// <summary>
        /// Цена продукта.
        /// </summary>
        decimal? Price { get; }

        /// <summary>
        /// Включенность продукта в продажу.
        /// </summary>
        bool Included { get; }

        /// <summary>
        /// Словарь цен по категориям.
        /// </summary>
        [NotNull]
        Dictionary<ClientPriceCategory, decimal> PricesForCategories { get; }

        /// <summary>
        /// Включенность в продажу по категориям.
        /// </summary>
        [NotNull]
        Dictionary<ClientPriceCategory, bool> IncludeForCategories { get; }
    }
}
