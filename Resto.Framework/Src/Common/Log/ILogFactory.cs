using System;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using log4net.Appender;

namespace Resto.Framework.Common
{
    public interface ILogFactory
    {
        bool Configured { get; }
        void ConfigureFromConfig(string configFileName, string logFilesDirectory);
        void ConfigureFromAppConfig(string logFilesDirectory);
        [NotNull]
        ILog GetLogger(Type type);

        void Configure(string logFileName);
        void Configure(string logFileName, string logPattern);
        void Configure(string logFileName, string logPattern, FileAppender.LockingModelBase lockingModel);
        void Configure(string logFileName, string logPattern,
                       FileAppender.LockingModelBase lockingModel, bool useAutoSplit, bool useAutoCompress, int logFileAgeDays);
        void SetLogThreshold(string logThreshold);
        void InitForBack();
    }
}
