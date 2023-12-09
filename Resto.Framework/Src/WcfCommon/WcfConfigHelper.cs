using System;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.WcfCommon
{
    public static class WcfConfigHelper
    {
        private static readonly Regex AddressWithPortRegex = new Regex(@".+:(\d{1,5})\/?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Получаем Uri сервиса путем объединения значения по-умолчанию из конфига и задаваемого пользователем адреса.
        /// </summary>
        public static Uri GetPartiallyOverridedUri([NotNull] Uri wcfConfigUri, [NotNull] string overrideUriString)
        {
            //в настройках допускаем формат без схемы
            var uriFromSettings = new UriBuilder(overrideUriString).Uri;

            // если в пользовательском Uri указан порт, брать его
            var port = AddressWithPortRegex.IsMatch(overrideUriString) ? uriFromSettings.Port : wcfConfigUri.Port;

            //в конфиге всё строго
            var actualUri = new UriBuilder(wcfConfigUri.Scheme, uriFromSettings.Host, port, wcfConfigUri.PathAndQuery).Uri;
            return actualUri;
        }
    }
}