using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Currency;

namespace Resto.Framework.Common
{
    public static class MoneyUtils
    {
        // Существует копия константы для плагинов Resto.Front.Api.Data.Payments.CurrencyHelper.MinDecimals
        private const int MinDecimals = -28;

        private static CultureInfo FormatCulture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        /// <summary>
        /// Округлить денежную сумму до второго знака после запятой.
        /// </summary>
        /// <param name="sum">Сумма</param>
        /// <returns>Округленная сумма</returns>
        [Obsolete("Для округления денежных сумм в зависимости от текущей валюты нужно использовать CurrencyHelper.CurrencySpecificMoneyRound")]
        public static decimal MoneyRound(this decimal sum)
        {
            return Math.Round(sum, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Округлить денежную сумму с точностью <paramref name="decimals"/>
        /// и с минимальным номиналом ден. знака <paramref name="minimumDenomination"/>.
        /// </summary>
        /// <param name="sum">Сумма.</param>
        /// <param name="decimals">
        /// Точность округления. Положительное значение — число знаков после запятой, отрицательное — число знаков до запятой.
        /// </param>
        /// <param name="minimumDenomination">
        /// Минимальный номинал денежного знака в валюте. Если равен 0, то не учитывается.
        /// </param>
        /// <param name="midpointRoundingTowardsZero">Нужно ли округлять до ближайшего меньшего по модулю числа, когда число находится посредине между двумя другими числами.</param>
        /// <returns>Округленная сумма</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> меньше -28 или больше 28 или
        /// <paramref name="minimumDenomination"/> меньше 0.
        /// </exception>
        /// Существует копия метода для плагинов Resto.Front.Api.Data.Payments.CurrencyHelper.MoneyRound(this decimal sum, decimal minDenomination)
        public static decimal MoneyRound(this decimal sum, int decimals, decimal minimumDenomination, bool midpointRoundingTowardsZero = false)
        {
            if (minimumDenomination < 0m)
                throw new ArgumentOutOfRangeException(nameof(minimumDenomination), minimumDenomination, "minimumDenomination must be nonnegative");

            var rounded = decimals >= 0 && !midpointRoundingTowardsZero
                ? Math.Round(sum, decimals, MidpointRounding.AwayFromZero)
                : Round(sum, decimals, midpointRoundingTowardsZero);

            return minimumDenomination == 0m ? rounded : Round(rounded, minimumDenomination, midpointRoundingTowardsZero);
        }

        // Существует копия метода для плагинов Resto.Front.Api.Data.Payments.CurrencyHelper.Round(decimal value, int decimals)
        private static decimal Round(decimal value, int decimals, bool midpointRoundingTowardsZero)
        {
            if (decimals < MinDecimals)
                throw new ArgumentOutOfRangeException(nameof(decimals), decimals, "decimals too small");

            return Round(value, (decimal)Math.Pow(10, -decimals), midpointRoundingTowardsZero);
        }

        // Существует копия метода для плагинов Resto.Front.Api.Data.Payments.CurrencyHelper.Round(decimal decimal, int minimumDenomination)
        private static decimal Round(decimal value, decimal minimumDenomination, bool midpointRoundingTowardsZero)
        {
            if (value < 0m)
                return -Round(-value, minimumDenomination, midpointRoundingTowardsZero);

            Debug.Assert(value >= 0m);
            var remainder = value % minimumDenomination;

            var halfOfMinimumDenomination = minimumDenomination / 2m;
            if (halfOfMinimumDenomination > remainder || midpointRoundingTowardsZero && halfOfMinimumDenomination == remainder)
            {
                return value - remainder;
            }

            return value - remainder + minimumDenomination;
        }

        public static decimal MoneyFloor(this decimal sum, int decimals, decimal minimumDenomination)
        {
            if (minimumDenomination < 0m)
                throw new ArgumentOutOfRangeException(nameof(minimumDenomination), minimumDenomination, "minimumDenomination must be nonnegative");

            var rounded = decimals == 0
                ? Math.Floor(sum)
                : Floor(sum, decimals);

            return minimumDenomination == 0m ? rounded : Floor(rounded, minimumDenomination);
        }

        private static decimal Floor(decimal value, int decimals)
        {
            if (decimals < MinDecimals)
                throw new ArgumentOutOfRangeException(nameof(decimals), decimals, "decimals too small");

            return Floor(value, (decimal)Math.Pow(10, -decimals));
        }

        private static decimal Floor(decimal value, decimal minimumDenomination)
        {
            if (value < 0m)
                return -Ceiling(-value, minimumDenomination);

            Debug.Assert(value >= 0m);
            var remainder = value % minimumDenomination;
            if (remainder == 0m)
                return value;

            return value - remainder;
        }

        public static decimal MoneyCeiling(this decimal sum, int decimals, decimal minimumDenomination)
        {
            if (minimumDenomination < 0m)
                throw new ArgumentOutOfRangeException(nameof(minimumDenomination), minimumDenomination, "minimumDenomination must be nonnegative");

            var rounded = decimals == 0
                ? Math.Ceiling(sum)
                : Ceiling(sum, decimals);

            return minimumDenomination == 0m ? rounded : Ceiling(rounded, minimumDenomination);
        }

        private static decimal Ceiling(decimal value, int decimals)
        {
            if (decimals < MinDecimals)
                throw new ArgumentOutOfRangeException(nameof(decimals), decimals, "decimals too small");

            return Ceiling(value, (decimal)Math.Pow(10, -decimals));
        }

        private static decimal Ceiling(decimal value, decimal minimumDenomination)
        {
            if (value < 0m)
                return -Floor(-value, minimumDenomination);

            Debug.Assert(value >= 0m);
            var remainder = value % minimumDenomination;
            if (remainder == 0m)
                return value;

            return value - remainder + minimumDenomination;
        }

        /// <summary>
        /// Разбить денежную сумму <paramref name="value"/> на несколько частей <paramref name="partsCount"/>.
        /// </summary>
        /// <remarks>Для разбиения веса (кол-ва блюда) на несколько частей используйте <see cref="DecimalUtils.SplitWeight(decimal,int)"/>.</remarks>
        [NotNull, Pure]
        public static decimal[] SplitWithMoneyRound(this decimal value, int partsCount)
        {
            if (partsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(partsCount), partsCount, "Parts count must be greater than or equal to 1.");
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be greater than or equal to 0.");
            if (value != value.CurrencySpecificMoneyRound())
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value should be rounded according to the currency settings.");

            if (partsCount == 1)
                return new[] { value };

            var part = (value / partsCount).CurrencySpecificMoneyRound();
            var remainder = value - part * partsCount;

            var count = 0;
            if (remainder != 0m)
                count = CurrencyHelper.CurrentCurrencyMinimumDenomination == 0m ? 1 : (int)Math.Abs(remainder / CurrencyHelper.CurrentCurrencyMinimumDenomination);

            var result = new decimal[partsCount];

            for (var i = 0; i < partsCount - count; i++)
                result[i] = part;

            if (count == 0)
                return result;

            for (var i = partsCount - count; i < partsCount; i++)
                result[i] = part + remainder / count;

            return result;
        }

        [Obsolete("Для распределения денежных сумм в зависимости от текущей валюты нужно использовать CurrencyHelper.CurrencySpecificDistributeUniformly")]
        public static IReadOnlyList<decimal> DistributeUniformly(IReadOnlyList<decimal> sums, decimal expectedSum)
        {
            return DistributeUniformly(sums, expectedSum, 2, 0m);
        }

        public static IReadOnlyList<decimal> DistributeUniformly(IReadOnlyList<decimal> sums, decimal expectedSum, int decimals, decimal minimumDenomination)
        {
            return sums.DistributeUniformly(expectedSum, value => MoneyRound(value, decimals, minimumDenomination));
        }

        /// <summary>
        /// Вычисление процента <paramref name="percent"/> от суммы <paramref name="sum"/>
        /// </summary>
        /// 
        /// <param name="sum">
        /// Сумма, процент которой вычисляется
        /// </param>
        /// <param name="percent">
        /// Процент, который вычисляется от суммы
        /// </param>
        /// 
        /// <returns>
        /// Возвращает процент от суммы
        /// </returns>
        /// 
        /// <remarks>
        /// Получившаяся сумма не округляется!
        /// </remarks>
        public static decimal GetPercentOfSum(this decimal sum, decimal percent)
        {
            return sum * percent / 100.0m;
        }

        /// <summary>
        /// Вычисляет сумму без НДС на основании полной суммы и процента НДС.
        /// Вычисление производится с округлением до сотых долей. Округление половин копеек происходит в бОльшую сторону (и именно так и должно округляться!!!).
        /// </summary>
        /// <param name="fullSum">Полная сумма.</param>
        /// <param name="vatPercent">Процент НДС.</param>
        /// <returns>Сумма без НДС.</returns>
        public static decimal GetSumWithoutVat(decimal fullSum, decimal vatPercent)
        {
            if (vatPercent == 0 || fullSum == 0)
                return fullSum;

            return (fullSum / (1m + vatPercent / 100)).CurrencySpecificFractionalMoneyRound();
        }

        /// <summary>
        /// Вычисляет сумму НДС на основании полной суммы и процента НДС.
        /// Вычисление производится с округлением до сотых долей. Округление половин копеек происходит в меньшую сторону (и именно так и должно округляться!!!).
        /// </summary>
        /// <param name="fullSum">Полная сумма.</param>
        /// <param name="vatPercent">Процент НДС.</param>
        /// <returns>Сумма НДС.</returns>
        public static decimal GetVatSum(decimal fullSum, decimal vatPercent)
        {
            return fullSum - GetSumWithoutVat(fullSum, vatPercent);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="value"/> в строку
        /// с точностью <paramref name="decimals"/>.
        /// Преобразование выполняется с использованием региональных настроек ОС для денежных величин.
        /// </summary>
        /// <param name="value">Денежная величина, которую нужно преобразовать в строку</param>
        /// <param name="decimals">Точность округления. Положительное значение — число знаков после запятой, отрицательное — число знаков до запятой.</param>
        /// <param name="minimumDenomination">Минимальный номинал денежного знака в валюте. Если равен 0, то не учитывается.</param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> меньше -28 или больше 28
        /// </exception>
        public static string MoneyToString(this decimal value, int decimals, decimal minimumDenomination)
        {
            return value.MoneyToStringWithCulture(decimals, minimumDenomination, null);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="value"/> в строку
        /// с точностью <paramref name="decimals"/>.
        /// Преобразование выполняется с использованием региональных настроек ОС для денежных величин.
        /// </summary>
        /// <param name="value">Денежная величина, которую нужно преобразовать в строку</param>
        /// <param name="decimals">Точность округления. Положительное значение — число знаков после запятой, отрицательное — число знаков до запятой.</param>
        /// <param name="minimumDenomination">Минимальный номинал денежного знака в валюте. Если равен 0, то не учитывается.</param>
        /// <param name="cultureInfo">Культура, если не задана, берется значение CurrentCulture.</param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> меньше -28 или больше 28
        /// </exception>
        public static string MoneyToStringWithCulture(this decimal value, int decimals, decimal minimumDenomination, CultureInfo cultureInfo)
        {
            return value.MoneyRound(decimals, minimumDenomination).ToString("n" + Math.Max(decimals, 0), cultureInfo ?? FormatCulture);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="value"/> в строку
        /// с точностью <paramref name="decimals"/> и
        /// использованием символа валюты <paramref name="currencyString"/>.
        /// </summary>
        /// <param name="value">Денежная величина, котороую нужно преобразовать в строку</param>
        /// <param name="decimals">Точность округления. Положительное значение — число знаков после запятой, отрицательное — число знаков до запятой.</param>
        /// <param name="minimumDenomination">Минимальный номинал денежного знака в валюте. Если равен 0, то не учитывается.</param>
        /// <param name="currencyString">Символьное обозначение валюты, непустая строка</param>
        /// <param name="currencyStringAfterSum">
        /// Где выводить символьное обозначение валюты <paramref name="currencyString"/>:
        /// <c>true</c> — после суммы (например, рубли), <c>false</c> — до суммы (например, доллары США).
        /// </param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="currencyString"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="currencyString"/><c>.Trim().Length == 0</c> или <paramref name="decimals"/> меньше -28 или больше 28
        /// </exception>
        public static string MoneyToString(this decimal value, int decimals, decimal minimumDenomination, string currencyString, bool currencyStringAfterSum)
        {
            return value.MoneyToStringWithCulture(decimals, minimumDenomination, currencyString, currencyStringAfterSum, null);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="value"/> в строку
        /// с точностью <paramref name="decimals"/> и
        /// использованием символа валюты <paramref name="currencyString"/>.
        /// </summary>
        /// <param name="value">Денежная величина, котороую нужно преобразовать в строку</param>
        /// <param name="decimals">Точность округления. Положительное значение — число знаков после запятой, отрицательное — число знаков до запятой.</param>
        /// <param name="minimumDenomination">Минимальный номинал денежного знака в валюте. Если равен 0, то не учитывается.</param>
        /// <param name="currencyString">Символьное обозначение валюты, непустая строка</param>
        /// <param name="currencyStringAfterSum">
        /// Где выводить символьное обозначение валюты <paramref name="currencyString"/>:
        /// <c>true</c> — после суммы (например, рубли), <c>false</c> — до суммы (например, доллары США).
        /// </param>
        /// <param name="cultureInfo">Культура, если не задана, берется значение CurrentCulture.</param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="currencyString"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="currencyString"/><c>.Trim().Length == 0</c> или <paramref name="decimals"/> меньше -28 или больше 28
        /// </exception>
        public static string MoneyToStringWithCulture(this decimal value, int decimals, decimal minimumDenomination, string currencyString, bool currencyStringAfterSum, CultureInfo cultureInfo)
        {
            if (currencyString == null)
                throw new ArgumentNullException(nameof(currencyString));
            if (currencyString.Trim().Length == 0)
                throw new ArgumentOutOfRangeException(nameof(currencyString), currencyString, "Currency string must be nonempty");

            var formatString = currencyStringAfterSum
                                   ? "{0:n" + Math.Max(decimals, 0) + "}\u00A0{1}"
                                   : "{1}\u00A0{0:n" + Math.Max(decimals, 0) + "}";

            return String.Format(cultureInfo ?? FormatCulture, formatString, value.MoneyRound(decimals, minimumDenomination), currencyString.Trim());

            // \u00A0 — неразрывный пробел
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="value"/> в строку для печати (без разделителей групп символов)
        /// с точностью <paramref name="decimals"/>.
        /// Преобразование выполняется с использованием региональных настроек ОС для денежных величин.
        /// </summary>
        /// <param name="value">Денежная величина, котороую нужно преобразовать в строку</param>
        /// <param name="decimals">Точность округления. Положительное значение — число знаков после запятой, отрицательное — число знаков до запятой.</param>
        /// <param name="minimumDenomination">Минимальный номинал денежного знака в валюте. Если равен 0, то не учитывается.</param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="value"/> для печати (без разделителей групп символов)
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> меньше -28 или больше 28
        /// </exception>
        public static string MoneyToStringForPrint(this decimal value, int decimals, decimal minimumDenomination)
        {
            return value.MoneyRound(decimals, minimumDenomination).ToString("n" + Math.Max(decimals, 0), FormatCulture);
        }
    }
}