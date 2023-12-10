using Resto.Data;
using Resto.Framework.Common.Currency;

namespace Resto.Common
{
    /// <summary>
    /// Класс, содержащий общие для всего приложения настройки GUI.
    /// </summary>
    public class GuiSettings
    {
        private const string preciseAmountsFormatString = "f4";
        private const string amountsFormatString = "f3";
        private const string moneyFormatString = "f2";
        private const string percentFormatString = "f2";

        /// <summary>
        /// Возвращает строку форматирования для отображения количественных значений (массы в кг)
        /// с точностью до десятых грамма.
        /// </summary>
        public static string PreciseAmountsFormatString
        {
            get { return preciseAmountsFormatString; }
        }

        /// <summary>
        /// Возвращает строку форматирования для отображения количественных значений (массы в кг).
        /// </summary>
        public static string AmountsFormatString
        {
            get { return amountsFormatString; }
        }

        /// <summary>
        /// Возвращает строку форматирования для отображения значений денежных сумм.
        /// В режиме дизайнера форм всегда возвращается <see cref="moneyFormatString"/>.
        /// В режиме выполнени формат формируется на основе настройки <see cref="Corporation.MoneyPrecision"/>
        /// </summary>
        public static string MoneyFormatString
        {
            get
            {
                // Проверка на режим дизайнера форм взята из класса VsHelper.
                return VsHelper.IsDesigntime
                    ? moneyFormatString
                    : "f" + CurrencyHelper.GetMoneyPrecision();
            }
        }

        /// <summary>
        /// Возвращает строку форматирования для отображения значений процентов.
        /// <remarks>
        /// Эта строка форматирования, в отличие от станртной строки форматирования "%"
        /// не приводит к умножению значения на 100 и не добавляет сам символ % на конце.
        /// </remarks>
        /// </summary>
        public static string PercentFormatString
        {
            get { return percentFormatString; }
        }
    }
}
