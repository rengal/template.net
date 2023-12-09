using System;
using System.ComponentModel;
using System.Diagnostics;
using log4net;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows
{
    public static class ProcessHelper
    {
        [Pure]
        public static bool CheckHasExited([NotNull] this Process process, int processId, bool undeterminedStateValue, [NotNull] ILog log)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            try
            {
                return process.HasExited;
            }
            catch (InvalidOperationException e)
            {
                log.WarnFormat("Cannot determine whether process {0} is alive: {1}", processId, e.Message);
                return undeterminedStateValue;
            }
            catch (Win32Exception e)
            {
                log.WarnFormat("Cannot determine whether process {0} is alive: {1}", processId, e.Message);
                return undeterminedStateValue;
            }
        }

        public static int? TryGetExitCode([NotNull] this Process process, [NotNull] string processDescription, [NotNull] ILog log)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));
            if (processDescription == null)
                throw new ArgumentNullException(nameof(processDescription));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            try
            {
                return process.ExitCode;
            }
            catch (InvalidOperationException e)
            {
                log.Warn($"Cannot determine exit code of the process {process.TryGetProcessId(processDescription, log) } “{processDescription}”: {e.Message}");
                return null;
            }
        }

        public static int? TryGetProcessId([NotNull] this Process process, [NotNull] string processDescription, [NotNull] ILog log)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));
            if (processDescription == null)
                throw new ArgumentNullException(nameof(processDescription));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            try
            {
                return process.Id;
            }
            catch (InvalidOperationException e)
            {
                log.Warn($"Couldn't get pid for process “{processDescription}”: {e.Message}");
                return null;
            }
        }
    }
}
