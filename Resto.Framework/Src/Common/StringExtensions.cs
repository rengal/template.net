using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class StringExtensions
    {
        #region Fields
        public const string NullRepresentation = "null";
        private static readonly Regex WordMathcher = new Regex(@"(\w+)", RegexOptions.Compiled);
        #endregion

        #region Methods
        [Pure]
        [ContractAnnotation("null => true", true)]
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        [Pure]
        public static bool Match([NotNull] this string str, [NotNull] string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            return str.Match(new Regex(pattern));
        }

        [Pure]
        public static bool Match([NotNull] this string str, [NotNull] Regex regex)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            var match = regex.Match(str);

            return match.Success &&
                   match.Index == 0 &&
                   match.Length == str.Length;
        }

        [Pure]
        public static string Capitalize([NotNull] this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (str.Length == 0)
                return string.Empty;

            return char.ToUpper(str[0]) + str.Substring(1);
        }

        [Pure]
        public static string CapitalizeWords(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            return WordMathcher.Replace(str, m => m.Groups[1].Value.Capitalize());
        }

        /// <summary>
        /// Получить строку указанной длины из исходной с выравниванием по концу строки.
        /// </summary>
        /// <remarks>
        /// Если базовая строка короче требуемой длины, то слева добавляются символы ' '. Если базовая строка длиннее,
        /// то берется подстрока необходимой длинны справа.
        /// </remarks>
        /// <param name="str">Базовая строка.</param>
        /// <param name="length">Желаемая длинна строки.</param>
        /// <returns>Полученная строка.</returns>
        [Pure]
        public static string FitEnd(this string str, int length)
        {
            return FitEnd(str, length, ' ');
        }

        /// <summary>
        /// Получить строку указанной длины из исходной с выравниванием по концу строки.
        /// </summary>
        /// <remarks>
        /// Если базовая строка короче требуемой длины, то слева добавляются символы. Если базовая строка длиннее,
        /// то берется подстрока необходимой длины справа.
        /// </remarks>
        /// <param name="str">Базовая строка.</param>
        /// <param name="length">Желаемая длинна строки.</param>
        /// <param name="symbol">Символ для подстановки недостающих символов.</param>
        /// <returns>Полученная строка.</returns>
        [Pure]
        public static string FitEnd(this string str, int length, char symbol)
        {
            return str.Length < length ? str.PadLeft(length, symbol) : str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// Добавляет пробелы по обе стороны строки, пока она не достигнет желаемой длины
        /// </summary>
        /// <example>
        /// <code>
        /// PadBoth("abc", 5) => " abc ";
        /// PadBoth("abc", 2) => "abc";
        /// </code>
        /// </example>
        [ContractAnnotation("str:null => null;str:notnull => notnull"), Pure]
        public static string PadBoth([CanBeNull] this string str, int length)
        {
            if (str == null)
            {
                return null;
            }
            var spaces = length - str.Length;
            var padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }

        [ContractAnnotation("str:null => null;str:notnull => notnull"), Pure]
        public static string TrimSafe([CanBeNull] this string str, params char[] trimChars)
        {
            return str?.Trim(trimChars);
        }

        /// <summary>
        /// Соединяет строки из последовательности <paramref name="items"/> в одну строку,
        /// вставляя между ними разделитель <paramref name="separator"/>.
        /// </summary>
        /// <param name="items">Строки, которые нужно объединить.</param>
        /// <param name="separator">Разделитель, вставляемый между строками из <paramref name="items"/>.</param>
        /// <returns>
        /// Строка, полученная соединением строк из последовательности <paramref name="items"/> с
        /// использованием разделителя <paramref name="separator"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/><c> == null</c> или
        /// <paramref name="separator"/><c> == null</c>
        /// </exception>
        [Pure]
        [NotNull]
        public static string Join([NotNull, InstantHandle] this IEnumerable<string> items, [NotNull] string separator)
        {
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            return string.Join(separator, items);
        }

        /// <summary>
        /// Соединяет строки из последовательности <paramref name="items"/> в одну строку,
        /// вставляя между ними разделитель <paramref name="separator"/>.
        /// </summary>
        /// <param name="items">Строки, которые нужно объединить.</param>
        /// <param name="separator">Разделитель, вставляемый между строками из <paramref name="items"/>.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// Строка, полученная соединением строк из последовательности <paramref name="items"/> с
        /// использованием разделителя <paramref name="separator"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/><c> == null</c> или
        /// <paramref name="separator"/><c> == null</c>
        /// </exception>
        [Pure]
        [NotNull]
        public static string Join<T>([NotNull, InstantHandle] this IReadOnlyCollection<T> items, [NotNull] string separator, [NotNull] Func<T, string> selector)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            if (items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return selector(items.First());

            return string.Join(separator, items.Select(selector));
        }

        /// <summary>
        /// Соединяет строки из последовательности <paramref name="items"/> в одну строку,
        /// вставляя между ними разделитель-запятую.
        /// </summary>
        /// <param name="items">Строки, которые нужно объединить.</param>
        /// <returns>
        /// Строка, полученная соединением строк из последовательности <paramref name="items"/> с
        /// использованием разделителя-запятой.
        /// </returns>
        public static string JoinWithComma([NotNull, InstantHandle] this IEnumerable<string> items)
        {
            return Join(items, ", ");
        }

        /// <summary>
        /// Соединяет строки из последовательности <paramref name="items"/>. В начало каждой строки
        /// добавляется символ "-"; строки разделяются символом переноса строки.
        /// </summary>
        /// <param name="items">Строки</param>
        /// <param name="listLimit">
        /// Ограничение количества выводимых строк. Выводятся первые <paramref name="listLimit"/> строк,
        /// потом добавляется многоточие. Если <paramref name="listLimit"/> = 0, то выводятся все строки.
        /// </param>
        /// <remarks>
        /// Можно использовать для формирования списка ошибок или любого другого списка, где
        /// каждый элемент списка должен располагаться на новой строке.
        /// </remarks>
        public static string JoinWithDash([NotNull, InstantHandle] this IEnumerable<string> items, int listLimit = 10)
        {
            var limitedItems = items.ToArray();

            if (listLimit != 0 && limitedItems.Length > listLimit)
            {
                limitedItems = limitedItems
                    .Take(listLimit)
                    .Concat(new[] { "..." })
                    .ToArray();
            }

            return limitedItems
                .Select(item => string.Format("- {0}", item))
                .Join(Environment.NewLine);
        }

        /// <summary>
        /// Обнуляет внутренее содержимое строки, заполняя его символом: '\0'.
        /// </summary>
        /// <remarks>
        /// Метод предназначен для очистки важных данных, таких как пароль, из памяти.
        /// </remarks>
        /// <param name="stringBuilder"></param>
        public static void Clear(this StringBuilder stringBuilder)
        {
            const char ZeroChar = '\0';
            for (var i = 0; i < stringBuilder.Length; i++)
            {
                stringBuilder[i] = ZeroChar;
            }
            stringBuilder.Remove(0, stringBuilder.Length);
        }

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="str">The string to test</param>
        /// <returns>true if the value parameter is null or <see cref="string.Empty"/>, or if value consists exclusively of white-space characters.</returns>
        [ContractAnnotation("null=>true", true), Pure]
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Проверяет, состоит ли переданная строка только из пробельных символов.
        /// </summary>
        /// <param name="str">Строка для проверки</param>
        /// <returns>
        /// true, если строка состоит только из пробельных символов;
        /// false, если строка равна null или <see cref="string.Empty"/>, или содержит непробельные символы.
        /// </returns>
        [Pure]
        public static bool IsWhiteSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            for (var i = 0; i < str.Length; i++)
            {
                if (!char.IsWhiteSpace(str, i))
                    return false;
            }

            return true;
        }

        [Pure]
        public static int NullSafeLength(this string str)
        {
            return str == null
                ? -1
                : str.Length;
        }

        [Pure]
        [ContractAnnotation("null => null; notnull => notnull")]
        public static string NormalizeNewLines([CanBeNull] this string str)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : str.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        /// <summary>
        /// Возвращает обрезанную строку в виде "Стр...", если ее длина превышает <paramref name="length"/>,
        /// иначе возвращает <paramref name="str"/> неизмененной.
        /// </summary>
        /// <param name="str">Строка, которую необходимо обрезать.</param>
        /// <param name="length">Длина возвращаемой строки.</param>
        [Pure]
        [ContractAnnotation("str: null => null; str: notnull => notnull")]
        public static string TrimWithEllipsis([CanBeNull] this string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "length should be greater or equal to zero");
            }

            if (str == null || str.Length <= length)
            {
                return str;
            }

            const char ellipsis = '.';
            const string ellipsisString = "...";
            return length < ellipsisString.Length
                ? new string(ellipsis, length)
                : str.Substring(0, length - ellipsisString.Length) + ellipsisString;
        }

        [NotNull, Pure]
        public static string FormatPairs([NotNull] string resultFormatString,
            [NotNull] IEnumerable<Pair<string, string>> formatStringsWithValues,
            [NotNull] string separator)
        {
            if (resultFormatString == null)
                throw new ArgumentNullException(nameof(resultFormatString));
            if (formatStringsWithValues == null)
                throw new ArgumentNullException(nameof(formatStringsWithValues));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            return string
                .Format(resultFormatString, formatStringsWithValues
                    .Select(pair => FormatValue(pair.First, pair.Second, separator))
                    .ToArray())
                .Trim(separator.ToCharArray());
        }

        [NotNull, Pure]
        private static string FormatValue([CanBeNull] string format, [CanBeNull] string value, [NotNull] string separator)
        {
            Debug.Assert(separator != null);

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return (format == null ? value : string.Format(format, value)) + separator;
        }

        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static string Between(this string value, string a, string b)
        {
            var posA = value.IndexOf(a, StringComparison.Ordinal);
            var posB = value.LastIndexOf(b, StringComparison.Ordinal);

            if (posA == -1)
                return string.Empty;
            if (posB == -1)
                return string.Empty;

            var adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
                return string.Empty;

            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            var posA = value.IndexOf(a, StringComparison.Ordinal);
            if (posA == -1)
                return string.Empty;

            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            var posA = value.LastIndexOf(a, StringComparison.Ordinal);
            if (posA == -1)
                return string.Empty;

            var adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
                return string.Empty;

            return value.Substring(adjustedPosA);
        }
        /// <summary>
        /// Синтаксический сахар для <see cref="StringBuilder"/>: AppendFormat + AppendLine
        /// </summary>
        public static StringBuilder AppendFormatLine(this StringBuilder stringBuilder, string format, params Object[] args)
        {
            return stringBuilder.AppendFormat(format, args).AppendLine();
        }

        /// <summary>
        /// Добавляет в <see cref="StringBuilder"/> множество строк, разделяя их разрывами строки,
        /// и, при необходимости, дополнительными разделителями
        /// </summary>
        /// <param name="stringBuilder">StringBuilder</param>
        /// <param name="lines">Множетво строк</param>
        /// <param name="separator">Строка для использования в качестве разделителя. 
        /// <c>separator</c> включается в возвращаемую строку, только если в <c>lines</c>более одного элемента.</param>
        public static StringBuilder AppendLines(this StringBuilder stringBuilder, IEnumerable<string> lines, bool offset = false, string separator = null)
        {
            if (offset)
                lines = lines.Select(line => "    " + line);

            return stringBuilder.AppendJoin(separator + Environment.NewLine, lines);
        }

        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, string separator, IEnumerable<string> values)
        {
            if (separator == null)
                separator = string.Empty;

            using (var enumerator = values.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return stringBuilder;
                if (enumerator.Current != null)
                    stringBuilder.Append(enumerator.Current);

                while (enumerator.MoveNext())
                {
                    stringBuilder.Append(separator);
                    if (enumerator.Current != null)
                        stringBuilder.Append(enumerator.Current);
                }
                return stringBuilder;
            }
        }

        public static string DefaultIfEmpty(this string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// Считывает из строки указанное количество строк
        /// </summary>
        /// <param name="str">Исходная строка</param>
        /// <param name="linesLimit">Количество строк для чтения</param>
        /// <param name="charLimit">Максимальное количество выводимых символов. По умолчанию количество не ограничивается</param>
        /// <param name="trimmedEnd">Строка, добавляемая выводимой части в случае, когда исходная строка обрезается</param>
        /// <returns></returns>
        public static string ReadLines(this string str, int linesLimit, int charLimit = -1, string trimmedEnd = null)
        {
            var strLenght = str.Length;
            var length = charLimit >= 0 ? charLimit : str.Length;
            var lCount = 1;
            var cCount = 0;
            var i = 0;

            while (i < strLenght && cCount < length)
            {
                var ch = str[i++];
                if (ch == '\r' || ch == '\n')
                {
                    if (++lCount > linesLimit)
                    {
                        i--;
                        break;
                    }

                    if (ch == '\r' && i < length && str[i] == '\n')
                        i++;
                }
                else
                    cCount++;
            }

            if (i == strLenght)
                return str;

            return str.Substring(0, i) + trimmedEnd;
        }

        public static int IntParseOrDefault(this string s)
        {
            return int.TryParse(s, out var result) ? result : 0;
        }
        #endregion
    }
}