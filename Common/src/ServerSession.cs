using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using EnumerableExtensions;
using log4net;
using Resto.Common;
using Resto.Common.Properties;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.Framework.Localization;

namespace Resto.Data
{
    /// <remarks>
    /// Творчески скопипащено в код доставочного плагина старого КЦ: dev\iikoFront.Net\Resto.Front.Api.Delivery\ServerCallsDeliveryImpl\ServerSession.cs
    /// </remarks>
    public class ServerSession
    {
        #region Consts

        public const string CLIENT_BACK = "BACK";
        public const string CLIENT_FRONT_FF = "FRONT_FF";
        public const string CLIENT_FRONT_TS = "FRONT_TS";
        public const string CLIENT_INTEGRATION = "INTEGRATION";
        public const string CLIENT_INTEGRATION_LIGHT = "INTEGRATION_LIGHT";
        public const string CLIENT_UPDATER = "UPDATER";

        public const string DEFAULT_INTEGRATION_USER_LOGIN = "SystemIntegrationUser";

        #endregion

        private static readonly object mutex = new object();
        private static volatile ServerSession currentSession;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerSession));

        public delegate void EmptyHandler();

        /// <summary>
        /// Срабатывает при принудительном разрыве связи с сервером.
        /// </summary>
        public static event EmptyHandler Disconnected;

        /// <summary>
        /// Определяет, будет ли вызываться метод <see cref="IBaseEntityManager.SaveEntities"/>
        /// при вызове метода <see cref="Disconnect"/>. Для фронта это ненужно.
        /// </summary>
        public static bool SaveEntitiesOnDisconnect = true;

        public static bool IsConnected
        {
            get
            {
                if (currentSession == null)
                    return false;
                return !IsAuthorizationConnection()
                           ? currentSession.frontConnected
                           : currentSession.currentUser != null;
            }
        }

        public static bool IsAuthorizationConnection()
        {
            return !(currentSession.ClientType == CLIENT_FRONT_FF
                || currentSession.ClientType == CLIENT_FRONT_TS
                || currentSession.ClientType == CLIENT_INTEGRATION_LIGHT
                || currentSession.ClientType == CLIENT_UPDATER);
        }

        public static ServerSession CurrentSession
        {
            get
            {
                var local = currentSession;
                if (local == null)
                {
                    throw new RestoException("Not connected");
                }
                return local;
            }
            //Включено для тестов
            //TODO - заменить на фабрики сразу после стабилизации
            set
            {
                lock (mutex)
                {
                    if (currentSession == null)
                        currentSession = value;
                }
            }
        }

        public static void ChangeCurrentSessionPassword(string passwordHash, User user)
        {
            var session = new ServerSession(currentSession.ClientType, CommonConfig.Instance.ServerUrl, currentSession.loginName, null, currentSession.SynchronizeInvoke);
            session.passwordHash = passwordHash;
            user.PasswordHash = passwordHash;
            session.currentUser = user;
            currentSession = session;
        }

        /// <summary>
        /// Открывает соединение с сервером.
        /// </summary>
        /// <param name="clientType">Тип клиента</param>
        /// <param name="loginName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="synchronizeInvoke">Объект, по которому будут синхронизовываться обратные вызовы. Обычно - главное окон приложения.</param>
        /// <param name="cancelToken">Токен для отмены попытки подключения.</param>
        /// <returns>Результат операции соединения</returns>
        public static ConnectionResult Connect(string clientType, string loginName, string password, ISynchronizer synchronizeInvoke, CancellationToken? cancelToken = null)
        {
            return Connect(clientType, CommonConfig.Instance.ServerUrl, loginName, password, synchronizeInvoke, cancelToken);
        }

        /// <summary>
        /// Открывает соединение с сервером.
        /// </summary>
        /// <param name="clientType">Тип клиента</param>
        /// <param name="serverUrl">URL сервера</param>
        /// <param name="loginName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="synchronizeInvoke">Объект, по которому будут синхронизовываться обратные вызовы. Обычно - главное окон приложения.</param>
        /// <param name="cancelToken">Токен для отмены попытки подключения.</param>
        /// <returns>Результат операции соединения</returns>
        public static ConnectionResult Connect(string clientType, string serverUrl, string loginName,
            string password, ISynchronizer synchronizeInvoke, CancellationToken? cancelToken = null)
        {
            return DoConnect(new ServerSession(clientType, serverUrl, loginName, password, synchronizeInvoke), cancelToken);
        }

        /// <summary>
        /// Открывает соединение с сервером.
        /// При этом пробует от 1 до n из переданного набора сетевых протоколов.
        /// Предполагается использование следующим образом: сначала пробуем подключиться по HTTPS, потом по HTTP.
        /// </summary>
        /// <param name="clientType">Тип клиента</param>
        /// <param name="protocols">Сетевые протоколы, по которым пробуем подключаться.</param>
        /// <param name="serverUrlWithoutProtocol">URL сервера без протокола.</param>
        /// <param name="loginName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="synchronizeInvoke">Объект, по которому будут синхронизовываться обратные вызовы. Обычно - главное окон приложения.</param>
        /// <param name="cancelToken">Токен для отмены попытки подключения.</param>
        /// <returns>Результат последней по порядку из операций соединения.</returns>
        public static Pair<string, ConnectionResult> Connect(
            string clientType, ICollection<string> protocols,
            string serverUrlWithoutProtocol, string loginName,
            string password, ISynchronizer synchronizeInvoke, CancellationToken? cancelToken = null)
        {
            if (protocols == null)
            {
                throw new ArgumentNullException(nameof(protocols));
            }
            if (protocols.IsEmpty())
            {
                throw new ArgumentException("protocols");
            }

            Pair<string, ConnectionResult> result = default(Pair<string, ConnectionResult>);

            foreach (var protocol in protocols)
            {
                string serverUrl = protocol + serverUrlWithoutProtocol;
                Log.InfoFormat("Trying to connect to {0}", serverUrl);
                ConnectionResult connectionResult = DoConnect(new ServerSession(clientType, serverUrl, loginName, password, synchronizeInvoke), cancelToken);
                result = new Pair<string, ConnectionResult>(protocol, connectionResult);
                if (connectionResult == ConnectionResult.SUCCESS)
                {
                    break;
                }
            }

            return result;
        }


        public static ConnectionResult DoConnect(ServerSession session, CancellationToken? cancelToken = null)
        {
            currentSession = session;
            try
            {
                session.DoConnect(cancelToken);
                return ConnectionResult.SUCCESS;
            }
            catch (RestoServiceConnectionException e)
            {
                Log.Error($"{e.Message} (internal connection issue code: {e.Result}).", e);
                return e.Result;
            }
        }

        public static void Disconnect()
        {
            lock (mutex)
            {
                if (SaveEntitiesOnDisconnect)
                {
                    EntityManager.INSTANCE.SaveEntities();
                    if (CommonConfig.Instance.CanArchiveCache)
                    {
                        EntitiesCacheArchiver.Instance.ProcessArchiveCache(true);
                    }
                }

                currentSession = null;

                if (Disconnected != null)
                {
                    Disconnected();
                }
            }
        }

        private readonly string loginName;
        private string passwordHash;
        private User currentUser;
        private bool frontConnected;
        private readonly CookieContainer container;

        public ServerSession(string clientType, string serverUrl, string loginName, string password)
        {
            ClientType = clientType;
            ServerUrl = serverUrl;
            this.loginName = loginName;
            if (password != null)
                passwordHash = MathUtil.HexSHA1(password);
            container = new CookieContainer();
            LanguageCode = Localizer.Culture.TwoLetterISOLanguageName;
        }

        public ServerSession(string clientType, string serverUrl, string loginName, string password, [NotNull] ISynchronizer synchronizeInvoke)
            : this(clientType, serverUrl, loginName, password)
        {
            if (synchronizeInvoke == null)
                throw new ArgumentNullException(nameof(synchronizeInvoke));

            SynchronizeInvoke = synchronizeInvoke;
        }

        private void DoConnect(CancellationToken? cancelToken = null)
        {
            lock (mutex)
            {
                FullUpdate(cancelToken);
                if (IsAuthorizationConnection())
                {
                    currentUser = ServiceClientFactory.AuthorizationService.GetCurrentAuthInfo().CallSync().User;
                    Log.Info("Authorized as user: " + currentUser);
                }
                else
                {
                    frontConnected = true;
                    Log.Info("Connected as iikoFront.NET");
                }
            }
        }

        /// <summary>
        /// Полное обновление сущностей.
        /// </summary>
        /// <remarks>
        /// Для поддержания актуального состояния сущностей в памяти бэк всегда должен получать их
        /// обновления при сервисных вызовах <see cref="RemoteMethodCaller.RequestEntityUpdatesAsServiceCallSideEffect"/>.
        /// Для корректной работы бэкофиса в целом первый FullUpdate должен выполняться
        /// до resto.back.security.AuthorizationService#getCurrentAuthInfo
        /// (иначе при запросе придет обновление, которое будет обработано некорректно)
        /// и строго после того, как будут получены/обновлены "отпечатки", чтоб подгрузить верный кэш из файла.
        /// </remarks>
        protected virtual void FullUpdate(CancellationToken? cancelToken = null)
        {
            ServerFingerPrintsContainer.Instance.UpdateFingerPrints();
            if (ServerFingerPrintsContainer.Instance.HasFingerPrintsInfo)
            {
                CachedEntitesPreloader.Instance.DoWork();
            }

            UpdateManager.INSTANCE.FullUpdate(cancelToken);
        }

        public HttpWebRequest CreateRequest(string serviceName, string methodName, Guid callId, TimeSpan? requestTimeout)
        {
            string url = $"{ServerUrl}/services/{serviceName}?methodName={methodName}";
            var request = (HttpWebRequest)WebRequest.Create(url);

            SetCommonHeaders(request, callId, requestTimeout);
            SetCustomHeaders(request);

            return request;
        }

        private void SetCommonHeaders([NotNull] HttpWebRequest request, Guid callId, TimeSpan? requestTimeout)
        {
            request.ServicePoint.ConnectionLimit = 32;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.ContentType = "text/xml";
            request.Method = "POST";
            request.CookieContainer = container;
            request.Headers[RPCHeaders.CORRELATION_ID.Name] = callId.ToString();
            request.Headers[RPCHeaders.LOGIN.Name] = loginName;
            request.Headers[RPCHeaders.PASSWORD_HASH.Name] = passwordHash;
            request.Headers[RPCHeaders.BACK_VERSION.Name] = VersionInfo.String;
            request.Headers[RPCHeaders.AUTH_TYPE.Name] = ClientType;
            request.Headers[RPCHeaders.EDITION.Name] = Edition.CurrentEdition.ToString();
            request.Timeout = (int?)requestTimeout?.TotalMilliseconds ?? Settings.Default.SERVICE_METHOD_TIMEOUT_MILLISECONDS;
            if (LanguageCode != null)
            {
                request.Headers["Accept-Language"] = LanguageCode;
            }
            request.KeepAlive = CommonConfig.Instance.KeepAliveHttpWebRequestFlag;
        }

        protected virtual void SetCustomHeaders([NotNull] HttpWebRequest request)
        { }

        public string ServerUrl { get; }

        public string ClientType { get; }

        public ISynchronizer SynchronizeInvoke { get; }

        public ServerFingerPrintsInfo ServerFingerPrintsInfo { get; private set; }

        public User GetCurrentUser()
        {
            return currentUser;
        }

        /// <summary>
        /// Метод добавлен для работы некоторых тестов
        /// После стабилизации и изменения ServerSession
        /// он будет удален
        /// </summary>
        /// <param name="curUser"></param>
        public void SetCurrentUser(User curUser)
        {
            currentUser = curUser;
        }

        public string LanguageCode { get; set; }

        private static string defaultIntegrationUserPassword;

        public static string GetDefaultIntegrationUserPassword()
        {
            if (!String.IsNullOrEmpty(defaultIntegrationUserPassword))
            {
                return defaultIntegrationUserPassword;
            }

            var macAddresses = NetworkInterface.GetAllNetworkInterfaces()
               .Select(nif => nif.GetPhysicalAddress().GetAddressBytes())
               .Where(addressBytes => addressBytes.Length > 0)
               .Select(macAddress => macAddress.Select(addrPart => addrPart.ToString("x2").ToUpper()).Join("-"))
               .ToList();

            if (macAddresses.Count == 0)
            {
                return "resto#test";
            }
            macAddresses.Sort();

            var sb = new StringBuilder(100);
            macAddresses.ForEach(macAddr => sb.Append(macAddr));
            defaultIntegrationUserPassword = MathUtil.HexSHA1(sb.ToString());
            if (defaultIntegrationUserPassword.Length > 10)
            {
                defaultIntegrationUserPassword = defaultIntegrationUserPassword.Substring(0, 10);
            }
            return defaultIntegrationUserPassword;
        }
    }
}
