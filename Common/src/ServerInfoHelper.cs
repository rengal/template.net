using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Text;
using Resto.Common.Properties;
using Resto.Data;
using Resto.Framework.Common.XmlSerialization;
using log4net;

namespace Resto.Common
{
    public static class ServerInfoHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerInfoHelper));

        /// <param name="edition">EditionManager.CurrentEdition.GetID() (default/chain)</param>
        /// <returns>true, если редакция удаленного сервера равна редакции бекофиса, либо неизвестна</returns>
        public static bool HasCompatibleOrUnknownEdition(ServerInfo si, String edition)
        {
            return String.IsNullOrEmpty(si.Edition) || EditionsEqual(si.Edition, edition);
        }

        public static bool EditionsEqual(string edition1, string edition2)
        {
            return string.IsNullOrEmpty(edition1) && string.IsNullOrEmpty(edition2)
                   || edition1 != null && edition1.Equals(edition2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool UpdateServerInfo(ServerInfo si)
        {
            return UpdateServerInfo(si, true);
        }

        public static bool UpdateServerInfo(ServerInfo si, bool logErrors)
        {
            string text = GetServerInfo(si, logErrors);
            if (string.IsNullOrEmpty(text))
                return false;
            ProcessResult(si, text);
            return true;
        }

        public static string GetServerInfo(ServerInfo si, bool logErrors)
        {
            Contract.Requires(si != null);

            var protocols = ServerInfo.GetProtocolsToTry(si.Protocol);

            foreach (var protocol in protocols)
            {
                var request = (HttpWebRequest)WebRequest.Create(string.Format(
                    Settings.SERVER_INFO_PAGE_ADDRESS,
                    protocol, si.ServerAddr, si.Port, si.ServerSubUrl));

                request.Timeout = Settings.GETTING_SERVER_INFO_TIMEOUT_MILLISECONDS;
                request.Method = "GET";
                request.KeepAlive = CommonConfig.Instance.KeepAliveHttpWebRequestFlag;
                si.IsPresent = false;
                try
                {
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        string charset = response.CharacterSet;
                        if (string.IsNullOrEmpty(charset))
                        {
                            charset = "UTF-8";
                        }
                        // ReSharper disable once AssignNullToNotNullAttribute https://stackoverflow.com/a/16911086
                        var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                        string result = reader.ReadToEnd().Trim();

                        si.Protocol = protocol;

                        return result;
                    }
                }
                catch (WebException ex)
                {
                    si.IsPresent = false;
                    if (logErrors && Log.IsWarnEnabled)
                    {
                        Log.Warn(ex);
                    }
                }
            }

            return null;
        }

        public static string GetServerInfo(ServerInfo si)
        {
            return GetServerInfo(si, true);
        }

        private static void ProcessResult(ServerInfo si, string text)
        {
            var info = Serializer.Deserialize<ServerInfo>(text, true);
            si.ServerName = info.ServerName;
            si.Version = info.Version;
            si.Edition = info.Edition;
            si.ComputerName = info.ComputerName;
            si.ServerState = info.ServerState;

            si.IsPresent = true;
        }
    }
}
