using System;
using System.IO;

namespace Resto.Common.Properties
{
    /// <summary>
    /// Класс для хранения общих настроек ("настраиваемых констант").
    /// </summary>
    partial class Settings
    {
        private string additionalTempFolder = string.Empty;

        /// <summary>
        /// Позволяет настроить другую конечную папку для хранения временных данных приложения.
        /// </summary>        
        public string AdditionalTempFolder
        {
            set { additionalTempFolder = value; }
            private get { return additionalTempFolder; }
        }

        /// <summary>
        /// Пароль, указанный в параметрах запуска.
        /// </summary>
        public string StartupParamsPassword { get; set; }

        internal string EditionSubPath
        {
            get
            {
#if CHAIN_EDITION
                return Path.Combine(CHAIN_EDITION_SUBPATH, AdditionalTempFolder);                
#else
                return Path.Combine(RMS_EDITION_SUBPATH, AdditionalTempFolder);
#endif
            }
        }

        public string CommonUsersHomePath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), IIKO_APPDATA_SUBPATH, EditionSubPath);
            }
        }

        public string HomePath
        {
            get
            {
                // Возможно стоит предусмотреть возможность загрузки конфига не из %APPDATA%
                // а из настраиваемой папки... В этом случае нужно разрулить имена конфига и шаредконфига.
                // string path = BASE_PATH;
                // if (string.IsNullOrEmpty(path)
                //     || path.Trim().Length == 0)
                // {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), IIKO_APPDATA_SUBPATH, EditionSubPath);
                // }
                // return path;
            }
        }

        public string SharedHomePath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), IIKO_APPDATA_SUBPATH, EditionSubPath);
            }
        }

        public string CardReaderHomePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), IIKO_APPDATA_SUBPATH);
            }
        }

        public string DataTerminalHomePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), IIKO_APPDATA_SUBPATH);
            }
        }
    }
}
