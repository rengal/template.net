using System;
using System.IO;
using System.Net;
using log4net;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common.Services.ConnectionCode
{
    /// <summary>
    /// Класс общения с веб-сервисом, который раздает коды для быстрого подключения фронтов.
    /// </summary>
    public static class ConnectionCodeService
    {
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(ConnectionCodeService));

        #region Constants

        /// <summary>
        /// Имя веб-сервиса
        /// </summary>
        public const string WebServiceName = "id.iiko.online";

        /// <summary>
        /// Адрес веб-сервиса
        /// </summary>
        private static readonly string WebServiceUrl = $@"https://{WebServiceName}/api/front/codes/";

        /// <summary>
        /// Метод - запрос кода
        /// </summary>
        private const string Push = "push";

        /// <summary>
        /// Метод - запрос данных кассы
        /// </summary>
        private const string Pop = "pop";

        /// <summary>
        /// Тип обмена данными с сервисом - JSON
        /// </summary>
        private const string ContentType = "application/json";

        private const string Post = "POST";

        #endregion

        #region Methods

        /// <summary>
        /// Запрос кода для быстрого подключения фронта
        /// </summary>
        /// <param name="serverUrl">Адрес сервера</param>
        /// <param name="agentId">Идентификатор агента</param>
        /// <param name="terminalId">Идентификатор терминала</param>
        /// <param name="error">Ошибка общения с сервисом выдачи кодов подключения</param>
        [CanBeNull]
        public static string GetCode([NotNull] string serverUrl, Guid agentId, Guid terminalId, out ConnectionCodeServiceFailReason error)
        {
            if (serverUrl == null)
                throw new ArgumentNullException(nameof(serverUrl));

            var code = PostRequest(JsonHelper.ToJson(new CodePushRequest(serverUrl, agentId, terminalId)), Push, out error);

            Log.InfoFormat("Code = {0} for agent: {1}, terminal: {2}.", code, agentId, terminalId);

            return code;
        }

        /// <summary>
        /// Запрос информации терминала: адрес сервера, идентификатор агента, идентификатор терминала
        /// </summary>
        /// <param name="code">Код быстрого подключения фронта</param>
        /// <param name="error">Ошибка общения с сервисом выдачи кодов подключения</param>
        [CanBeNull]
        public static TerminalInfo GetTerminalInfo([NotNull] string code, out ConnectionCodeServiceFailReason error)
        {
            if (!int.TryParse(code, out var codeNumber))
            {
                error = ConnectionCodeServiceFailReason.CodeNotFound;
                return null;
            }

            var terminalInfo = PostRequest(JsonHelper.ToJson(new CodePopRequest(codeNumber)), Pop, out error);
            return terminalInfo != null ? JsonHelper.FromJson<TerminalInfo>(terminalInfo) : null;
        }

        #endregion Methods

        #region Helpers

        [CanBeNull]
        private static string PostRequest([NotNull] string jsonRequest, [NotNull] string type, out ConnectionCodeServiceFailReason error)
        {
            if (jsonRequest == null)
                throw new ArgumentNullException(nameof(jsonRequest));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebServiceUrl + type);
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Method = Post;

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonRequest);
                }

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute https://stackoverflow.com/a/16911086
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        error = ConnectionCodeServiceFailReason.None;
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException we)
            {
                error = !(we.Response is HttpWebResponse response) ? ConnectionCodeServiceFailReason.ServiceUnavailable : GetError(response.StatusCode);
                Log.Warn(we.Message);
                return null;
            }
            catch (IOException e)
            {
                error = ConnectionCodeServiceFailReason.ServiceUnavailable;
                Log.Warn(e.Message);
                return null;
            }
            catch (Exception e)
            {
                error = ConnectionCodeServiceFailReason.UnexpectedError;
                Log.Error(e);
                return null;
            }
        }

        private static ConnectionCodeServiceFailReason GetError(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Forbidden:
                    return ConnectionCodeServiceFailReason.CodeWasUsed;
                case HttpStatusCode.NotFound:
                    return ConnectionCodeServiceFailReason.CodeNotFound;
                case HttpStatusCode.InternalServerError:
                    return ConnectionCodeServiceFailReason.InternalServiceError;
                case HttpStatusCode.ServiceUnavailable:
                    return ConnectionCodeServiceFailReason.ServiceUnavailable;
                default:
                    return ConnectionCodeServiceFailReason.UnexpectedError;
            }
        }

        #endregion Helpers
    }
}
