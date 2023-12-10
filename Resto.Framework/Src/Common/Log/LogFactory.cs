using Microsoft.Extensions.DependencyInjection;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс-фабрика для получения экземпляров логгера
    /// </summary>
    public class LogFactory
    {
        private static ILogFactory? instance;

        public static ILogFactory Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                var services = new ServiceCollection();
                var serviceProvider = services.BuildServiceProvider();
                instance = serviceProvider.GetService<ILogFactory>();
                return instance;
            }
        }

        /// <summary>
        /// Полная маска форматирования лога
        /// [Дата/время] уровень лога [ID нити] (Метод@Модуль:Строка)
        /// </summary>
        public const string DEFAULT_LOG_PATTERN = @"[%d] %5p [%t] (%M@%F:%L) - %m%n";

        /// <summary>
        /// Маска форматирования лога
        /// [yyyy-MM-dd HH.mm.ss] уровень лога [ID нити] [название типа:название метода]
        /// </summary>
        public const string SHORT_LOG_PATTERN = @"[%date{yyyy-MM-dd HH:mm:ss,fff}] %5p [%2t] [%type{1}:%M] - %m%n";
        //public const string SHORT_LOG_PATTERN = @"[%date{HH:mm:ss,fff}] %5p [%t] - %m%n";

        /// <summary>
        /// Уровень лога по умолчанию (DEBUG)
        /// </summary>
        public const string DEFAULT_LOG_THRESHOLD = "DEBUG";

        /// <summary>
        /// Маска форматирования дат для вывода в лог
        /// </summary>
        public const string DEFAULT_DATE_TIME_FORMAT = "dd:MM:yyyy HH:mm:ss";
    }
}