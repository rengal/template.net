using System;
using Resto.Common;

namespace Resto.Data
{
    public partial class VersionInfo
    {
        /// <summary>
        /// Основные цифры версии MAJOR.MINOR (Ex: 2.5).
        /// </summary>
        public static string CurrentVersion => $"{MAJOR}.{MINOR}";

        /// <summary>
        /// Номер версии с точностью до ревизии сборки: MAJOR.MINOR.BUILD.REVISION (Ex: 4.1.1158.0).
        /// Такое представление использует в своих конфигах бекофис.
        /// </summary>
        public static string String => $"{MAJOR}.{MINOR}.{BUILD}.{REVISION}";

        /// <summary>
        /// Номер версии с точностью до ревизии сборки с альтернативным форматом для локальных сборок:
        /// MAJOR.MINOR.BUILD.REVISION (Ex: 4.1.1158.0)
        /// либо
        /// MAJOR.MINOR." custom" (Ex: 2.4 custom).
        /// Такое представление версии использует внутри себя сервер.
        /// </summary>
        public static string ShortDescription => BUILD == 0 ? $"{MAJOR}.{MINOR} custom" : String;

        /// <summary>
        /// Возвращает дату и время сборки.
        /// </summary>
        public static DateTime BuildTime => (BUILD_DATE_MILLIS / (TimeSpan.TicksPerSecond / TimeSpan.TicksPerMillisecond)).FromUnixTime();
    }
}