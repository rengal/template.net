using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.WatchDog;
using Resto.Framework.Common;
using Resto.Framework.Common.XmlSerialization;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Data;
using log4net;

namespace Resto.Data
{
    /// <remarks>
    /// Творчески скопипащено в код доставочного плагина старого КЦ: dev\iikoFront.Net\Resto.Front.Api.Delivery\ServerCallsDeliveryImpl\Call.cs
    /// </remarks>
    public interface IServiceMethodCall<out T>
    {
        T CallSync(IDeserializationContext cache);
        T CallSync(IDeserializationContext cache, TimeSpan timeout);
        T CallSync(TimeSpan? timeout = null);
        /// <summary>
        /// Метод производит асинхронный вызов серверного метода
        /// </summary>
        /// <param name="handler">Обработчик результатов обращения к серверу, вызывается в GUI-потоке</param>
        /// <param name="errorHandler">Обработчик серверных ошибок</param>
        /// <param name="context">Контекст десериализации</param>
        IAsyncCallResult CallAsync(AsyncResultHandler<T> handler, AsyncErrorHandler errorHandler, IDeserializationContext context = null);
        /// <summary>
        /// Метод производит асинхронный вызов серверного метода
        /// </summary>
        /// <param name="handler">Обработчик результатов обращения к серверу, вызывается в GUI-потоке</param>
        /// <param name="errorHandler">Обработчик серверных ошибок</param>
        /// <param name="warningHandler">Обработчик серверных ошибок, помеченных как warning</param>
        /// <param name="context">Контекст десериализации</param>
        IAsyncCallResult CallAsync(AsyncResultHandler<T> handler, AsyncErrorHandler errorHandler, AsyncWarningHandler warningHandler, IDeserializationContext context = null);
    }

    /// <summary>
    /// Результат вызова асинхронной операции.
    /// </summary>
    /// <remarks>
    /// Содержит информацию об асинхронном вызове и позволяет производить операции над ним (например, вызывать его отмену).
    /// Не содержит информации о фактическом результате выполнения операции (возвращаемом значении).
    /// </remarks>
    public interface IAsyncCallResult
    {
        /// <summary>
        /// Делает на сервер асинхронный вызов отмены операции
        /// </summary>
        /// <param name="errorHandler">Обработчик ошибки отмены операции</param>
        void CancelAsync(AsyncErrorHandler errorHandler);
    }

    /// <summary>
    /// Интерфейс хелпера асинхронных вызовов к серверу, возвращающих значение типа <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
    public interface IAsyncCallHelper<T>
    {
        T Value { get; }
        bool Call(IServiceMethodCall<T> call, bool enableWarnings = true);
    }

    internal class ServiceMethodCall<T> : IServiceMethodCall<T>
    {
        private readonly RemoteMethodCaller caller;
        private readonly object[] args;
        private readonly Guid callId;

        public ServiceMethodCall(RemoteMethodCaller caller, object[] args)
        {
            this.caller = caller;
            this.args = args;
            callId = Guid.NewGuid();
        }

        public T CallSync(IDeserializationContext cache)
        {
            return caller.Call<T>(cache, callId, null, args);
        }

        public T CallSync(IDeserializationContext cache, TimeSpan timeout)
        {
            return caller.Call<T>(cache, callId, timeout, args);
        }

        /// <summary>
        /// Синхронный вызов удаленного метода сервера
        /// </summary>
        /// <exception cref="RestoServiceConnectionException">
        /// Возникает, когда в процессе вызова связь с сервером потеряна.
        /// </exception>
        /// <exception cref="RestoServiceInternalException">
        /// При вызове сервисного метода произошла внутренняя ошибка/исключение.
        /// </exception>
        /// <exception cref="RestoServiceException">
        /// Общее исключение удаленного вызова.
        /// </exception>
        public T CallSync(TimeSpan? timeout = null)
        {
            return caller.Call<T>(callId, timeout, args);
        }

        public IAsyncCallResult CallAsync(AsyncResultHandler<T> handler, AsyncErrorHandler errorHandler, IDeserializationContext context = null)
        {
            return CallAsync(handler, errorHandler, null, context);
        }

