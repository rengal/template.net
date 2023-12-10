using System.ComponentModel;
using System.Diagnostics;

namespace Resto.Common
{
    /// <summary>
    /// Класс-помошник взаимодействия разработываемого кода с VisualStudio
    /// </summary>
    public static class VsHelper
    {
        /// <summary>
        /// Признак выполнения кода в режиме дизайнера
        /// </summary>
        private static readonly bool isDesigntime;

        static VsHelper()
        {
            // Подробности: http://dotnetfacts.blogspot.ru/2009/01/identifying-run-time-and-design-mode.html
            isDesigntime =
                (LicenseManager.UsageMode == LicenseUsageMode.Designtime) ||
                (Process.GetCurrentProcess().ProcessName == "devenv");
        }

        /// <summary>
        /// Признак выполнения кода в режиме дизайнера
        /// </summary>
        public static bool IsDesigntime
        {
            get { return isDesigntime; }
        }

        /// <summary>
        /// Признак выполнения кода не в режиме дизайнера
        /// </summary>
        public static bool IsRuntime
        {
            get { return !IsDesigntime; }
        }
    }
}