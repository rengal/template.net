using System;
using System.Collections.Generic;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Cheques.Razor
{
    public abstract partial class TemplateBase<T> : RazorEngine.Templating.TemplateBase<T>
    {
        #region Format Methods
        [PublicAPI, Pure]
        protected static string FormatAmount(decimal amount)
        {
            return PrintUtils.FormatAmount(amount);
        }

        [PublicAPI, Pure]
        protected static string FormatFoodValueItem(decimal foodValueItem)
        {
            return PrintUtils.FormatFoodValueItem(foodValueItem);
        }

        [PublicAPI, Pure]
        protected static string FormatMoneyFractional(decimal money)
        {
            return PrintUtils.FormatPriceFractional(money);
        }

        [PublicAPI, Pure]
        protected static string FormatMoney(decimal money)
        {
            return PrintUtils.FormatPrice(money);
        }

        [PublicAPI, Pure]
        protected static string FormatMoney(decimal money, [NotNull] string isoName)
        {
            return money.MoneyToStringForPrint(CurrencyHelper.GetCurrencyFractionalPartLength(isoName), 0m);
        }

        [PublicAPI, Pure]
        protected static string FormatMoneyMin(decimal money)
        {
            return PrintUtils.FormatPriceMin(money);
        }

        [PublicAPI, Pure]
        protected static string FormatMoneyInWords(decimal money)
        {
            return money.CreateCurrencyStr();
        }

        [PublicAPI, Pure]
        protected static string FormatPercent(decimal value)
        {
            return PrintUtils.FormatPercent(value);
        }

        [PublicAPI, Pure]
        protected static string FormatTime(DateTime time)
        {
            return PrintUtils.FormatTime(time);
        }

        [PublicAPI, Pure]
        protected static string FormatLongTime(DateTime time)
        {
            return PrintUtils.FormatLongTime(time);
        }

        [PublicAPI, Pure]
        protected static string FormatLongDateTime(DateTime dateTime)
        {
            return PrintUtils.FormatLongDateTime(dateTime);
        }

        [PublicAPI, Pure]
        protected static string FormatFullDateTime(DateTime dateTime)
        {
            return PrintUtils.FormatFullDateTime(dateTime);
        }

        [PublicAPI, Pure]
        protected static string FormatDate(DateTime date)
        {
            return PrintUtils.FormatDate(date);
        }

        [PublicAPI, Pure]
        protected static string FormatDateTimeCustom(DateTime dateTime, string format)
        {
            return PrintUtils.FormatDateTimeCustom(dateTime, format);
        }

        [PublicAPI, Pure]
        protected static string FormatTimeSpan(TimeSpan timeSpan, bool displaySeconds)
        {
            return PrintUtils.FormatTimeSpan(timeSpan, displaySeconds);
        }
        #endregion

        #region Helpers
        [PublicAPI, Pure]
        protected static decimal CalculatePercent(decimal fullValue, decimal partValue)
        {
            return fullValue == 0m ? 0m : Math.Round(partValue / fullValue * 100m, 2, MidpointRounding.AwayFromZero);
        }

        [PublicAPI, Pure]
        protected static decimal GetVatSum(decimal fullSum, decimal vatPercent)
        {
            return MoneyUtils.GetVatSum(fullSum, vatPercent);
        }

        [PublicAPI, Pure]
        protected static decimal GetSumInAdditionalCurrency([NotNull] IAdditionalCurrency currency, decimal additionalCurrencyRate, decimal currentCurrencySum)
        {
            return new Currency(currency.IsoName, null, null, null).GetSumInAdditionalCurrency(additionalCurrencyRate, currentCurrencySum);
        }
        #endregion

        #region Equality Comparer From Equals
        private sealed class EqualsEqualityComparer<TObject> : IEqualityComparer<TObject>
        {
            private readonly Func<TObject, TObject, bool> equals;
            private readonly Func<TObject, int> getHashCode;

            internal EqualsEqualityComparer([NotNull] Func<TObject, TObject, bool> equals, [NotNull] Func<TObject, int> getHashCode)
            {
                Debug.Assert(equals != null);
                Debug.Assert(getHashCode != null);

                this.equals = equals;
                this.getHashCode = getHashCode;
            }

            public bool Equals(TObject x, TObject y)
            {
                return equals(x, y);
            }

            public int GetHashCode(TObject obj)
            {
                return getHashCode(obj);
            }
        }

        [PublicAPI, Pure]
        protected static IEqualityComparer<TObject> CreateComparer<TObject>([NotNull] Func<TObject, TObject, bool> equals)
        {
            if (equals == null)
                throw new ArgumentNullException(nameof(equals));

            return new EqualsEqualityComparer<TObject>(equals, _ => 0);
        }

        [PublicAPI, Pure]
        protected static IEqualityComparer<TObject> CreateComparer<TObject>([NotNull] Func<TObject, TObject, bool> equals, [NotNull] Func<TObject, int> getHashCode)
        {
            if (equals == null)
                throw new ArgumentNullException(nameof(equals));
            if (getHashCode == null)
                throw new ArgumentNullException(nameof(getHashCode));

            return new EqualsEqualityComparer<TObject>(equals, getHashCode);
        }
        #endregion
    }
}