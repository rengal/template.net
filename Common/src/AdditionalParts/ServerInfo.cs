using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;

namespace Resto.Data
{
    public partial class ServerInfo
    {
        public ServerInfo(string serverName, string serverAddr, string serverSubUrl, int port, [CanBeNull] string protocol)
        {
            this.serverName = serverName;
            this.version = String.Empty;
            this.computerName = String.Empty;
            this.serverAddr = serverAddr;
            this.serverSubUrl = serverSubUrl;
            this.port = port;
            this.protocol = CommunicationProtocols.CoerceProtocol(protocol);
        }

        public string ProtocolText
        {
            get
            {
                return Protocol == CommunicationProtocols.Unknown
                    ? string.Empty
                    : Protocol;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as ServerInfo;
            if (other == null)
                return false;

            // Оптимизация
            if (ReferenceEquals(this, obj))
                return true;

            return HasMatchedAddress(other.ServerAddr, other.Port)
                   && CommunicationProtocols.CoerceProtocol(other.Protocol) == CommunicationProtocols.CoerceProtocol(Protocol)
                   && other.ServerSubUrl == ServerSubUrl
                   && ServerInfoHelper.EditionsEqual(edition, other.Edition);
        }

        public override int GetHashCode()
        {
            return (ServerAddr + Port + ServerSubUrl + CommunicationProtocols.CoerceProtocol(Protocol)).GetHashCode();
        }

        public bool HasMatchedAddress(string address, int port)
        {
            if (Port != port)
                return false;

            if (ServerAddr == address)
                return true;
            if (ServerAddr == null || address == null)
                return false;

            string thisAddress = ServerAddr.ToLowerInvariant();
            address = address.ToLowerInvariant();
            if (thisAddress == "localhost")
            {
                thisAddress = "127.0.0.1";
            }
            if (address == "localhost")
            {
                address = "127.0.0.1";
            }
            return address == thisAddress;
        }

        public static ICollection<string> GetProtocolsToTry(string knownProtocol)
        {
            var result = new List<string>();
            if (CommunicationProtocols.CoerceProtocol(knownProtocol) == CommunicationProtocols.Unknown)
            {
                result.Add(CommunicationProtocols.Https);
                result.Add(CommunicationProtocols.Http);
            }
            else
            {
                result.Add(knownProtocol);
            }
            return result;
        }

        public static class CommunicationProtocols
        {
            public const string Http = "http";
            public const string Https = "https";

            /// <summary>
            /// Представляет неизвестный протокол (может быть как HTTP, так и HTTPS).
            /// </summary>
            public const string Unknown = "unknown";

            public static string CoerceProtocol(string protocol, string defaultProtocol = Unknown)
            {
                return string.IsNullOrEmpty(protocol) || protocol == Unknown
                    ? defaultProtocol
                    : protocol;
            }
        }
    }
}