namespace Resto.Data
{
    public partial class AbsoluteValuePricingStrategy
    {
        /// <summary>
        /// Преобразует строковое представление величины скидки в валюте в 
        /// экземпляр класса <see cref="AbsoluteValuePricingStrategy"/>.
        /// </summary>
        /// <param name="textValue">Строковое представление величины скидки в валюте.</param>
        /// <returns>
        /// Экземпляр класса <see cref="AbsoluteValuePricingStrategy"/> представляющий стратегию 
        /// вычисления цены, если данное строковое представление валютной величины находится в допустимом диапазоне, иначе Null.
        /// </returns>
        public static AbsoluteValuePricingStrategy TryParse(string textValue)
        {
            decimal decimalValue;
            return !decimal.TryParse(textValue, out decimalValue)
                       ? null
                       : new AbsoluteValuePricingStrategy(decimalValue);
        }
    }
}