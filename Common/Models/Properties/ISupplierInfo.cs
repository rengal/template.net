using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Товар поставщика.
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelNomenclatureItemSupplierInfo"/>
    /// </summary>
    public interface ISupplierInfo
    {
        /// <summary>
        /// Поставщик
        /// </summary>
        User Supplier { get; }

        /// <summary>
        /// Продукт
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Фасовка
        /// </summary>
        Container Container { get; }
    }
}