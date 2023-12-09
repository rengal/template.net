using System;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository;

namespace Resto.Framework.Xml
{
    static partial class SafeObjectStateSaver
    {
        private sealed class MemoryLoggerContext : IDisposable
        {
            private readonly ILoggerRepository repository;
            private readonly MemoryAppender memoryAppender;

            public MemoryLoggerContext()
            {
                memoryAppender = new MemoryAppender
                {
                    Threshold = Level.All
                };
                memoryAppender.ActivateOptions();

                repository = LogManager.CreateRepository(Guid.NewGuid().ToString());
                var hierarchy = (log4net.Repository.Hierarchy.Hierarchy)repository;
                hierarchy.Root.AddAppender(memoryAppender);
                hierarchy.Root.Level = Level.All;
                hierarchy.Configured = true;

                Logger = new LogImpl(hierarchy.Root);
            }

            public ILog Logger { get; }

            public LoggingEvent[] PopAllEvents() => memoryAppender.PopAllEvents();

            public void Dispose() => repository.Shutdown();
        }
    }
}
