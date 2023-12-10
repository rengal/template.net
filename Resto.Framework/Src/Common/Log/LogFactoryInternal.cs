using System;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace Resto.Framework.Common
{
    public class LogFactoryInternal : ILogFactory
    {
        /// <summary>
        /// Признак инициализированности класса
        /// </summary>
        private bool configured;
        public bool Configured
        {
            get { return configured; }
        }

        /// <summary>
        /// Appender для консоли
        /// </summary>
        private ConsoleAppender consoleAppender;
        /// <summary>
        /// Appender для файла
        /// </summary>
        private FileAppender fileAppender;

        /// <summary>
        /// Закрыть все используемые логгеры, освободить ресурсы
        /// </summary>
        public void Close()
        {
            if (!configured)
                return;
            if (consoleAppender != null)
                consoleAppender.Close();
            if (fileAppender != null)
                fileAppender.Close();
            LogManager.ShutdownRepository();
            configured = false;
        }

        /// <summary>
        /// Задать конфигурационные параметры
        /// </summary>
        /// <param name="logFileName">Полный путь к лог-файлу</param>
        public void Configure(string logFileName)
        {
            Configure(logFileName, LogFactory.DEFAULT_LOG_PATTERN);
        }

        /// <summary>
        /// Задать конфигурационные параметры
        /// </summary>
        /// <param name="logFileName">Полный путь к лог-файлу</param>
        /// <param name="logPattern">Шаблон форматирования лога</param>
        public void Configure(string logFileName, string logPattern)
        {
            Configure(logFileName, logPattern, new FileAppender.ExclusiveLock());
        }

        /// <summary>
        /// Задать конфигурационные параметры
        /// </summary>
        /// <param name="logFileName">Полный путь к лог-файлу</param>
        /// <param name="logPattern">Шаблон форматирования лога</param>
        /// <param name="lockingModel">Модель блокировки лог-файла</param>
        public void Configure(string logFileName, string logPattern, FileAppender.LockingModelBase lockingModel)
        {
            Configure(logFileName, logPattern, lockingModel, false, false, 0);
        }

        /// <summary>
        /// Задать конфигурационные параметры
        /// </summary>
        /// <param name="logFileName">Полный путь к лог-файлу</param>
        /// <param name="logPattern">Шаблон форматирования лога</param>
        /// <param name="lockingModel">Модель блокировки лог-файла</param>
        /// <param name="useAutoSplit">Использовать ли авторазбиение лога</param>
        /// <param name="useAutoCompress">Использовать ли автосжатие лога</param>
        /// <param name="logFileAgeDays">Сколько дней хранятся старые зазипованные логи</param>
        public void Configure(string logFileName, string logPattern,
            FileAppender.LockingModelBase lockingModel, bool useAutoSplit, bool useAutoCompress, int logFileAgeDays)
        {
            Close();

            var layout = new PatternLayout(logPattern);
            consoleAppender = new ConsoleAppender { Layout = layout };

            fileAppender = !useAutoSplit ? new FileAppender() : new ZippedRollingFileAppender(useAutoCompress, logFileAgeDays);
            fileAppender.File = logFileName;
            fileAppender.Layout = layout;
            fileAppender.Encoding = Encoding.UTF8;
            if (lockingModel != null)
                fileAppender.LockingModel = lockingModel;
            BasicConfigurator.Configure(consoleAppender);
            BasicConfigurator.Configure(fileAppender);
            fileAppender.ActivateOptions();
            consoleAppender.ActivateOptions();
            configured = true;
        }

        public void ConfigureFromAppConfig(string logFilesDirectory)
        {
            ThreadContext.Properties["LogFileDir"] = logFilesDirectory;
            XmlConfigurator.Configure();
            configured = true;
        }

        public void ConfigureFromConfig(string configFileName, string logFilesDirectory)
        {
            ThreadContext.Properties["LogFileDir"] = logFilesDirectory;
            XmlConfigurator.Configure(new Uri(configFileName));
            configured = true;
        }

        /// <summary>
        /// Учтановить уровень логгирования для текущего AppDomain
        /// </summary>
        /// <param name="logThreshold">Уровень логгирования (регистронечувствительно). Возможные значения  - 
        /// 'ALL', 'DEBUG', 'INFO', 'WARNING', 'ERROR', 'FATAL', 'OFF'</param>
        public void SetLogThreshold(string logThreshold)
        {
            switch (logThreshold.Trim().ToUpper())
            {
                case "ALL":
                    LogManager.GetRepository().Threshold = Level.All;
                    break;
                case "INFO":
                    LogManager.GetRepository().Threshold = Level.Info;
                    break;
                case "ERROR":
                    LogManager.GetRepository().Threshold = Level.Error;
                    break;
                case "FATAL":
                    LogManager.GetRepository().Threshold = Level.Fatal;
                    break;
                case "DEBUG":
                    LogManager.GetRepository().Threshold = Level.Debug;
                    break;
                case "WARNING":
                    LogManager.GetRepository().Threshold = Level.Warn;
                    break;
                case "OFF":
                    LogManager.GetRepository().Threshold = Level.Off;
                    break;
            }
        }

        /// <summary>
        /// Инициализировать фабрику "по старой схеме" для BackOffice
        /// </summary>
        public void InitForBack()
        {
            Configure(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                    + @"\iiko\" + "back-log.log", LogFactory.SHORT_LOG_PATTERN);
        }

        /// <summary>
        /// Получить экземпляр логгера
        /// </summary>
        /// <param name="type">Тип, который будет ассоциирован с логгером</param>
        /// <returns>интерфейс ILog для использования</returns>
        [NotNull]
        ILog ILogFactory.GetLogger(Type type)
        {
            if (!configured)
                throw new RestoException("Log factory not configured. Use LogFactory.Instance.Configure() method first");
            return LogManager.GetLogger(type);
        }
    }
}
