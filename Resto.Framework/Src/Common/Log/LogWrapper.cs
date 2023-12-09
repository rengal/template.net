using System;
using log4net;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс-враппер для интерфейса логгера. Предназначен для Lazy-загрузки логгеров, т.е.
    /// реально интерфейс запрашивается только при первом обращении к методу интерфейса ILog.
    /// </summary>
    public sealed class LogWrapper
    {
        private ILog innerLog;

        public ILog Log => innerLog ?? (innerLog = LogFactory.Instance.GetLogger(Type));

        public Type Type { get; }

        public LogWrapper(Type type)
        {
            if (type == null)
                throw new RestoException("Logged type must be defined.");

            Type = type;
        }
    }
}