using System;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Search
{
    public static class SearchUtils
    {
        /// <summary>
        /// Отфильтровать строку, оставив в ней только цифры.
        /// </summary>
        /// <param name="s">Исходная строка.</param>
        /// <returns>Отфильтрованная строка.</returns>
        [Pure]
        public static string LeaveOnlyDigits([NotNull] this string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            var sb = new StringBuilder();
            for (var i = 0; i < s.Length; ++i)
                if (char.IsDigit(s[i]))
                    sb.Append(s[i]);
            return sb.ToString();
        }
    }
}