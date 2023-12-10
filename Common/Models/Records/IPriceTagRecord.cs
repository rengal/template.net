namespace Resto.Data
{
    /// <summary>
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelPriceTagItem"/> из вью-модели грида в окне "Печать
    /// ценников" <see cref="DishRecord"/>
    /// </summary>
    public interface IPriceTagRecord
    {
        /// <summary>
        /// Продукт
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// Размер блюда
        /// </summary>
        ProductSize ProductSize { get; }

        /// <summary>
        /// Цена
        /// </summary>
        decimal? Price { get; }
    }
}