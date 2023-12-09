using System;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс для сравнения строк, учитывающий вхождения чисел: "z10" будет идти после "z2", хотя при
    /// алфавитной сортировке наоборот.
    /// </summary>
    /// <remarks>
    /// Код позаимствован чуть менее, чем полностью, из http://dotnetperls.com/alphanumeric-sorting.
    /// </remarks>
    public static class AlphanumStringComparer
    {
        public static int CompareStrings([NotNull] string x, [NotNull] string y, [NotNull] CultureInfo culture)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            if (string.Compare(x, y, false, culture) == 0)
                return 0;

            var len1 = x.Length;
            var len2 = y.Length;
            var marker1 = 0;
            var marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                var ch1 = x[marker1];
                var ch2 = y[marker2];

                // Some buffers we can build up characters in for each chunk.
                var space1 = new char[len1];
                var loc1 = 0;
                var space2 = new char[len2];
                var loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 >= len1)
                        break;

                    ch1 = x[marker1];
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 >= len2)
                        break;

                    ch2 = y[marker2];
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                var str1 = new string(space1);
                var str2 = new string(space2);

                var result = long.TryParse(str1, out var thisNumericChunk) && long.TryParse(str2, out var thatNumericChunk)
                    ? thisNumericChunk.CompareTo(thatNumericChunk)
                    : string.Compare(str1, str2, false, culture);

                if (result != 0)
                    return result;
            }
            return len1 - len2;
        }
    }
}
