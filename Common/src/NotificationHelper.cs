using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using log4net;
using Resto.Data;

namespace Resto.Common
{
    public class NotificationHelper
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(NotificationHelper));

        public static void SendClientNotification(ProblemSeverity problemSeverity, string errorText, Exception ex)
        {
            SendClientNotification(problemSeverity, errorText, errorText, ex);
        }

        
        /// <summary>
        /// Отсылает нотификацию от клиента на сервер об ошибке
        /// </summary>
        /// <param name="problemSeverity">Важность сообщения</param>
        /// <param name="errorText">Текст ошибки</param>
        /// <param name="checkerId">Константы для антиспама, при нуле выступает errorText</param>
        /// <param name="ex">Эксепшен, если есть</param>
 
        public static void SendClientNotification(ProblemSeverity problemSeverity, string errorText, string checkerId, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            if (ex != null)
            {
                sb.AppendLine("Exception: " + ex.Message);
            }

            System.Reflection.Assembly oAssembly = Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo oFileVersionInfo = FileVersionInfo.GetVersionInfo(oAssembly.Location);
            sb.AppendFormat("Client Version, {0}: {1}", oFileVersionInfo.InternalName, oFileVersionInfo.FileVersion);
            sb.AppendLine();
            string hostName = Dns.GetHostName();
            sb.AppendLine("Client Host: " + hostName);
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            var ipSortedList = new List<IPAddress>(hostEntry.AddressList);
            ipSortedList.Sort((i1, i2) => i1.ToString().CompareTo(i2.ToString()));
            sb.Append("IP: ");
            sb.AppendLine(string.Join(", ", ipSortedList.Select(item => item.ToString()).ToArray()));
            errorText += sb.ToString();

            LOG.Info("SendClientNotification: " + errorText);

            try
            {
                var result = new CheckResult(problemSeverity, errorText, checkerId);
                ServiceClientFactory.WatchDogService.Notify(result).CallSync();
            }
            catch (Exception e)
            {
                LOG.Error("Exception in SendClientNotification: " + e.Message, e);
            }
        }
    }
}
