using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.WcfCommon.Internationalization;

namespace Resto.Framework.Src.WcfCommon
{
    public static class ServiceCaller
    {
        private static readonly ConcurrentDictionary<ConnectionInfo, IChannelFactory> ChannelFactoryCache =
            new ConcurrentDictionary<ConnectionInfo, IChannelFactory>();

        #region Call Action overloads

        public static void Call<TServiceContract>([NotNull] Action<TServiceContract> serviceCall,
            [CanBeNull] string userName, [CanBeNull] string password)
            where TServiceContract : class
        {
            Call(serviceCall, userName, password, null);
        }

        public static void Call<TServiceContract>([NotNull] Action<TServiceContract> serviceCall)
            where TServiceContract : class
        {
            Call(serviceCall, null, null, null);
        }

        public static void Call<TServiceContract>([NotNull] Action<TServiceContract> serviceCall,
            [CanBeNull] string userName, [CanBeNull] string password, TimeSpan? timeout)
            where TServiceContract : class
        {
            Call(serviceCall, null, null, userName, password, timeout);
        }

        public static void Call<TServiceContract>([NotNull] Action<TServiceContract> serviceCall,
             [CanBeNull] string hostName, [CanBeNull] string endpointDnsIdentity, [CanBeNull] string userName, [CanBeNull] string password, TimeSpan? timeout)
            where TServiceContract : class
        {
            Call<TServiceContract, bool>(service =>
            {
                serviceCall(service);
                return true;
            }, hostName, endpointDnsIdentity, userName, password, timeout);            
        }
        
        #endregion

        #region Call Func overloads

        public static TResult Call<TServiceContract, TResult>([NotNull] Func<TServiceContract, TResult> serviceCall,
            [CanBeNull] string userName, [CanBeNull] string password)
            where TServiceContract : class
        {
            return Call(serviceCall, userName, password, null);
        }

        public static TResult Call<TServiceContract, TResult>([NotNull] Func<TServiceContract, TResult> serviceCall)
            where TServiceContract : class
        {
            return Call(serviceCall, null, null, null);
        }

        public static TResult Call<TServiceContract, TResult>([NotNull] Func<TServiceContract, TResult> serviceCall,
            [CanBeNull] string userName, [CanBeNull] string password, TimeSpan? timeout)
            where TServiceContract : class
        {
            return Call(serviceCall, null, null, userName, password, timeout);
        }

        public static TResult Call<TServiceContract, TResult>([NotNull] Func<TServiceContract, TResult> serviceCall,
            [CanBeNull] string hostName,
            [CanBeNull] string endpointDnsIdentity,
            [CanBeNull] string userName,
            [CanBeNull] string password,
            TimeSpan? timeout,
            [CanBeNull] string endpointConfigurationName = null)
            where TServiceContract : class
        {
            if (serviceCall == null)
                throw new ArgumentNullException(nameof(serviceCall));

            var configName = string.IsNullOrWhiteSpace(endpointConfigurationName) ? typeof(TServiceContract).Name : endpointConfigurationName;
            var service = GetService<TServiceContract>(hostName, endpointDnsIdentity, userName, password, configName);

            var clientChannel = (IClientChannel)service;
            if (timeout != null)
                clientChannel.OperationTimeout = timeout.Value;

            var channel = (IChannel)service;
            var success = false;
            try
            {
                var result = serviceCall(service);
                if (channel.State != CommunicationState.Faulted)
                {
                    channel.Close();
                    success = true;
                }
                return result;
            }
            finally
            {
                if (!success)
                    channel.Abort();
            }
        }

        #endregion

        #region channel factory

        private static TServiceContract GetService<TServiceContract>([CanBeNull] string hostName, [CanBeNull] string endpointDnsIdentity,
            [CanBeNull] string userName, [CanBeNull] string password, [NotNull] string endpointConfigurationName)
            where TServiceContract : class
        {
            var channelFactory = GetChannelFactory<TServiceContract>(hostName, endpointDnsIdentity, userName, password, endpointConfigurationName);
            return channelFactory.CreateChannel();
        }

