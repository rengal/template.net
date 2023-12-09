using System;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Currency;
using Resto.Front.PrintTemplates.Reports.TemplateModels;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Reports
{
    public abstract partial class TemplateBase : RazorEngine.Templating.TemplateBase<ITemplateModel>
    {
        [PublicAPI, Pure]
        protected static string FormatAmount(decimal amount)
        {
            return PrintUtils.FormatAmount(amount);
        }

        [PublicAPI, Pure]
        protected static string FormatPriceFractional(decimal price)
        {
            return PrintUtils.FormatPriceFractional(price);
        }

        [PublicAPI, Pure]
        protected static string FormatPrice(decimal price)
        {
            return PrintUtils.FormatPrice(price);
        }

        [PublicAPI, Pure]
        protected static string FormatAmountAndPrice(decimal amount, decimal price)
        {
            return PrintUtils.FormatAmountAndPrice(amount, price);
        }

        [PublicAPI, Pure]
        protected static string FormatPercent(decimal percent)
        {
            return PrintUtils.FormatPercent(percent);
        }

        [PublicAPI, Pure]
        protected static string FormatAveragePercent(decimal percent)
        {
            return PrintUtils.FormatAveragePercent(percent);
        }

        [PublicAPI, Pure]
        protected static string FormatAverage(decimal amount)
        {
            return PrintUtils.FormatAverage(amount);
        }

        [PublicAPI, Pure]
        protected static string FormatLongDateTime(DateTime dateTime)
        {
            return PrintUtils.FormatLongDateTime(dateTime);
        }

        [PublicAPI, Pure]
        protected static string FormatDate(DateTime dateTime)
        {
            return PrintUtils.FormatDate(dateTime);
        }

        [PublicAPI, Pure]
        protected static string FormatTime(DateTime dateTime)
        {
            return PrintUtils.FormatTime(dateTime);
        }

        [PublicAPI, Pure]
        protected static string FormatTimeSpan(TimeSpan ts, bool useSeconds)
        {
            return PrintUtils.FormatTimeSpan(ts, useSeconds);
        }

        [PublicAPI, Pure]
        protected static decimal GetSumInAdditionalCurrency([NotNull] IAdditionalCurrency currency, decimal additionalCurrencyRate, decimal currentCurrencySum)
        {
            return new Currency(currency.IsoName, null, null, null).GetSumInAdditionalCurrency(additionalCurrencyRate, currentCurrencySum);
        }
    }
}