using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Phone
{
    public static class PhoneUtils
    {
        /// <summary>
        /// Номер телефона в международном формате.
        /// </summary>
        private const string PhoneRegexString =
            @"^(8|\+?\d{1,3})? ?\(?(\d{3})\)? ?(\d{3})[ -]?(\d{2})[ -]?(\d{2})$";

        /// <summary>
        /// Неполный номер телефона в международном формате (например, номер в процессе ввода с UI).
        /// </summary>
        private const string UncompletedPhoneRegexString =
            @"^(8|\+?\d{1,}?)? ?\(?(\d{0,3})\)? ?(\d{0,3})[ -]?(\d{0,2})[ -]?(\d{0,2})$";

        /// <summary>
        /// Номер телефона без кода страны.
        /// </summary>
        private const string PhoneWithoutCountryCodeRegexString =
            @"^\(?(\d{3})\)? ?(\d{3})[ -]?(\d{2})[ -]?(\d{2})$";

        /// <summary>
        /// Номер телефона без кода страны и без кода города.
        /// </summary>
        private const string ShortPhoneRegexString =
            @"^(\d{1,3})[ -]?(\d{1,2})[ -]?(\d{2})$";

        private static readonly Regex PhoneRegex = new Regex(PhoneRegexString,
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex UncompletedPhoneRegex = new Regex(UncompletedPhoneRegexString,
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex PhoneWithoutCountryCodeRegex = new Regex(PhoneWithoutCountryCodeRegexString,
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ShortPhoneRegex = new Regex(ShortPhoneRegexString,
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly string FakePhoneNumberWithoutCountryCode = "0000000000";
        private static readonly string FakePhoneNumber = "+00000000000";

        /// <summary>
        /// Нормализует телефонный номер <paramref name="match"/> в полном формате:
        /// нормализует код страны, а также убирает форматирование.
        /// </summary>
        [NotNull]
        private static string Normalize([NotNull] Match match, string cultureName)
        {
            Contract.Requires(IsMatch(match));
            Contract.Requires(!cultureName.IsNullOrEmpty());

            var countryCode = match.Groups[1];
            return GetNormalizedCountryCode(countryCode.Value, cultureName) +
                 string.Concat(match.Groups.Cast<Group>().Where(g => g.Success).Skip(2).Select(g => g.Value));
        }

        /// <summary>
        /// Возвращает фиктивный, несуществующий номер телефона в международном формате.
        /// Используется в самовывозной доставке без клиента для снятия ограничения на обязательность телефона.
        /// </summary>
        [NotNull]
        public static string GetFakePhoneNumber([CanBeNull] string countryCode)
        {
            if (!countryCode.IsNullOrEmpty())
                return string.Concat($"+{countryCode}", FakePhoneNumberWithoutCountryCode);
            return FakePhoneNumber;
        }

        /// <summary>
        /// Нормализует номер <paramref name="phone"/>, если он проходит валидацию в международном формате.
        /// Иначе возвращает <c>null</c>.
        /// </summary>
        [CanBeNull]
        public static string TryNormalize([NotNull] string phone, [NotNull] string cultureName)
        {
            Contract.Requires(!phone.IsNullOrEmpty());
            Contract.Requires(!cultureName.IsNullOrEmpty());
            if (phone.IsNullOrWhiteSpace())
                return null;

            if (phone[0] == '+')
            {
                if (phone.Length < 12 || phone.Length > 14)
                    return null;

                for (var i = 1; i < phone.Length; i++)
                {
                    if (phone[i] < '0' || phone[i] > '9')
                        break;
                    if (i == phone.Length - 1)
                        return phone;
                }
            }

            var match = PhoneRegex.Match(phone);
            return IsMatch(match)
                ? Normalize(match, cultureName)
                : null;
        }

        /// <summary>
        /// Нормализует номер <paramref name="phone"/>, если он проходит валидацию формате номера, находящегося в процессе ввода.
        /// Иначе возвращает <c>null</c>.
        /// </summary>
        [CanBeNull]
        public static string TryNormalizeUncompletedPhone([NotNull] string phone, [NotNull] string cultureName)
        {
            Contract.Requires(!phone.IsNullOrEmpty());
            Contract.Requires(!cultureName.IsNullOrEmpty());

            var match = UncompletedPhoneRegex.Match(phone);
            if (match.Success && match.Groups.Count > 1)
            {
                //для неполного номера критично распарсить самую первую группу. Во-первых, иного способа получить код страны в неполном номере нет.
                //Во-вторых, это исключает проблемные ситуации с вводом символов вроде "(" или "-",
                //которые подходят под регулярку из-за того, что группы необязательны, но при этом группа остается не распознанной.
                var countryCode = match.Groups[1];
                if (countryCode.Success)
                    return GetNormalizedCountryCode(countryCode.Value, cultureName) +
                        match.Groups.Cast<Group>().TakeWhile(g => g.Success).Skip(2).Select(g => g.Value).Join("");
                return null;
            }
            return null;
        }

        /// <summary>
        /// Если номер <paramref name="phone"/> проходит валидацию в международном формате, то нормализует его.
        /// Иначе рассматривает <paramref name="phone"/> как короткий номер и пытается нормализовать.
        /// Если ни один из форматов не подошел, возвращает номер нетронутым.
        /// </summary>
        /// <param name="phone">Телефонный номер, подлежащий нормализации</param>
        /// <param name="cultureName">Имя текущей культура</param>
        /// <param name="cityCode">Телефонный код города</param>
        /// <param name="countryCode">Код страны</param>
        [NotNull]
        public static string Normalize([NotNull] string phone, [NotNull] string cultureName,
            [CanBeNull] string cityCode, [CanBeNull] string countryCode)
        {
            var match = PhoneRegex.Match(phone);
            if (IsMatch(match))
                return Normalize(match, cultureName);

            if (IsMatchNumberWithoutCountryCode(phone) && !countryCode.IsNullOrEmpty())
            {
                var result = NormalizeNumberWithoutCountryCode(phone, cultureName, countryCode);
                if (IsMatch(result))
                    return result;
            }

            if (IsMatchShortNumber(phone) && !cityCode.IsNullOrEmpty() && !countryCode.IsNullOrEmpty())
            {
                var result = NormalizeShortNumber(phone, cultureName, cityCode, countryCode);
                if (IsMatch(result))
                    return result;
            }

            return phone;
        }


        /// <summary>
        /// Возвращает номер телефона в международном формате.
        /// Если ни один из известных форматов не подошел, возвращает номер нетронутым.
        /// </summary>
        /// <param name="phone"> Нормальзованный телефонный номер</param>
        [NotNull]
        public static string ConvertToInternationalFormat([NotNull] string phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            var match = PhoneRegex.Match(phone);
            if (IsMatch(match))
            {
                var parts = new List<string>
                {
                    match.Groups.Cast<Group>().Skip(1).SkipLast(3).Select(g => g.Value).Join(" "),
                    match.Groups.Cast<Group>().Skip(3).Select(g => g.Value).Join("-")
                };
                return parts.Join(" ");
            }
            if (IsMatchNumberWithoutCountryCode(phone))
            {
                var matchMatchNumberWithoutCountryCode = PhoneWithoutCountryCodeRegex.Match(phone);
                var parts = new List<string>
                {
                    matchMatchNumberWithoutCountryCode.Groups.Cast<Group>().Skip(1).SkipLast(3).Select(g => g.Value).Join(" "),
                    matchMatchNumberWithoutCountryCode.Groups.Cast<Group>().Skip(2).Select(g => g.Value).Join("-")
                };
                return parts.Join(" ");
            }
            if (IsMatchShortNumber(phone))
            {
                var matchShortNumber = ShortPhoneRegex.Match(phone);
                return matchShortNumber.Groups.Cast<Group>().Skip(1).Select(g => g.Value).Join("-");
            }

            return phone;
        }

        public static string NormalizeWithCountryCodeReplacement([NotNull] string phone, [NotNull] string cultureName,
            [CanBeNull] string cityCode, [CanBeNull] string cafeSetupCountryCode, [NotNull] string countryCodeReplacement)
        {
            var normalizedPhone = Normalize(phone, cultureName, cityCode, cafeSetupCountryCode);
            var normalizedCountryCode = GetNormalizedCountryCode(cafeSetupCountryCode, cultureName);
            if (normalizedCountryCode != null && normalizedPhone.StartsWith(normalizedCountryCode))
            {
                return countryCodeReplacement + normalizedPhone.Substring(normalizedCountryCode.Length);
            }
            return normalizedPhone;
        }

        /// <summary>
        /// Возвращает признак того, является ли номер <paramref name="phone"/> валидным телефонным номером 
        /// в международном формате либо коротким номером (при условии, что задан код города и страны).
        /// </summary>
        /// <param name="phone">Телефонный номер, подлежащий валидации</param>
        /// <param name="cultureName">Имя текущей культура</param>
        /// <param name="cityCode">Код города</param>
        /// <param name="countryCode">Код страны</param>
        public static bool Validate([CanBeNull] string phone, [NotNull] string cultureName,
            [CanBeNull] string cityCode, [CanBeNull] string countryCode)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            if (IsMatch(phone))
                return true;
            if (IsMatchNumberWithoutCountryCode(phone))
                return true;

            if (IsMatchShortNumber(phone))
            {
                return !countryCode.IsNullOrWhiteSpace() && !cityCode.IsNullOrEmpty()
                    && cityCode.Split(';').Length == 1
                    && IsMatch(NormalizeShortNumber(phone, cultureName, cityCode, countryCode));
            }
            return false;
        }

        /// <summary>
        /// Возвращает признак того, что строка <paramref name="phone"/> является телефонным номером в международном формате.
        /// </summary>
        /// <param name="phone">Строка для проверки</param>
        public static bool IsMatch([NotNull] string phone)
        {
            Contract.Requires(!phone.IsNullOrEmpty());

            return IsMatch(PhoneRegex.Match(phone));
        }

        private static bool IsMatch([NotNull] Match match)
        {
            if (!match.Success || match.Groups.Count != 6)
                return false;

            for (var i = 0; i < 6; i++)
            {
                var group = match.Groups[i];
                if (!group.Success || group.Value.IsNullOrWhiteSpace())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Возвращает признак того, что строка <paramref name="phone"/> является телефонным номером в международном формате без кода страны.
        /// </summary>
        /// <param name="phone">Строка для проверки</param>
        private static bool IsMatchNumberWithoutCountryCode([NotNull] string phone)
        {
            Contract.Requires(!phone.IsNullOrEmpty());

            var match = PhoneWithoutCountryCodeRegex.Match(phone);
            return match.Success && match.Groups.Count == 5 &&
                   match.Groups.OfType<Group>().Take(5).All(g => g.Success && !g.Value.IsNullOrWhiteSpace());
        }

        /// <summary>
        /// Приводит телефонный номер без кода страны к международному формату.
        /// </summary>
        [NotNull]
        private static string NormalizeNumberWithoutCountryCode([NotNull] string phone, [NotNull] string cultureName, [NotNull] string countryCode)
        {
            Contract.Requires(IsMatchNumberWithoutCountryCode(phone));
            Contract.Requires(!phone.IsNullOrEmpty());
            Contract.Requires(!cultureName.IsNullOrEmpty());

            var normalizedCountryCode = GetNormalizedCountryCode(countryCode, cultureName);
            var match = PhoneWithoutCountryCodeRegex.Match(phone);
            return normalizedCountryCode + match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).Join("");
        }

        /// <summary>
        /// Возвращает признак того, что строка <paramref name="phone"/> является телефонным номером в международном или коротком формате.
        /// </summary>
        /// <param name="phone">Строка для проверки</param>
        public static bool IsMatchFullOrWithoutCountryCodeOrShortNumber([NotNull] string phone)
        {
            return IsMatch(phone) || IsMatchNumberWithoutCountryCode(phone) || IsMatchShortNumber(phone);
        }

        /// <summary>
        /// Приводит телефонный номер в коротком формате к международному формату.
        /// </summary>
        [NotNull]
        private static string NormalizeShortNumber([NotNull] string phone, [NotNull] string cultureName, [NotNull] string cityCode, [NotNull] string countryCode)
        {
            Contract.Requires(IsMatchShortNumber(phone));
            Contract.Requires(!phone.IsNullOrEmpty());
            Contract.Requires(!cultureName.IsNullOrEmpty());
            Contract.Requires(!cityCode.IsNullOrEmpty());

            var normalizedCountryCode = GetNormalizedCountryCode(countryCode, cultureName);
            var match = ShortPhoneRegex.Match(phone);
            return normalizedCountryCode + cityCode +
                match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).Join("");
        }

        /// <summary>
        /// Возвращает признак того, что строка <paramref name="phone"/> является телефонным номером в коротком формате.
        /// </summary>
        /// <param name="phone">Строка для проверки</param>
        private static bool IsMatchShortNumber([NotNull] string phone)
        {
            Contract.Requires(!phone.IsNullOrEmpty());

            return ShortPhoneRegex.IsMatch(phone);
        }

        private static bool IsRussianUser(string cultureName)
        {
            return string.Equals(cultureName, "ru", StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(cultureName, "ru-Ru", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Пытается нормализовать код страны <paramref name="countryCode"/>.
        /// </summary>
        [CanBeNull]
        public static string GetNormalizedCountryCode([CanBeNull] string countryCode, [NotNull] string cultureName)
        {
            if (cultureName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(cultureName));

            if (countryCode == "8" && IsRussianUser(cultureName))
                return "+7";

            if (!string.IsNullOrEmpty(countryCode) && countryCode[0] != '+')
                return $"+{countryCode}";

            return countryCode;
        }

        /// <summary>
        /// Пытается нормализовать код страны <paramref name="countryCode"/>.
        /// </summary>
        [CanBeNull]
        public static string GetNotNormalizedCountryCode([CanBeNull] string countryCode, [NotNull] string cultureName)
        {
            if (cultureName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(cultureName));

            if ((countryCode == "7" || countryCode == "+7") && IsRussianUser(cultureName))
                return "8";

            if (!string.IsNullOrEmpty(countryCode) && countryCode[0] == '+')
                return countryCode.Substring(1);

            return countryCode;
        }

        /// <summary>
        /// Маскирует середину телефонного номера
        /// </summary>
        /// <param name="phone">Номер телефона</param>
        /// <returns></returns>
        [CanBeNull]
        public static string HidePhoneString(string phone)
        {
            //Минимальная длинна номера, что короче скрывать смысла нет.
            const int minPhoneLength = 7;
            const int firstVisiblePhonePart = 5;
            const int lastVisiblePhonePart = 2;

            //Берем первые 5 символов,далее вставляем маскировку *-- и после последние 2 цифры номера
            var res = phone;
            if (phone.Length >= minPhoneLength)
                res = $"{phone.Substring(0, firstVisiblePhonePart)} *-- {phone.Substring(phone.Length - lastVisiblePhonePart)}";
            return res;
        }
    }
}
