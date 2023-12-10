using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using log4net;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Common.DataTerminal;

namespace Resto.Common.DataTerminalDrivers
{
    #region CleverenceDataTerminalDriver

    /// <summary>
    /// Драйвер для работы с любым терминалом, который поддерживает программное
    /// обеспечение от Cleverence.
    /// </summary>
    public class CleverenceDataTerminalDriver : DataTerminalDriverBase
    {
        #region Data members

        /// <summary>
        /// Командная строка при выгрузке данных на ТСД.
        /// </summary>
        private const string UploadCmd = "upload close closeerr notray";

        /// <summary>
        /// Командная строка при загрузке данных с ТСД.
        /// </summary>
        private const string DownloadCmd = "download close closeerr notray";

        /// <summary>
        /// Консольная программа для обмена данымии с ТСД.
        /// </summary>
        private const string Programm = "SyncCon.exe";

        /// <summary>
        /// Имя процесса, который отвечает за синхронизацию с ТСД.
        /// </summary>
        private const string ProccessName = "Cleverence.MobileSMARTS.Synchronizer";

        /// <summary>
        /// Логгер.
        /// </summary>
        private static readonly ILog LOG = LogFactory.Instance.GetLogger(typeof(CleverenceDataTerminalDriver));
 
        #endregion Data members

        #region Constructor

        /// <summary>
        /// Пустой конструктор, требуется для сериализации.
        /// </summary>
        public CleverenceDataTerminalDriver()
        {
        }

        /// <summary>
        /// Конструктор задаёт имя драйвера.
        /// </summary>
        /// <param name="name">Имя драйвера.</param>
        public CleverenceDataTerminalDriver(string name)
            : base(name)
        {
        }

        #endregion

        #region Overriden methods

        public override void UploadToDevice()
        {
            CheckSettings();
            base.UploadToDevice();
            Run(Path.Combine(ExePath, Programm), UploadCmd);
        }

        public override void DownloadFromDevice()
        {
            CheckSettings();
            base.DownloadFromDevice();
            Run(Path.Combine(ExePath, Programm), DownloadCmd);
        }

        private void CheckSettings()
        {
            if (string.IsNullOrEmpty(ExePath))
                throw new RestoException(Resources.ExePathNotDefined);
        }

        /// <summary>
        /// Выполняет запуск консольной программы по обмену с ТСД.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        private static void Run(string fileName, string args)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(string.Format(Resources.SyncFileNotFoundException, fileName));
            }

            var info = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                Arguments = args,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process p = Process.Start(info))
                {
                    byte[] bytes = p.StandardOutput.CurrentEncoding.GetBytes(p.StandardOutput.ReadToEnd());

                    string res = Encoding.GetEncoding(866).GetString(bytes);

                    if(p.ExitCode == 1 || p.ExitCode == -1)
                    {
                        LOG.ErrorFormat("The process '{0}' fails: {1}", ProccessName, res);
                    }

                    switch (p.ExitCode)
                    {
                        case 1:
                            if (!Settings.IgnoreCleverenceDriverDriverExceptions)
                                throw new RestoException(res);
                            break;
                        case -1:
                            throw new RestoException(Resources.AbortOperation);
                        default:
                            LOG.InfoFormat("The process '{0}' finished successfully: {1}", ProccessName, res);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LOG.ErrorFormat("Can not start '{0}': {1}", fileName, ex.StackTrace);
                throw new RestoException(string.Format(Resources.StartingProccessException, ex.Message));
            }
        }

        /// <summary>
        /// Останавливает процесс который отвечает за выгрузку/загрузку.
        /// </summary>
        public override void Stop()
        {
            var proccess = Process.GetProcessesByName(ProccessName);
            foreach (var p in proccess)
            {
                try
                {
                    p.Kill();
                }
                catch (InvalidOperationException ex)
                {
                    // возникает в случае, если процесс уже был остановлен, просто пишем в лог
                    LOG.ErrorFormat("The process '{0}' have already stopped: {1}", ProccessName, ex.Message);
                }
                catch (Exception ex)
                {
                    LOG.ErrorFormat("Can't stop the process '{0}': {1}", ProccessName, ex.StackTrace);
                    throw new RestoException(string.Format(Resources.FinishingProccessException, ex.Message));
                }
            }
        }

        #endregion Overriden methods
    }

    #endregion CleverenceDataTerminalDriver
}
