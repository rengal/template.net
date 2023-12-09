using System;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using log4net.Core;

namespace Resto.Framework.Common.Log
{
    public static class ILogExtensions
    {
        public static void Trace(this ILog log, string message)
        {
            log.Logger.Log(null, Level.Trace, message, null);
        }

        [StringFormatMethod("format")]
        public static void TraceFormat(this ILog log, string format, params object[] args)
        {
            log.Trace(string.Format(format, args));
        }

        [StringFormatMethod("format")]
        public static void ErrorFormat(this ILog log, Exception exception, string format, params object[] args)
        {
            log.Error(string.Format(format, args), exception);
        }
    }
}
