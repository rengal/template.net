using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Resto.Framework.Common.Helpers
{
    public static class ClrHelper
    {
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        /// <summary>
        /// Выполнить полную сборку мусора и отдать системе зарезервированные, но не используемы страницы памяти.
        /// Использовать только после ОЧЕНЬ тяжелых операций - запуск приложения, загрузка большого объема данных и т.п.
        /// </summary>
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        
    }
}