        public IAsyncCallResult CallAsync(AsyncResultHandler<T> handler, [CanBeNull] AsyncErrorHandler errorHandler, AsyncWarningHandler warningHandler, IDeserializationContext context = null)
        {
            caller.CallAsync<T>(
                args,
                delegate (object[] values)
                {
                    handler?.Invoke((T)values[0]);
                },
                delegate (bool error, Exception exception)
                {
                    if (warningHandler != null && error && exception is RestoServiceException && ((RestoServiceException)exception).IsWarning)
                    {
                        var repeat = warningHandler((RestoServiceException)exception);
                        if (repeat)
                        {
                            CallAsync(handler, errorHandler, context);
                        }
                    }
                    else
                    {
                        errorHandler?.Invoke(error, exception);
                    }
                },
                warningHandler != null,
                callId,
                context
            );
            return new AsyncCallResult(callId);
        }

        private class AsyncCallResult : IAsyncCallResult
        {
            private readonly Guid callId;

            public AsyncCallResult(Guid callId)
            {
                this.callId = callId;
            }

            public void CancelAsync(AsyncErrorHandler errorHandler)
            {
                ServiceClientFactory.SystemService.CancelRequestById(callId)
                    .CallAsync(null, (error, exception) => { errorHandler?.Invoke(error, exception); });
            }
        }
    }

    public delegate void AsyncResultHandler(params object[] values);
    public delegate void AsyncResultHandler<in T>(T result);
    public delegate bool AsyncWarningHandler(RestoServiceException exception);
    public delegate void AsyncErrorHandler(bool error, Exception exception);

    public sealed class RemoteMethodCaller
    {
        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(RemoteMethodCaller));

        private static ILog Log => logWrapper.Log;

        private readonly string serviceName;
        private readonly string methodName;
        private readonly ArgumentsSerializer requestHelper;

        public RemoteMethodCaller(string serviceName, string methodName)
        {
            this.serviceName = serviceName;
            this.methodName = methodName;
            requestHelper = new ArgumentsSerializer(serviceName + "." + methodName);
            foreach (var arg in SpecialArg.VALUES)
            {
                requestHelper.AddArgument(arg.Name, arg.Type);
            }
        }

        public void AddArg(string name, Type type)
        {
            requestHelper.AddArgument(name, type);
        }

        public IServiceMethodCall<T> CreateCall<T>(params object[] args)
        {
            return new ServiceMethodCall<T>(this, args);
        }

