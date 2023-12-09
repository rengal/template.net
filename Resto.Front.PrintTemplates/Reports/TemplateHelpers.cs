using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using Resto.Front.Localization.Resources;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Reports
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

        [PublicAPI, Pure]
        public static string GetLocalName(this PaymentGroup paymentGroup)
        {
            if (paymentGroup == PaymentGroup.Cash)
                return ReportLocalResources.PaymentGroupNameCash;
            if (paymentGroup == PaymentGroup.Card)
                return ReportLocalResources.Card;
            if (paymentGroup == PaymentGroup.Writeoff)
                return ReportLocalResources.PaymentGroupNameWriteoff;
            if (paymentGroup == PaymentGroup.NonCash)
                return ReportLocalResources.NonCash;

            throw new ArgumentOutOfRangeException(nameof(paymentGroup), paymentGroup, "Invalid payment group");
        }

        /// <summary>
        /// Родительный падеж, множественное число (Кого? Чего?) [рублей]
        /// </summary>
        [PublicAPI, Pure]
        public static string GetGenetiveMany()
        {
            return CurrencyHelper.GetGenetiveMany();
        }

        public static bool IsCreditPaymentType(this IPaymentType paymentType)
        {
            if (paymentType == null)
                return false;

            if (paymentType is IConfigurablePaymentType cpt)
                paymentType = cpt.BasePaymentType;

            return paymentType is ICreditPaymentType;
        }

        public static bool IsWriteoffPaymentType(this IPaymentType paymentType)
        {
            if (paymentType == null)
                return false;

            if (paymentType is IConfigurablePaymentType cpt)
                paymentType = cpt.BasePaymentType;

            return paymentType is IWriteoffPaymentType;
        }
    }
}
