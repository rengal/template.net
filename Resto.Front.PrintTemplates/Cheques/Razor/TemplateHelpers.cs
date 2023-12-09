using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;

namespace Resto.Front.PrintTemplates.Cheques.Razor
{
    public static class TemplateHelpers
    {
        /// <summary>
        /// Округление денежных сумм в соответствии с округлением валюты, заданным в BackOffice
        /// </summary>
        /// <param name="value">Денежная величина, которую нужно округлить</param>
        /// <returns>Округленное значение</returns>
        [PublicAPI, Pure]
        public static decimal RoundMoney(this decimal value)
        {
            return value.CurrencySpecificMoneyRound();
        }

        /// <summary>
        /// Округление денежных сумм в соответствии с длиной дробной части текущей валюты
        /// </summary>
        /// <param name="value">Денежная величина, которую нужно округлить</param>
        /// <returns>Округленное значение</returns>
        [PublicAPI, Pure]
        public static decimal RoundMoneyFractional(this decimal value)
        {
            return value.CurrencySpecificFractionalMoneyRound();
        }

        [PublicAPI, Pure]
        public static decimal RoundWeight(this decimal value)
        {
            return value.WeightRound();
        }
    }
}