        internal T Call<T>(IDeserializationContext cache, Guid callId, TimeSpan? timeout = null, params object[] args)
        {
            var session = ServerSession.CurrentSession;
            try
            {
                var result = GetServiceResult<T>(session, args, false, callId, cache, timeout);

                if (!result.Success)
                    throw GetException(result, args);

                ProcessSideEffects(result, session.SynchronizeInvoke);
                return result.ReturnValue;
            }
            catch (WebException e)
            {
                Log.WarnFormat("Service call failed {0}.{1}: {2} ({3})", serviceName, methodName, e.Message, e.Status);
                throw RestoServiceConnectionException.Create(e);
            }
            catch (IOException e)
            {
                Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, e.Message);
                throw new RestoServiceConnectionException(ConnectionResult.CONNECTION_ERROR, e);
            }
            catch (SerializerException e) // такое возможно, если вместо сервера нам ответит прокси-заглушка
            {
                Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, e.Message);
                throw new RestoServiceConnectionException(ConnectionResult.CONNECTION_ERROR, e);
            }
            catch (Exception e)
            {
                Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, e.Message);
                throw;
            }
        }

        internal T Call<T>(Guid callId, TimeSpan? timeout = null, params object[] args)
        {
            return Call<T>(new DeserializationContext(EntityManager.EntitiesProvider), callId, timeout, args);
        }

        private WebRequest SendRequest(ServerSession session, object[] args, bool enableWarnings, Guid callId, TimeSpan? timeout)
        {
            var request = session.CreateRequest(serviceName, methodName, callId, timeout);
            WriteRequestArguments(request.GetRequestStream(), args, callId, session.ClientType, enableWarnings);
            return request;
        }

        internal void CallAsync<T>(object[] args, AsyncResultHandler handler, AsyncErrorHandler errorHandler, Guid callId, IDeserializationContext context)
        {
            CallAsync<T>(args, handler, errorHandler, false, callId, context);
        }

        public void CallAsync(object[] args, Action resultHandler, AsyncErrorHandler errorHandler, IDeserializationContext context = null)
        {
            AsyncResultHandler rh = null;
            if (resultHandler != null)
            {
                rh = (returnValues => resultHandler());
            }
            CallAsync<object>(args, rh, errorHandler, Guid.NewGuid(), context);
        }

        public void CallAsync<T>(object[] args, Action<T> resultHandler, AsyncErrorHandler errorHandler, IDeserializationContext context = null)
        {
            AsyncResultHandler rh = null;
            if (resultHandler != null)
            {
                rh = (returnValues => resultHandler((T)returnValues[0]));
            }
            CallAsync<T>(args, rh, errorHandler, Guid.NewGuid(), context);
        }

        public void CallAsync<T>(object[] args, AsyncResultHandler handler, AsyncErrorHandler errorHandler, bool enableWarnings, Guid callId, IDeserializationContext context = null)
        {
            ServerSession session = ServerSession.CurrentSession;
            ISynchronizer synchronizeInvoke = session.SynchronizeInvoke;
            ThreadPool.QueueUserWorkItem(delegate
            {
                Exception exception = null;
                try
                {
                    var result = GetServiceResult<T>(session, args, enableWarnings, callId, context ?? new DeserializationContext(EntityManager.EntitiesProvider));

                    if (result.Success)
                    {
                        ProcessSideEffects(result, session.SynchronizeInvoke);
                        object returnValue = result.ReturnValue;
                        object[] outArgs = { returnValue };
                        if (handler != null)
                        {
                            if (synchronizeInvoke != null && synchronizeInvoke.InvokeRequired)
                            {
                                synchronizeInvoke.BeginInvoke(handler, outArgs);
                            }
                            else
                            {
                                handler(outArgs);
                            }
                        }
                    }
                    else
                    {
                        exception = GetException(result, args);
                        Log.Warn("Service call failed", exception);
                    }
                }
                catch (WebException e)
                {
                    exception = RestoServiceConnectionException.Create(e);
                    Log.WarnFormat("Service call failed {0}.{1}: {2} ({3})", serviceName, methodName, e.Message, e.Status);
                }
                catch (IOException e)
                {
                    exception = new RestoServiceConnectionException(ConnectionResult.CONNECTION_ERROR, e);
                    Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, e.Message);
                }
                catch (SerializerException e) // такое возможно, если вместо сервера нам ответит прокси-заглушка
                {
                    exception = new RestoServiceConnectionException(ConnectionResult.CONNECTION_ERROR, e);
                    Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, e.Message);
                }
                catch (Exception e)
                {
                    exception = e;
                    Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, e);
                }
                finally
                {
                    CallErrorHandler(errorHandler, synchronizeInvoke, exception);
                }
            });

        }

        public void CallAsyncWithNoWait()
        {
            ServerSession session = ServerSession.CurrentSession;
            ThreadPool.QueueUserWorkItem(delegate
            {
                Exception exception = null;
                try
                {
                    SendRequest(session, Array.Empty<object>(), false, Guid.NewGuid(), null).BeginGetResponse(null, null);
                }
                catch (Exception e)
                {
                    exception = e is WebException ? RestoServiceConnectionException.Create((WebException)e) : e;
                }
                finally
                {
                    if (exception != null)
                    {
                        Log.WarnFormat("Service call failed: {0}", exception is RestoServiceConnectionException ? exception.Message : exception.ToString());
                    }
                }
            });
        }

        [NotNull]
        public Task<TResult> Call<TResult>([NotNull] object[] args, [CanBeNull] object state, TimeSpan? requestTimeout)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var callId = Guid.NewGuid();
            var callHash = callId.GenerateShortHash();

            var session = ServerSession.CurrentSession;
            var request = session.CreateRequest(serviceName, methodName, callId, requestTimeout);

            Log.InfoFormat("Server method called: {0} (callId: {1} callHash: {2})", methodName, callId, callHash);
            var result = Observable.FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                .Do(requestStream => WriteRequestArguments(requestStream, args, callId, session.ClientType))
                .SelectMany(_ => Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)())
                .Select(response => ProcessResponse<TResult>(response, request.RequestUri.OriginalString, args, session.SynchronizeInvoke))
                .ToTask()
                .ContinueWith<TResult>(WrapExceptions, state);
            Log.InfoFormat("Response received from server method: {0} (callId: {1} callHash: {2})", methodName, callId, callHash);

            var timeoutSubscription = ThreadPool.RegisterWaitForSingleObject(((IAsyncResult)result).AsyncWaitHandle, HandleAsyncRequestTimeout, request, request.Timeout, true);
            result.ContinueWith(_ => timeoutSubscription.Unregister(null));
            return result;
        }

        private static void HandleAsyncRequestTimeout(object state, bool timedOut)
        {
            if (timedOut)
                ((HttpWebRequest)state).Abort();
        }

        /// <summary>
        /// Формирует запрос к серверу и возвращает результат.
        /// При необходимости выполняет повторный запрос.
        /// </summary>
        private ServiceResult<T> GetServiceResult<T>(ServerSession session, object[] args, bool enableWarnings, Guid callId, IDeserializationContext context, TimeSpan? timeout = null)
        {
            WebException lastException = null;
            for (var repeatCount = 0; repeatCount < CommonConfig.Instance.RepeatCallsServerCount + 1; repeatCount++)
            {
                try
                {
                    var request = SendRequest(session, args, enableWarnings, callId, timeout);
                    var callHash = callId.GenerateShortHash();
                    Log.InfoFormat("Server method called: {0} (callId: {1} callHash: {2})", methodName, callId, callHash);
                    var response = request.GetResponse();
                    Log.InfoFormat("Response received from server method: {0} (callId: {1} callHash: {2})", methodName, callId, callHash);
                    return ReadResponse<T>(response, context, request.RequestUri.OriginalString);
                }
                catch (WebException ex)
                {
                    Log.WarnFormat("WebException status: {0}. Repeat count: {1}.", ex.Status, repeatCount);

                    lastException = ex;
                    if (!ex.Status.In(WebExceptionStatus.SendFailure, WebExceptionStatus.ReceiveFailure))
                        throw;
                    Thread.Sleep(CommonConfig.Instance.RepeatCallsServerTimeoutInMs);
                }
            }
            if (lastException != null)
                throw lastException;
            throw new RestoServiceException("Can not get result.");
        }

        private static void CallErrorHandler(AsyncErrorHandler errorHandler, ISynchronizer synchronizeInvoke, [CanBeNull] Exception exception)
        {
            if (errorHandler != null)
            {
                var failed = exception != null;
                if (synchronizeInvoke != null)
                {
                    try
                    {
                        synchronizeInvoke.BeginInvoke(errorHandler, failed, exception);
                    }
                    catch
                    {
                        errorHandler(failed, exception);
                    }
                }
                else
                {
                    errorHandler(failed, exception);
                }
            }
        }

        /// <remarks>См. также форматирование удаленного стектрейса в resto.RestoRemoteException</remarks>
        private static Exception GetException<T>(ServiceResult<T> result, object[] arguments)
        {
            string message = result.ErrorString;
            if (result.StackTrace != null)
            {
                var builder = new StringBuilder();
                builder.Append("(");
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (i > 0) builder.Append(", ");
                    builder.Append(arguments[i]);
                }
                builder.Append(")");
                message += string.Format("{0}{1}{0}\tREMOTE CALL: {2} {3}",
                    Environment.NewLine, result.StackTrace, result.RequestUri, builder);
            }

            switch (result.ResultStatus)
            {
                case ServiceResultStatus.DISPLAYABLE_ERROR:
                case ServiceResultStatus.RECOVERABLE_WARNING:
                    return new RestoServiceException(message, result.ErrorString)
                    {
                        IsWarning = result.ResultStatus == ServiceResultStatus.RECOVERABLE_WARNING
                    };
                case ServiceResultStatus.SYSTEM_ERROR:
                    return new RestoServiceInternalException(message);
                default:
                    throw new InvalidEnumArgumentException("Illegal error status: " + result.ResultStatus);
            }
        }

        private void WriteRequestArguments([NotNull] Stream requestStream, [NotNull] object[] requestArguments, Guid callId, string clientType, bool enableWarnings = false)
        {
            if (requestStream == null)
                throw new ArgumentNullException(nameof(requestStream));
            if (requestArguments == null)
                throw new ArgumentNullException(nameof(requestArguments));

            var writer = new XmlTextWriter(requestStream, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteStartElement("args");

            requestHelper.WriteArguments(writer, CollectRequestArguments(requestArguments, callId, clientType, enableWarnings), new DefaultSerializationContext());

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        private static T ProcessResponse<T>([NotNull] WebResponse response, string requestUri, [NotNull] object[] requestArguments, [CanBeNull] ISynchronizer synchronizeInvoke)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            if (requestArguments == null)
                throw new ArgumentNullException(nameof(requestArguments));

            var serviceResult = ReadResponse<T>(response, new DeserializationContext(EntityManager.EntitiesProvider), requestUri);
            if (!serviceResult.Success)
                throw GetException(serviceResult, requestArguments);

            ProcessSideEffects(serviceResult, synchronizeInvoke);
            return serviceResult.ReturnValue;
        }

        [NotNull]
        private static ServiceResult<T> ReadResponse<T>([NotNull] WebResponse response, [NotNull] IDeserializationContext context, string requestUri)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            using (response)
            using (var responseStream = response.GetResponseStream())
            // ReSharper disable once AssignNullToNotNullAttribute https://stackoverflow.com/a/16911086
            using (var reader = new XmlTextReader(responseStream))
            {
                var serviceResult = Serializer.Deserialize<ServiceResult<T>>(reader, context, false);
                serviceResult.RequestUri = requestUri;
                return serviceResult;
            }
        }

        private T WrapExceptions<T>([NotNull] Task<T> resultTask, object state)
        {
            if (resultTask == null)
                throw new ArgumentNullException(nameof(resultTask));

            var aggregateException = resultTask.Exception;
            if (aggregateException == null)
                return resultTask.Result;

            var flattenedException = aggregateException.Flatten();
            if (flattenedException.InnerExceptions.Count == 1)
            {
                var innerException = flattenedException.InnerException;

                if (innerException is WebException webException)
                {
                    Log.WarnFormat("Service call failed {0}.{1}: {2} ({3})", serviceName, methodName, webException.Message, webException.Status);
                    throw RestoServiceConnectionException.Create(webException);
                }

                if (innerException is IOException ioException)
                {
                    Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, ioException.Message);
                    throw new RestoServiceConnectionException(ConnectionResult.CONNECTION_ERROR, ioException);
                }

                if (innerException is SerializerException serializerException)
                {
                    Log.WarnFormat("Service call failed {0}.{1}: {2}", serviceName, methodName, serializerException.Message);
                    throw new RestoServiceConnectionException(ConnectionResult.CONNECTION_ERROR, serializerException);
                }

                Log.Error("Unexpected exception", innerException);
                throw innerException.PrepareForRethrow();
            }

            Log.Error("Unexpected exception", flattenedException);
            throw flattenedException.PrepareForRethrow();
        }

        [NotNull]
        private static object[] CollectRequestArguments([NotNull] object[] explicitArguments, Guid callId, string clientType, bool enableWarnings)
        {
            if (explicitArguments == null)
                throw new ArgumentNullException(nameof(explicitArguments));

            var implicitArguments = new Dictionary<SpecialArg, object>();

            if (RequestEntityUpdatesAsServiceCallSideEffect)
            {
                implicitArguments[SpecialArg.ENTITIES_VERSION] = EntityManager.INSTANCE.EntitiesUpdateRevision;
            }

            implicitArguments[SpecialArg.CLIENT_TYPE] = clientType;
            implicitArguments[SpecialArg.ENABLE_WARNINGS] = enableWarnings;
            implicitArguments[SpecialArg.CLIENT_CALL_ID] = callId;
            implicitArguments[SpecialArg.USE_RAW_ENTITIES] = true;

            if (RequestWatchDogCheckResultsAsServiceCallSideEffect)
            {
                implicitArguments[SpecialArg.REQUEST_WATCHDOG_CHECK_RESULTS] = true;
            }

            AfterBuildSpecialArgs?.Invoke(null, new EventArgs<Dictionary<SpecialArg, object>>(implicitArguments));

            var specialArguments = SpecialArg.VALUES;
            var methodArguments = new object[specialArguments.Length + explicitArguments.Length];
            Array.Copy(explicitArguments, 0, methodArguments, specialArguments.Length, explicitArguments.Length);
            for (var i = 0; i < specialArguments.Length; i++)
            {
                var arg = specialArguments[i];
                if (implicitArguments.TryGetValue(arg, out var value))
                    methodArguments[i] = value;
            }

            return methodArguments;
        }

        private static void ProcessSideEffects<T>([NotNull] ServiceResult<T> serviceResult, [CanBeNull] ISynchronizer synchronizeInvoke)
        {
            if (serviceResult == null)
                throw new ArgumentNullException(nameof(serviceResult));

            if (serviceResult.LicenseInfo != null && DoUpdateLicenseInfo != null)
                DoUpdateLicenseInfo(serviceResult.LicenseInfo);

            if (RequestEntityUpdatesAsServiceCallSideEffect && serviceResult.EntitiesUpdate != null)
            {
                if (synchronizeInvoke != null && synchronizeInvoke.InvokeRequired)
                {
                    synchronizeInvoke.BeginInvoke((Action<EntitiesUpdate>)DefaultDoUpdateEntities, serviceResult.EntitiesUpdate).WaitForEnd();
                }
                else
                {
                    DefaultDoUpdateEntities(serviceResult.EntitiesUpdate);
                }
            }

            if (RequestWatchDogCheckResultsAsServiceCallSideEffect && serviceResult.WatchDogCheckResults != null)
            {
                ProcessWatchDogResults(serviceResult.WatchDogCheckResults);
            }
        }

        /// <summary>
        /// Определяет, будут ли обновления rms-объектов запрашиваться при вызове любых сервисных методов.
        /// BackOffice желает получать обновления при вызове любых сервисных методов, на фронте обновления запрашиваются явно и в виде побочного эффекта не нужны.
        /// </summary>
        public static bool RequestEntityUpdatesAsServiceCallSideEffect = true;

        /// <summary>
        /// Определяет, будут ли при каждом вызове любого сервисного метода запрашиваться результаты проверок WatchDog <see cref="CheckResult"/>.
        /// </summary>
        public static bool RequestWatchDogCheckResultsAsServiceCallSideEffect { get; set; }

        private static void DefaultDoUpdateEntities(EntitiesUpdate update)
        {
            //Вызываем через update manager, чтобы изменить дату последнего обновления
            UpdateManager.INSTANCE.ProcessUpdates(EntityManager.INSTANCE.ParseUpdate(update));
        }

        private static void ProcessWatchDogResults(ICollection<CheckResult> checkResults)
        {
            WatchDogCheckResultsManager.Instance?.OnCheckResultsReceived(checkResults);
        }

        public static Action<LicenseInfoResult> DoUpdateLicenseInfo;

        public static event EventHandler<EventArgs<Dictionary<SpecialArg, object>>> AfterBuildSpecialArgs;
    }

    public class ServiceResult<T>
    {
        private readonly T returnValue;
        private bool success = false;
        private readonly ServiceResultStatus resultStatus;
        private readonly string errorString;
        private readonly string stackTrace;
        private readonly EntitiesUpdate entitiesUpdate;
        private readonly LicenseInfoResult licenseInfo;
        private readonly Collection<CheckResult> watchDogCheckResults;
        private string requestUri;

        public ServiceResult() { }

        public ServiceResult(T returnValue, string errorString, ServiceResultStatus resultStatus,
            string stackTrace, EntitiesUpdate entitiesUpdate, LicenseInfoResult licenseInfo, Collection<CheckResult> watchDogCheckResults)
        {
            this.returnValue = returnValue;
            this.errorString = errorString;
            this.resultStatus = resultStatus;
            this.stackTrace = stackTrace;
            this.entitiesUpdate = entitiesUpdate;
            this.licenseInfo = licenseInfo;
            this.watchDogCheckResults = watchDogCheckResults;
        }

        public T ReturnValue
        {
            get { return returnValue; }
        }

        public bool Success
        {
            get { return success; }
        }

        public ServiceResultStatus ResultStatus
        {
            get { return resultStatus; }
        }

        public string ErrorString
        {
            get { return errorString; }
        }

        public string StackTrace
        {
            get { return stackTrace; }
        }

        public EntitiesUpdate EntitiesUpdate
        {
            get { return entitiesUpdate; }
        }

        public LicenseInfoResult LicenseInfo
        {
            get { return licenseInfo; }
        }

        /// <summary>
        /// Отрицательные результаты проверок WatchDog 
        /// </summary>
        public ICollection<CheckResult> WatchDogCheckResults
        {
            get { return watchDogCheckResults; }
        }

        public string RequestUri
        {
            get { return requestUri; }
            set { requestUri = value; }
        }
    }
}