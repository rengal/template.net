using System;
using System.Globalization;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using Resto.Framework.Common.Print.Tags.Xml;

namespace Resto.Front.PrintTemplates
{
    /// <summary>
    /// Класс-хелпер, содержащий впомогательные функции для печати
    /// </summary>
    public static class PrintUtils
    {
        // количество знаков после запятой для количества
        private const int AmountDecimalsCount = 3;
        private static readonly string AmountFormatString = "f" + AmountDecimalsCount;
        // количество знаков после запятой для цены
        // количество знаков после запятой для процентов в скидках и надбавках
        private const int PercentDecimalsCount = 2;
        private static readonly string PercentFormatString = string.Format("{{0:f{0}}}%", PercentDecimalsCount);

        private static CultureInfo FormatCulture => CultureInfo.CurrentCulture;

        public static Line NewDoubleLine()
        {
            return new Line("=");
        }

        public static Cell NewLineRow()
        {
            return new LineCell("-");
        }

        public static Column NewCodeColumn()
        {
            return new Column(new AutoWidthAttr());
        }

        public static Cell NewCodeCell(string code)
        {
            return new TextCell(code);
        }

        public static Column NewNameColumn()
        {
            return new Column(FormatterAttr.Split);
        }

        public static Column NewPriceColumn()
        {
            return NewNumericColumn();
        }

        public static Cell NewPriceCell(decimal price)
        {
            return new TextCell(FormatPrice(price));
        }

        public static Column NewAmountColumn()
        {
            return NewNumericColumn();
        }

        public static Column NewNumericColumn()
        {
            return new Column(AlignAttr.Right, new AutoWidthAttr());
        }

        public static Cell NewAmountCell(decimal amount)
        {
            return new TextCell(FormatAmount(amount));
        }

        public static Cell NewAmountAndPriceCell(decimal amount, decimal price)
        {
            return new TextCell(FormatAmountAndPrice(amount, price));
        }

        public static string FormatAmountAndPrice(decimal amount, decimal price)
        {
            return string.Format("{0}x{1}", FormatAmount(amount), FormatPriceMin(price));
        }

        public static string FormatAmount(decimal amount)
        {
            return amount.IsIntegral()
                ? amount.ToString("f0", FormatCulture)
                : amount.ToString(AmountFormatString, FormatCulture);
        }

        public static string FormatFoodValueItem(decimal foodValueItem)
        {
            return foodValueItem.ToString("0.#", FormatCulture);
        }

        public static string FormatAverage(decimal amount)
        {
            return amount.ToString("0.##");
        }

        /// <summary>
        /// форматирует цену как "123,45"
        /// </summary>
        public static string FormatPrice(decimal price)
        {
            return price.MoneyToStringForPrint();
        }

        public static string FormatPriceFractional(decimal price)
        {
            return price.FractionalMoneyToStringForPrint();
        }

        /// <summary>
        /// форматирует цену как "123,45", или "123" если значение целое
        /// </summary>
        public static string FormatPriceMin(decimal price)
        {
            return price.CurrencySpecificMoneyRound().IsIntegral() ?
                price.ToString("f0", FormatCulture) : FormatPrice(price);
        }

        public static string FormatPercent(decimal percent)
        {
            return decimal.Round(percent, PercentDecimalsCount).IsIntegral()
                ? string.Format(FormatCulture, "{0:f0}%", percent)
                : string.Format(FormatCulture, PercentFormatString, percent);
        }

        public static string FormatAveragePercent(decimal percent)
        {
            return string.Format(FormatCulture, "{0:f1}%", percent);
        }

        public static string FormatTime(DateTime time)
        {
            return time.ToString("HH:mm");
        }

        public static string FormatLongTime(DateTime time)
        {
            return time.ToString("HH:mm:ss");
        }

        public static string FormatLongDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm");
        }

        public static string FormatFullDateTime(DateTime dateTime)
        {
            return dateTime.ToString("d MMM HH:mm");
        }

        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy");
        }

        public static string FormatDateTimeCustom(DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        public static string FormatTimeSpan(TimeSpan ts, bool displaySeconds)
        {
            if (displaySeconds)
                return string.Format("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);

            if (ts.Seconds >= 30)
                ts = ts.Add(new TimeSpan(0, 0, 1, 0));

            return string.Format("{0:00}:{1:00}", (int)ts.TotalHours, ts.Minutes);
        }
    }
}