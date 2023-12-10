namespace Resto.Framework.Common.DataTerminal
{

    #region DataTerminalBase

    /// <summary>
    /// Базовый класс драйвера под ТСД.
    /// Используется в качестве родителя, для реализации прослойки между конкретным 
    /// программным обеспечением ТСД и iiko.
    /// (Один и тот же ТСД, может сканировать используя программы от разных производителей)
    /// </summary>
    public class DataTerminalDriverBase : IDataTerminalDriver
    {
        #region Constructors

        public DataTerminalDriverBase()
        {
        }

        public DataTerminalDriverBase(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Имя драйвера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Путь для выгрузки csv файлов из iiko.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Путь для загрузки csv файлов в iiko.
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// Путь для испольняемого файла программы выгрузки.
        /// </summary>
        public string ExePath { get; set; }

        /// <summary>
        /// true - если ТСД в рабочем режиме.
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region Methods

        public void Update(IDataTerminalDriver newTerminal)
        {
            IsActive = newTerminal.IsActive;
            OutputPath = newTerminal.OutputPath;
            InputPath = newTerminal.InputPath;
            ExePath = newTerminal.ExePath;
        }

        /// <summary>
        /// Выгружает данные на терминал/устройство.
        /// </summary>
        public virtual void UploadToDevice()
        {
        }

        /// <summary>
        /// Загружает ланные с терминала.
        /// </summary>
        public virtual void DownloadFromDevice()
        {
        }

        /// <summary>
        /// Прекращает процесс обмена загрузки/выгрузки.
        /// </summary>
        public virtual void Stop()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }

    #endregion DataTerminalBase
}