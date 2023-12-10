namespace Resto.Data
{
    public partial class RoundRule
    {
        #region Private fields

        /// <summary>
        /// Максимально допустимое значение точности округления.
        /// </summary>
        private const decimal PrecisionMaxValue = 100000m;

        /// <summary>
        /// Значение, больше которого должно быть значение точности округления.
        /// </summary>
        private const decimal PrecisionMinValue = 0;

        #endregion

        #region Public methods

        /// <summary>
        /// Преобразует строковое представление величины точности округления
        /// экземпляр класса <see cref="RoundRule"/>.
        /// </summary>
        /// <param name="precisionTextValue">Строковое представление величины точности округления.</param>
        /// <param name="roundMode">Способ округления.</param>
        /// <returns>
        /// Экземпляр класса <see cref="RoundRule"/> представляющий правило округления, 
        /// если данное строковое представление точности округления находится в допустимом диапазоне, иначе Null.
        /// </returns>
        public static RoundRule TryParse(RoundMode roundMode, string precisionTextValue)
        {
            decimal parsedPrecision;
            return (decimal.TryParse(precisionTextValue, out parsedPrecision) && (parsedPrecision > PrecisionMinValue)) &&
                   (parsedPrecision <= PrecisionMaxValue)
                       ? new RoundRule(roundMode, parsedPrecision)
                       : null;
        }

        #endregion
    }
}