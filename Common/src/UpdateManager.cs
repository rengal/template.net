using System;
using System.Threading;
using Resto.Framework.Src;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Resto.Common;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public interface IUpdateManager
    {
        /// <summary>
        /// Статус подключения
        /// true - подключение к серверу установлено
        /// false - подключение к серверу не установлено
        /// null - статус не ясен
        /// </summary>
        bool? IsConnected { get; }
        event AsyncErrorHandler ConnectionStatusChanged;
        event EntitiesUpdateFinished UpdateFinished;
        void SetServerTimeout(int fullTimeOut, int serverTimeoutMilliseconds);
        void Reset();
        void BeginListen();
        void StopListen();
        void FullUpdate(CancellationToken? cancelToken = null);
        void ProcessUpdates(ParsedEntitiesUpdate update);
    }

    public static class UpdateManager
    {
        private static readonly object syncObject = new object();
        #region Singleton

        private static IUpdateManager? instance;
        public static IUpdateManager INSTANCE
        {
            get
            {
                lock (syncObject)
                {
                    if (instance != null)
                        return instance;
                    instance = ServiceProviderExtensions.GetService<IUpdateManager>();
                    return instance;

                }
            }
            set
            {
                lock (syncObject)
                {
                    instance = value;
                }
            }
        }

        #endregion

        public static void ResetInstance()
        {
            // lock (syncObject)
            // {
            //     UnityHelper.GetFactoryContainer().Teardown(instance);
            //     instance = UnityHelper.GetFactoryContainer().Resolve<IUpdateManager>();
            // } //todo debugnow
        }
    }

    public sealed class RMSUpdateManager : IUpdateManager
    {
        #region Fields / Properties
        private static bool connectionError;
        private static readonly ILog Log = LogFactory.Instance.GetLogger<RMSUpdateManager>();
        private static readonly TimeSpan DelayOnError = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan TimeoutOnError = TimeSpan.Zero;
        private static readonly TimeSpan MinDelay = TimeSpan.FromSeconds(1);

        private TimeSpan entitiesUpdateTimeout = TimeSpan.FromSeconds(30);
        private TimeSpan serverTimeoutCorrect;

        private TimeStamp lastListenTime;
        private bool cancel;
        private bool listened;

        public event AsyncErrorHandler ConnectionStatusChanged;
        public event EntitiesUpdateFinished UpdateFinished;
        #endregion

        public RMSUpdateManager()
        {
            Reset();
        }

        public bool? IsConnected { get; private set; }

        private void OnConnectionStatusChanged(bool error, Exception ex)
        {
            IsConnected = !error;
            if (ConnectionStatusChanged != null)
                ConnectionStatusChanged(error, ex);

            // если было отключение сервера connectionError = true, а потом связь восстановилась error = false
            if (!error && connectionError && CommonConfig.Instance.UpdateCacheAfterReconnection)
            {
                // попытка обновить кэш
                FullUpdate();
            }

            connectionError = error;
        }

        public void FullUpdate(CancellationToken? cancelToken = null)
        {
            Reset();
            CachedEntitesPreloader.Instance.WaitForFinish();
            if (CachedEntitesPreloader.Instance.Ok && CachedEntitesPreloader.Instance.Update != null)
            {
                Log.Info("Cached service instance id: " + CachedEntitesPreloader.Instance.Update.ServerInstanceId);
                Log.Info("Cached revision: " + CachedEntitesPreloader.Instance.Update.Revision);
                if (CachedEntitesPreloader.Instance.Update.ServerInstanceId.Equals(Authorize(cancelToken)))
                {
                    Log.Info("Trying to load entities from cache");
                    ProcessUpdates(CachedEntitesPreloader.Instance.Update);
                    Log.Info("Entities loaded from cache");
                }
                else
                {
                    Log.Info("Cache is out of date");
                }
            }
            Log.Info("Starting loading data from server");
            ServiceClientFactory.UpdateService.WaitEntitiesUpdate(EntityManager.INSTANCE.EntitiesUpdateRevision, 0, true).CallSync();
            Log.Info("End loading data from server");
        }

        /// <param name="cancelTokenNullable">Токен отмены, если задан.
        /// <para>
        /// Вызов <see cref="IAuthorizationService.Authorize()"/> может зависнуть при определенных проблемах на сервере.
        /// Чтобы не блокировался навсегда <see cref="ServerSession.mutex"/>, при вызове <see cref="FullUpdate"/> может использоваться токен отмены.
        /// Сама отмена инициируется, когда пользователь нажимает кнопку "Отмена" в диалоге входа в систему 
        /// во время осуществления попытки входа.
        /// </para>
        /// </param>
        private Guid? Authorize(CancellationToken? cancelTokenNullable = null)
        {
            var call = ServiceClientFactory.AuthorizationService.Authorize();

            if (cancelTokenNullable == null)
            {
                return call.CallSync();
            }

            var cancelToken = cancelTokenNullable.Value;

            cancelToken.ThrowIfCancellationRequested();

            Guid? guid = null;
            bool error = false;
            Exception exception = null;

            var manualResetEvent = new ManualResetEventSlim(false);

            call.CallAsync(result =>
                {
                    guid = result;
                },
                (err, exc) =>
                {
                    error = err;
                    exception = exc;
                    manualResetEvent.Set();
                });

            manualResetEvent.Wait(cancelToken);

            cancelToken.ThrowIfCancellationRequested();

            if (exception != null)
            {
                throw exception;
            }
            if (error)
            {
                throw new RestoException("Unknown server call error.");
            }

            return guid;
        }

        public void SetServerTimeout(int fullTimeOut, int serverTimeoutMilliseconds)
        {
            if (fullTimeOut > 0)
                entitiesUpdateTimeout = TimeSpan.FromMilliseconds(fullTimeOut);

            var serverTimeout = TimeSpan.FromMilliseconds(serverTimeoutMilliseconds);
            serverTimeoutCorrect = serverTimeout >= TimeSpan.Zero && entitiesUpdateTimeout > serverTimeout
                ? entitiesUpdateTimeout - serverTimeout
                : TimeSpan.Zero;
        }

        public void Reset()
        {
            cancel = false;
            listened = false;
            EntityManager.INSTANCE.Reset();
        }

        public void BeginListen()
        {
            if (listened)
                return;

            Log.Info("Begin listen for updates.");
            cancel = false;
            listened = true;
            CallListenAsync(ServerSession.CurrentSession, TimeSpan.Zero, entitiesUpdateTimeout);
        }

        public void StopListen()
        {
            cancel = true;
            listened = false;
            Log.Info("Stop listen for updates.");
        }

        public void ProcessUpdates(ParsedEntitiesUpdate update)
        {
            if (cancel)
                return;

            //Нужно передать dataUpdate в EntityManager, даже если он пустой - могла измениться ревизия сервера
            //Реальная ревизия сервера используется при вызове сервисов сервера, использующих БД, чтобы убедиться, что все изменения, 
            //сделанные данным клиентом, сохранены
            var updateSucceeded = EntityManager.INSTANCE.OnDataUpdate(update);

            if (updateSucceeded)
            {
                FireUpdateFinished();
            }
        }

        private void FireUpdateFinished()
        {
            try
            {
                var handlers = UpdateFinished;
                if (handlers != null)
                    handlers();
            }
            catch (Exception e)
            {
                Log.Warn("Exception in UpdateFinished listener", e);
            }
        }

        private delegate void ListenDelegate(ServerSession listenSession, TimeSpan delay, TimeSpan timeout);

        private void CallListenAsync(ServerSession listenSession, TimeSpan delay, TimeSpan timeout)
        {
            new ListenDelegate(CallListenNow).BeginInvoke(listenSession, delay, timeout, null, null);
        }

        private void CallListenNow(ServerSession listenSession, TimeSpan delay, TimeSpan timeout)
        {
            if (!ServerSession.IsConnected || listenSession != ServerSession.CurrentSession)
                return;

            Log.Info(string.Format("Sleep {0}", delay));

            Thread.Sleep(TimeSpanUtils.Max(TimeSpan.Zero, delay));
            if (cancel)
                return;

            lastListenTime = TimeStamp.Now();

            var callTimeout = TimeSpanUtils.Max(TimeSpan.Zero, timeout - serverTimeoutCorrect);

            Log.Info(string.Format("Send request (lastListenTime: {0}, callTimeout: {1})", lastListenTime, callTimeout));

            ServiceClientFactory.UpdateService.WaitEntitiesUpdate(EntityManager.INSTANCE.EntitiesUpdateRevision, (int)callTimeout.TotalMilliseconds, true,
                     delegate(bool error, Exception ex)
                     {
                         if (cancel)
                             return;
                         if (!ServerSession.IsConnected || listenSession != ServerSession.CurrentSession)
                             return;
                         if (error)
                             CallListenAsync(listenSession, DelayOnError, TimeoutOnError);
                         OnConnectionStatusChanged(error, ex);
                     },
                     delegate
                     {
                         if (cancel)
                             return;
                         if (!ServerSession.IsConnected || listenSession != ServerSession.CurrentSession)
                             return;

                         var timeLeftBeforeNextCall = MinDelay - (TimeStamp.Now() - lastListenTime);
                         var delayNext = timeLeftBeforeNextCall > TimeSpan.Zero
                             ? TimeSpanUtils.Max(TimeSpan.Zero, timeLeftBeforeNextCall + serverTimeoutCorrect)
                             : TimeSpan.Zero;

                         CallListenAsync(listenSession, delayNext, entitiesUpdateTimeout);
                     }
                );
        }
    }
}