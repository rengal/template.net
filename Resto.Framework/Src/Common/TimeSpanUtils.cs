using System;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class TimeSpanUtils
    {
        public static TimeSpan Min(TimeSpan left, TimeSpan right)
        {
            return left < right ? left : right;
        }

        public static TimeSpan Max(TimeSpan left, TimeSpan right)
        {
            return left > right ? left : right;
        }

        /// <summary>
        /// Преобразует строковое представление интервала в его эквиваленте <see cref="TimeSpan"/>
        /// </summary>
        [Pure]
        public static TimeSpan? ParseSafe([NotNull] string input, [NotNull] CultureInfo culture)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (TimeSpan.TryParse(input, culture.DateTimeFormat, out var result))
            {
                return result;
            }

            var separatorIndex = culture.CompareInfo.IndexOf(input, culture.DateTimeFormat.TimeSeparator);
            if (separatorIndex == EnumerableExtensions.NotFound || separatorIndex + 1 == input.Length)
            {
                return null;
            }

            if (!int.TryParse(input.Substring(0, separatorIndex), NumberStyles.Integer, culture.NumberFormat, out var hours))
            {
                return null;
            }

            if (!int.TryParse(input.Substring(separatorIndex + 1), NumberStyles.Integer, culture.NumberFormat, out var minutes))
            {
                return null;
            }

            return new TimeSpan(hours, minutes, 0);
        }
    }
}
