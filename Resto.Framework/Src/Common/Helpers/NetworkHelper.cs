using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Helpers
{
    public static class NetworkHelper
    {
        /// <summary>
        /// Возвращает список адресов, по которым к данному компьютеру можно подключиться.
        /// Порядок: IPv4, IPv6, имя NetBIOS.
        /// </summary>        
        [NotNull, Pure]
        public static IEnumerable<string> GetExternalAddresses()
        {
            NetworkInterface[] networkInterfaces;
            try
            {
                networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            }
            catch (NetworkInformationException)
            {
                return Enumerable.Empty<string>();
            }

            var ipAddresses = networkInterfaces
                .Where(networkInterface => networkInterface.NetworkInterfaceType.NotIn(NetworkInterfaceType.Loopback, NetworkInterfaceType.Tunnel))
                .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                .Select(ipAddressInformation => ipAddressInformation.Address)
                .Where(ipAddress => ipAddress.AddressFamily.In(AddressFamily.InterNetwork, AddressFamily.InterNetworkV6))
                .OrderBy(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                .Do(ipAddress => { if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6) ipAddress.ScopeId = 0; })
                .Distinct()
                .Select(ipAddress => ipAddress.ToString());

            string hostName;
            try
            {
                hostName = Environment.MachineName;
            }
            catch (InvalidOperationException)
            {
                return ipAddresses;
            }

            return ipAddresses.ContinueWith(hostName);
        }
    }
}