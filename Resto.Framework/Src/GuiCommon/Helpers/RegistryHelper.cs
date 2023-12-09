using System.IO;
using Microsoft.Win32;

using Resto.Framework.Common;

namespace Resto.Framework.UI.Common
{
    public static class RegistryHelper
    {
        #region Methods

        /// <summary>
        /// Чтение ключа реестра
        /// </summary>
        /// <remarks>
        /// Поиск выполняется по записям реестра для 32-х и 64-х битным разделам реестра
        /// </remarks>
        /// <param name="hive">Узел рееста</param>
        /// <param name="filter">Подключ реестра</param>
        /// <returns>Значение ключа</returns>
        public static string GetKeyValue(RegistryHive hive, string filter)
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
            var view32 = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32);

            using (var key64 = view64.OpenSubKey(filter, false))
            {
                using (var key32 = view32.OpenSubKey(filter, false))
                {
                    var key = key64 ?? key32;

                    if (key == null)
                    {
                        return null;
                    }

                    return key.GetValue(string.Empty) as string;
                }
            }
        }

        /// <summary>
        /// Проверка наличия в системе COM-объекта c заданным ProgID
        /// </summary>
        /// <param name="progId">ProgID объекта</param>
        /// <param name="isInprocServer">Является ли объект Inproc-сервером</param>
        /// <returns>результат проверки</returns>
        public static bool CheckLibByProgId(string progId, bool isInprocServer)
        {
            var clsid = GetKeyValue(RegistryHive.ClassesRoot, string.Format("{0}\\Clsid", progId));

            if (clsid.IsNullOrEmpty())
            {
                return false;
            }

            return CheckLibByClsid(clsid, isInprocServer);
        }

        /// <summary>
        /// Проверка наличия в системе компонента c заданным CLSID
        /// </summary>
        /// <param name="clsid">CLSID компонента</param>
        /// <param name="isInprocServer">Является ли объект Inproc-сервером</param>
        /// <returns>результат проверки</returns>
        public static bool CheckLibByClsid(string clsid, bool isInprocServer)
        {
            // проверяем правильность регистрации в реестре
            var filter = isInprocServer
                ? string.Format("CLSID\\{0}\\InprocServer32", clsid)
                : string.Format("CLSID\\{0}\\LocalServer32", clsid);

            var dllPath = GetKeyValue(RegistryHive.ClassesRoot, filter);

            if (dllPath.IsNullOrEmpty())
            {
                return false;
            }

            dllPath = dllPath.Split('/')[0].Trim(new[] { ' ', '\"' });

            // Проверяем наличие исполняемого файла на диске.
            // Приложение может быть установлено, но сам файл удален
            return File.Exists(dllPath);
        }

        #endregion
    }
}