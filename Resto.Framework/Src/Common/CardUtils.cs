using System;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class CardUtils
    {

        /// <summary>
        /// Номер карты в шестнадцатеричном формате.
        /// Разрешены цифры и буквы от A до f, 
        /// а также часть символов для построения маски(групповой карты).
        /// Плагин доставки младше 6.4 принимает карты в этом формате.
        /// </summary>
        public const string HexadecimalSymbolsRegexPattern = @"(\d|=|\*|\?|[a-fA-F]+){1,32}";
        public static readonly Regex HexadecimalSymbolsRegex = new Regex("^" + HexadecimalSymbolsRegexPattern + "$");

        /// <summary>
        /// Номер карты с любыми буквами и цифрами
        /// и символы для построения маски(групповой карты).
        /// </summary>
        public const string AnySymbolsRegexPattern = @"((\d|=|\*|\?|\.|\,|\$|\^|\{|\[|\(|\||\)|\+|\?|\\|\}|\]|\-|\w)+)";
        public static readonly Regex AnySymbolsRegex = new Regex("^" + AnySymbolsRegexPattern + "$");

        /// <summary>
        /// Маскирует трек карты.
        /// </summary>
        /// <returns>Трек карты в виде "12*****34"</returns>
        /// <remarks>
        /// Если трек пустой или корочее 4 символов, трек возвращается не изменным.
        /// Исходных символов останется "длина строки"/2, но не более 8. 
        /// При это половина этих символов будет вначале, половина в конце, а остальные символы будут заменены на '*'.
        /// </remarks>
        [CanBeNull]
        public static string MaskCardTrack([CanBeNull] string track)
        {
            if (string.IsNullOrEmpty(track) || track.Length < 4)
                return track;

            const int maxVisibleChars = 8;

            var visibleChars = Math.Min(track.Length / 2, maxVisibleChars);
            visibleChars = visibleChars % 2 == 0 ? visibleChars : visibleChars + 1;
            var visibleCharsHalf = visibleChars / 2;

            return track.Substring(0, visibleCharsHalf)
                + new string('*', track.Length - visibleChars)
                + track.Substring(track.Length - visibleCharsHalf);
        }
    }
}