        private static ChannelFactory<TServiceContract> GetChannelFactory<TServiceContract>([CanBeNull] string hostName,
            [CanBeNull] string endpointDnsIdentity,
            [CanBeNull] string userName,
            [CanBeNull] string password,
            [NotNull] string endpointConfigurationName)
            where TServiceContract : class
        {
            var connectionInfo = new ConnectionInfo(typeof(TServiceContract), hostName, userName, password, endpointConfigurationName);

            return (ChannelFactory<TServiceContract>)ChannelFactoryCache.GetOrAdd(
                connectionInfo,
                type =>
                {
                    var channelFactory = new ChannelFactory<TServiceContract>(endpointConfigurationName);
                    // невозможно добавить internationalization как behaviorExtensions через конфиг
                    // bug: http://connect.microsoft.com/wcf/feedback/details/216431/wcf-fails-to-find-custom-behaviorextensionelement-if-type-attribute-doesnt-match-exactly
                    if (!channelFactory.Endpoint.Behaviors.Contains(typeof(InternationalizationBehavior)))
                        channelFactory.Endpoint.Behaviors.Add(new InternationalizationBehavior());

                    // add credentials if specified
                    if (userName != null)
                    {
                        if (channelFactory.Credentials == null)
                            throw new InvalidOperationException();
                        channelFactory.Credentials.UserName.UserName = userName;
                        channelFactory.Credentials.UserName.Password = password;
                    }
                    // replace host name in service url
                    if (!string.IsNullOrEmpty(connectionInfo.HostName))
                    {
                        Uri actualUri;
                        if (!Uri.TryCreate(connectionInfo.HostName, UriKind.RelativeOrAbsolute, out actualUri))
                            throw new ArgumentException("Invalid Uri format: " + connectionInfo.HostName);

                        var defaultAddress = channelFactory.Endpoint.Address;
                        var actualUriBuilder = new UriBuilder(defaultAddress.Uri)
                        {
                            Host = actualUri.IsAbsoluteUri ? actualUri.Host : actualUri.ToString()
                        };
                        var builder = string.IsNullOrEmpty(endpointDnsIdentity)
                            ? new EndpointAddressBuilder(defaultAddress) {Uri = actualUriBuilder.Uri}
                            : new EndpointAddressBuilder(defaultAddress)
                            {
                                Uri = actualUriBuilder.Uri,
                                Identity = EndpointIdentity.CreateDnsIdentity(endpointDnsIdentity)
                            };
                        channelFactory.Endpoint.Address = builder.ToEndpointAddress();
                    }
                    channelFactory.Open();
                    return channelFactory;
                });
        }

        private sealed class ConnectionInfo
        {
            [NotNull]
            private Type Contract { get; set; }

            [CanBeNull]
            public string HostName { get; private set; }

            [CanBeNull]
            private string UserName { get; set; }

            [CanBeNull]
            private string Password { get; set; }

            [NotNull]
            private string EndpointConfigurationName { get; set; }

            public ConnectionInfo([NotNull] Type contract, string hostName, string userName, string password, string endpointConfigurationName)
            {
                if (contract == null)
                    throw new ArgumentNullException(nameof(contract));
                Contract = contract;
                HostName = hostName;
                UserName = userName;
                Password = password;
                EndpointConfigurationName = endpointConfigurationName;
            }

            #region Equality members

            private bool Equals(ConnectionInfo other)
            {
                return Contract == other.Contract
                       && string.Equals(HostName, other.HostName)
                       && string.Equals(UserName, other.UserName)
                       && string.Equals(Password, other.Password)
                       && string.Equals(EndpointConfigurationName, other.EndpointConfigurationName);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                return obj is ConnectionInfo && Equals((ConnectionInfo)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Contract.GetHashCode();
                    hashCode = (hashCode*397) ^ (HostName != null ? HostName.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ (UserName != null ? UserName.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ (Password != null ? Password.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ EndpointConfigurationName.GetHashCode();
                    return hashCode;
                }
            }

            #endregion
        }

        #endregion
    }
}