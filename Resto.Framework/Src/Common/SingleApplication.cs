using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Приложение, которое может быть запущенно только в одном экземпляре
    /// </summary>
    [Obsolete("Класс некорректен, будет удален в ближайших версиях. Следует использовать SingleInstance")]
    public static class SingleApplication
    {
        #region Constants

        private const int SW_RESTORE = 9;

        #endregion Constants

        #region Data Members

        private static Mutex mutex;
        private static string sTitle;
        private static IntPtr windowHandle;

        #endregion Data Members

        #region Implementation
        

        private static bool EnumWindowCallBack(int hwnd, int lParam)
        {
            windowHandle = (IntPtr)hwnd;

            var sbuilder = new StringBuilder(256);
            NativeMethods.GetWindowText((int)windowHandle, sbuilder, sbuilder.Capacity);
            var strTitle = sbuilder.ToString();

            if (strTitle == sTitle)
            {
                NativeMethods.ShowWindow(windowHandle, SW_RESTORE);
                NativeMethods.SetForegroundWindow(windowHandle);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет, запущенно ли приложение или нет
        /// </summary>
        /// <returns>returns true if already running</returns>
        private static bool IsAlreadyRunning()
        {
            var longPath = new StringBuilder(255);
            NativeMethods.GetLongPathName(
                Assembly.GetEntryAssembly().Location,
                longPath,
                longPath.Capacity);

            var sExeName = Path.GetFileName(longPath.ToString());

            mutex = new Mutex(true, sExeName);

            return !mutex.WaitOne(0, false);
        }

        #endregion Implementation

        #region Methods

        /// <summary>
        /// Проверяет, запущенна ли уже копия этого приложения.
        /// </summary>
        /// <param name="frmMain">главная форма приложения</param>
        /// <returns>true если приложение ещё не было запущенно, иначе false</returns>
        public static bool CheckApplication(Form frmMain)
        {
            if (IsAlreadyRunning())
            {
                sTitle = frmMain.Text;

                //
                // Активирует копию приложения, которое было запущенно ранее
                //
                NativeMethods.EnumWindows(EnumWindowCallBack, 0);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Запускает копию одноэкземплярного Windows Form приложения. Если приложение уже запущенно,
        /// то ещё одна копия не запускаеться, а активируется копия, которая была запущенна ранее.
        /// </summary>
        /// <param name="frmMain">главная форма приложения</param>
        /// <param name="setForeground">параметр, показывающий следует ли активировать форму, запущенную ранее</param>
        /// <returns>true если запускается первая копия приложения, иначе false</returns>
        public static bool Run(Form frmMain, bool setForeground)
        {
            if (IsAlreadyRunning())
            {
                sTitle = frmMain.Text;

                //
                // Set focus on previously running app
                //
                if (setForeground)
                {
                    NativeMethods.EnumWindows(EnumWindowCallBack, 0);
                }

                return false;
            }

            Application.Run(frmMain);

            return true;
        }

        /// <summary>
        /// Запускает приложение в одноэкземплярном режиме для консольных приложений
        /// </summary>
        /// <returns>true если запускается первая копия приложения, иначе false</returns>
        public static bool Run()
        {
            return !IsAlreadyRunning();
        }

        #endregion Methods
    }
}
