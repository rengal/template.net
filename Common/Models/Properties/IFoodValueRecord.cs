using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Пищевая ценность.
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelNomenclatureItemFoodValue"/>
    /// </summary>
    public interface IFoodValueRecord
    {
        /// <summary>
        /// Продукт
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Жиры (исходное значение)
        /// </summary>
        decimal Fat { get; }

        /// <summary>
        /// Белки (исходное значение)
        /// </summary>
        decimal Fiber { get; }

        /// <summary>
        /// Углеводы (исходное значение)
        /// </summary>
        decimal Carbonhidrates { get; }

        /// <summary>
        /// Калорийность (исходное значение)
        /// </summary>
        decimal Calories { get; }

        /// <summary>
        /// Жиры (после приготовления)
        /// </summary>
        decimal FatOut { get; }

        /// <summary>
        /// Белки (после приготовления)
        /// </summary>
        decimal FiberOut { get; }

        /// <summary>
        /// Углеводы (после приготовления)
        /// </summary>
        decimal CarbonhidratesOut { get; }

        /// <summary>
        /// Калорийность (после приготовления)
        /// </summary>
        decimal CaloriesOut { get; }
    }
}