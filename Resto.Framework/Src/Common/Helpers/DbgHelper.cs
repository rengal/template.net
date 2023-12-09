using System.Diagnostics;
using System.IO;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Хелпер для работы с adplus.exe
    /// </summary>
    public static class DbgHelper
    {
        #region Consts

        private const string CmdTempate = "-crash -p {0} -NoDumpOnFirst -o \"{1}\"";
        private const string DumpsFolder = @"C:\Dumps";
        private const string AdPlusPath = DumpsFolder + @"\Tools\adplus.exe";

        #endregion

        #region Fields / Properties

        public static bool IsDebuggerAttached { get; private set; }

        #endregion

        /// <summary>
        /// Если в папке C:\Dumps присутствует дебаггер -
        /// запустить отладчик и подключить его к текущему процессу в режиме снятия полного дампа 
        /// при необработаботанном исключении
        /// </summary>
        public static void AttachDebuggerIfPresent()
        {
            if (!File.Exists(AdPlusPath))
                return;
            var currentProcess = Process.GetCurrentProcess();
            var dumpsDirPath = Path.Combine(DumpsFolder, currentProcess.ProcessName);
            if (!Directory.Exists(dumpsDirPath))
                Directory.CreateDirectory(dumpsDirPath);
            var command = string.Format(CmdTempate, currentProcess.Id, dumpsDirPath);
            Process.Start(AdPlusPath, command);
            IsDebuggerAttached = true;
        }
    }
}