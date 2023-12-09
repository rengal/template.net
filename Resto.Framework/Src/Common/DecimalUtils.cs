using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class DecimalUtils
    {
        private const decimal MinWeight = 0.001m;

        private static CultureInfo FormatCulture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        /// Округлить вес до третьего знака после запятой.
        /// </summary>
        /// <param name="amount">Вес</param>
        /// <returns>Округленный вес</returns>
        public static decimal WeightRound(this decimal amount)
        {
            return WeightRound(amount, 3);
        }

        /// <summary>
        /// Возвращает <c>null</c>, если <paramref name="d"/> == <c>0m</c>
        /// </summary>
        [CanBeNull]
        public static decimal? ZeroAsNull(this decimal d)
        {
            return d == 0m ? (decimal?)null : d;
        }

        public static decimal? ZeroAsNull(this decimal? d)
        {
            return d?.ZeroAsNull();
        }

        /// <summary>
        /// Округлить вес до количества знаков после запятой, равному параметру decimals.
        /// </summary>
        /// <param name="amount">Вес</param>
        /// <param name="decimals">Число знаков после запятой</param>
        /// <returns>Округленный вес</returns>
        public static decimal WeightRound(this decimal amount, int decimals)
        {
            return Math.Round(amount, decimals, MidpointRounding.AwayFromZero);
        }

        public static decimal WeightCeiling(this decimal amount)
        {
            var rounded = WeightRound(amount);
            return rounded >= amount ? rounded : rounded + MinWeight;
        }

        public static decimal Pow(this decimal value, int power)
        {
            if (power == 0)
                return 1m;
            var absPower = Math.Abs(power);
            var res = 1m;
            for (var i = 1; i <= absPower; ++i)
                res *= value;
            return power == absPower ? res : 1m / res;
        }

        /// <summary>
        /// Разбить вес (кол-во блюда) <paramref name="value"/> на несколько частей <paramref name="partsCount"/>.
        /// </summary>
        /// <remarks>Для разбиения денежной суммы на несколько частей используйте <see cref="MoneyUtils.SplitWithMoneyRound"/>.</remarks>
        [NotNull, Pure]
        public static decimal[] SplitWeight(this decimal value, int partsCount)
        {
            if (partsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(partsCount), partsCount, "Parts count must be greater than or equal to 1.");
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be greater than or equal to 0.");
            if (value < MinWeight * partsCount)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value is too small.");

            var val = value.WeightRound();

            if (partsCount == 1)
                return new[] { val };

            var part = (val / partsCount).WeightRound();
            var remainder = val - part * partsCount;
            var count = (int)Math.Abs(remainder / MinWeight);

            var result = new decimal[partsCount];

            for (var i = 0; i < partsCount - count; i++)
                result[i] = part;

            if (count == 0)
                return result;

            for (var i = partsCount - count; i < partsCount; i++)
                result[i] = part + remainder / count;

            return result;
        }

        //Временный фикс для Петролеума
        public static decimal[] Split(decimal amount, decimal multiplicand, decimal divisor)
        {
            return SplitWeight(amount, multiplicand, divisor);
        }

        public static decimal[] SplitWeight(decimal amount, decimal multiplicand, decimal divisor)
        {
            var results = new decimal[2];
            results[0] = WeightRound(amount * multiplicand / divisor);
            results[1] = amount - results[0];
            return results;
        }

        public static string GetFormattedIntegerAmountPart(decimal amount)
        {
            return GetFormattedIntegerAmountPart(amount, NumberFormatInfo.CurrentInfo);
        }

        public static string GetFormattedIntegerAmountPart(decimal amount, IFormatProvider formatProvider)
        {
            var intAmount = (int)amount;
            return intAmount.ToString(formatProvider);
        }

        public static string GetFormattedFractionalAmountPart(decimal amount)
        {
            return GetFormattedFractionalAmountPart(amount, FormatCulture);
        }

        public static string GetFormattedFractionalAmountPart(decimal amount, CultureInfo culture)
        {
            amount = amount.WeightRound();
            var fract = amount - Math.Floor(amount);
            if (fract > 0)
            {
                var fractPartLength = 0;
                while (fract != Math.Floor(fract))
                {
                    fract *= 10;
                    fractPartLength++;
                }
                var fractAmount = (int)fract;

                // Создаем format-строку вида {0}{1:Dn}, где n - длина дробной части
                var formatString = string.Format("{{0}}{{1:D{0}}}", fractPartLength);
                return string.Format(formatString, culture.NumberFormat.NumberDecimalSeparator, fractAmount);
            }
            return string.Empty;
        }

        /// <summary>
        /// Проверяет, является представленное число целым
        /// </summary>
        public static bool IsIntegral(this decimal value)
        {
            return Integral(value) == 0;
        }

        public static decimal Integral(this decimal value)
        {
            return value - Math.Truncate(value);
        }

        public static bool TryParseStr(string str, out decimal result, bool useCurrencySeparator = false)
        {
            result = 0m;
            if (str.IsNullOrEmpty())
            {
                return false;
            }

            var separator = useCurrencySeparator
                ? CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator
                : CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            return decimal.TryParse(str.Replace(",", separator), out result) ||
                decimal.TryParse(str.Replace(".", separator), out result);
        }

        public static decimal CalculateVatCoefficient(this decimal vatValue)
        {
            return 1m + vatValue / 100m;
        }

        public static decimal? SafeDivide(this decimal? divident, decimal? divider, bool zeroAsNull = false)
        {
            var quotient = divider == 0m ? 0m : divident / divider;
            return zeroAsNull ? (quotient == 0m ? null : quotient) : quotient;
        }

        /// <summary>
        /// Корректирует суммы массива sums так, чтобы sums.Sum() оказалась равной expectedSum и округлена в соответствии с длиной дробной части, 
        /// и каждый из элементов был округлен в соответствии с длиной дробной части.
        /// </summary>
        /// <remarks>
        /// По умолчанию округление до 2х знаков после запятой.
        /// </remarks>
        public static IReadOnlyList<decimal> DistributeUniformly(this IReadOnlyList<decimal> sums, decimal expectedSum, int decimals = 2)
        {
            return sums.DistributeUniformly(expectedSum, value => Math.Round(value, decimals));
        }

        /// <summary>
        /// Корректирует суммы массива sums так, чтобы sums.Sum() оказалась равной expectedSum и округлена в соответствии с длиной дробной части, 
        /// и каждый из элементов был округлен в соответствии с длиной дробной части.
        /// </summary>
        public static IReadOnlyList<decimal> DistributeUniformly(this IReadOnlyList<decimal> sums, decimal expectedSum, Func<decimal, decimal> round)
        {
            if (sums.Count == 0)
            {
                return sums;
            }

            var currentSum = sums.Sum();
            if (currentSum == decimal.Zero)
            {
                return sums;
            }

            var rate = expectedSum / currentSum;
            var result = new decimal[sums.Count];
            var initialPartialSum = decimal.Zero;
            var previousFixedPartialSum = decimal.Zero;
            for (var i = 0; i < sums.Count - 1; i++)
            {
                initialPartialSum += sums[i];
                var fixedPartialSum = round(initialPartialSum * rate);
                result[i] = round(fixedPartialSum - previousFixedPartialSum);
                previousFixedPartialSum = fixedPartialSum;
            }

            result[sums.Count - 1] = round(expectedSum - previousFixedPartialSum);
            return result;
        }
    }
}
