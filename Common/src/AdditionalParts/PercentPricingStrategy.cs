namespace Resto.Data
{
    public partial class PercentPricingStrategy
    {
        /// <summary>
        /// Минимально допустимое значение скидки в процентах.
        /// </summary>
        public const decimal DefaultDiscountPercentMin = -100m;

        /// <summary>
        /// Максимально допустимое значение скидки в процентах.
        /// </summary>
        public const decimal DefaultDiscountPercentMax = 1000000m;

        /// <summary>
        /// Преобразует строковое представление процента скидки в экземпляр класса <see cref="PercentPricingStrategy"/>,
        /// </summary>
        /// <param name="textValue">Строковое представление процента скидки.</param>
        /// <returns>
        /// Экземпляр класса <see cref="PercentPricingStrategy"/> представляющий стратегию вычисления цены, 
        /// если данное строковое представление процента скидки находится в допустимом диапазоне, иначе <c>null</c>.
        /// </returns>
        public static PercentPricingStrategy TryParse(string textValue)
        {
            decimal decimalValue;
            if (!decimal.TryParse(textValue, out decimalValue))
            {
                return null;
            }

            if ((decimalValue < DefaultDiscountPercentMin) || (decimalValue > DefaultDiscountPercentMax))
            {
                return null;
            }
            return new PercentPricingStrategy(decimalValue);
        }
    }
}