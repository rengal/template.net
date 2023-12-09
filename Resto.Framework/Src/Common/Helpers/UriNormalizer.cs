using System;
using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Properties;

namespace Resto.Framework.Common
{
    public static class UriNormalizer
    {
        /// <summary>
        /// Подставляет в url протокол protocol, если url указано без протокола
        /// </summary>
        /// <param name="url">Исходное значение url</param>
        /// <param name="defaultProtocol">Протокол по умолчанию, который будет подставлен в url</param>
        /// <param name="suitableProtocols">Список принимаемых протоколов</param>
        /// <returns>Исходный url, если в нем проставлен протокол. Либо исходный url,
        /// но с проставленным протоколом defaultProtocol, если итоговое url стало валидным.</returns>
        /// <remarks>
        /// В итоговом url будет проставлен протокол, если его изначально не было. Например:
        /// адрес типа "http://localhost:8080/resto" или "https://localhost:8080/resto" останется без изменений.
        /// адрес типа "htp://localhost:8080/resto"  или "ftp://localhost:8080/resto" останется без изменений.
        /// адрес типа "localhost:8080/resto" превратится в "http://localhost:8080/resto".
        /// адрес типа "//localhost:8080/resto" превратится в "http://localhost:8080/resto"
        /// А также, чтобы не поломать текущее построение строки запроса вида "<значение serverUrl> + /v1/api/...",
        /// метод возвращает url без "/" в качестве последнего символа:
        /// адрес типа "//localhost/resto/" превратится в "http://localhost/resto".</remarks>
        public static string NormalizeUrl([NotNull] string url, [NotNull] string defaultProtocol, [NotNull] List<string> suitableProtocols)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(url));
            if (defaultProtocol.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(defaultProtocol));
            if (suitableProtocols == null || suitableProtocols.IsEmpty())
                throw new ArgumentNullException(nameof(suitableProtocols));

            // Допустим, url уже со протоколом. В случае как корректного протокола ("http://localhost:8080/resto", "ftp://localhost:8080/resto"),
            // так и ошибочного ("htp://localhost:8080/resto"), url не меняем.
            // Сложность возникает с тем, что, допустим, адрес вида localhost:8080 также парсится UriBuilder'ом, но как Scheme = localhost и Path = 8080.
            // Поэтому с помощью второго параметра мы отсекаем здесь обработку адресов вида "localhost:8080" - они будут обработаны ниже.
            var uri = CorrectUri(url, suitableProtocols, url.Contains("://"));
            if (uri != null)
                return uri;

            // Допустим, нет протокола и нет "://" - например "localhost:8080/resto"
            uri = CorrectUri(defaultProtocol + "://" + url, suitableProtocols);
            if (uri != null)
                return uri;

            // Допустим, нет протокола, но есть "//" - например "//localhost:8080/resto"
            uri = CorrectUri(defaultProtocol + ":" + url, suitableProtocols);
            if (uri != null)
                return uri;

            return url;
        }

        private static string CorrectUri([NotNull] string url, [NotNull] List<string> suitableProtocols, bool force = false)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(url));
            if (suitableProtocols == null || suitableProtocols.IsEmpty())
                throw new ArgumentNullException(nameof(suitableProtocols));

            UriBuilder uriBuilder;

            try
            {
                uriBuilder = new UriBuilder(url);

                // В левую часть условия попадают неверно распарсенные url типа "localhost:8080/resto", когда UriBuilder считает, что
                // uriBuilder.Scheme = "localhost", uriBuilder.Host = "", uriBuilder.Port = -1, uriBuilder.Path = 8080/resto.
                // В правую часть условия попадаем, если у url сразу была не была проставлена схема, неважно валидная или нет.
                if (!suitableProtocols.Contains(uriBuilder.Scheme) && !force)
                    return null;
            }
            // Некорректный url, например "://localhost/resto" или "//localhost/resto"
            catch (UriFormatException)
            {
                return null;
            }

            var resultUri = uriBuilder.Uri.ToString();

            // Если в uriBuilder значение пути пусто, то свойство Path будет содержать только символ "/" и им будет заканчиваться свойство Uri.
            // И нам надо этот слэш убрать, чтобы не поломать текущее построение строки запроса вида "<значение serverUrl> + /v1/api/..."
            // Аналогично, если исходный url заканчивался слэшом, например "//localhost/resto/".
            if (uriBuilder.Path.EndsWith("/"))
                return resultUri.Remove(resultUri.Length - 1);

            return resultUri;
        }
    }
}
