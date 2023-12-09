using System;
using System.IO;
using System.Text;
using log4net;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Вспомогательный класс для работы с уникальными именами объектов
    /// </summary>
    public static class UniqueFileNameHelper
    {
        #region Delegates

        private delegate string ToBase32StringSuitableForDirNameDelegate(byte[] buff);

        #endregion

        #region Consts

        // Разделитель частей имени файла
        private const string PARTS_SEPARATOR = "_";
        // Формат даты, которая будет включена в имя файла
        private const string DATE_FORMAT_STRING = "yyyyMMddHHmmss";
        // максимальное значение для случайной части имени
        private const int SALT_LENGTH = 8;

        #endregion

        #region Fields / Properties

        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(UniqueFileNameHelper));
        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private static ILog LOG
        {
            get { return logWrapper.Log; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверить имя файла на допустимость
        /// </summary>
        /// <param name="fileName">имя файла</param>
        private static void CheckFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                throw new RestoException("Source file name must be defined.");
            }           
        }

        /// <summary>
        /// Проверить тип-владелец данных на допустимость
        /// </summary>
        /// <param name="ownerType"></param>
        private static void CheckOwnerType(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new RestoException("Owner type must be defined.");
            }
        }

        /// <summary>
        /// Получить уникальное имя файла или маску для поиска объекта 
        /// </summary>
        /// <param name="sourceFileName">Исходное имя (заготовка)</param>
        /// <param name="ownerType">Тип-владелец данных</param>
        /// <param name="isSearchMask">true - создать маску для поиска (без даты) / false - создать полное имя</param>
        /// <returns>Имя файла / маска для поиска</returns>
        private static string GetUniqueFileName(
            string sourceFileName, Type ownerType, bool isSearchMask)
        {
            // проверяем корректность исходных данных
            CheckFileName(sourceFileName);
            CheckOwnerType(ownerType);
            // получаем из заготовки основную часть имени (без расширения)
            var mainPart = Path.GetFileNameWithoutExtension(sourceFileName);
            // получаем часть для хэша владельца данных
            var ownerHashPart = Math.Abs(ownerType.Name.GetHashCode()).ToString();
            // получаем расширение (из заготовки)
            var extension = Path.GetExtension(sourceFileName);
            // если генерируется маска - данных уже достаточно, создаем и возвращаем маску
            if (isSearchMask)
            {
                return String.Concat(
                    mainPart, PARTS_SEPARATOR,
                    ownerHashPart, PARTS_SEPARATOR,
                    "*", extension);
            }
            // получаем часть для даты/времени
            var dateTimePart = DateTime.Now.ToUniversalTime().ToString(DATE_FORMAT_STRING);
            // формируем и возвращаем полное уникальное имя файла
            var sb = new StringBuilder(50);
            sb.Append(mainPart);
            sb.Append(PARTS_SEPARATOR);
            sb.Append(ownerHashPart);
            sb.Append(PARTS_SEPARATOR);
            sb.Append(dateTimePart);
            sb.Append(extension);
            return sb.ToString();
        }

        /// <summary>
        /// Получить уникальное имя файла для объекта ([MainPart]_[OwnerTypeHash]_[Date].[extension])
        /// </summary>
        /// <param name="sourceFileName">Исходное имя (заготовка)</param>
        /// <param name="ownerType">Тип-владелец данных</param>
        /// <returns>Имя файла</returns>
        public static string GetUniqueFileName(string sourceFileName, Type ownerType)
        {
            return GetUniqueFileName(sourceFileName, ownerType, false);
        }

        /// <summary>
        /// Получить уникальное имя файла или маску для поиска объекта 
        /// 
        /// ([MainPart]_[OwnerTypeHash]_*.[Extension])
        /// </summary>
        /// <param name="sourceFileName">Исходное имя (заготовка)</param>
        /// <param name="ownerType">Тип-владелец данных</param>
        /// <returns>Маска для поиска</returns>
        public static string GetSearchMask(string sourceFileName, Type ownerType)
        {
            return GetUniqueFileName(sourceFileName, ownerType, true);
        }

        /// <summary>
        /// Получить основные составные части из имени файла
        /// </summary>
        /// <param name="uniqueFileName">Имя файла</param>
        /// <param name="ownerTypeHash">Хэш названия типа владельца данных</param>
        /// <param name="creationDate">Дата создания файла (локальное время)</param>
        /// <param name="useSalt">Включена ли в имя случайная часть</param>
        /// <returns>true - имя файла удалось обработать / false - не удалось</returns>
        public static bool ParseUniqueFileName(string uniqueFileName, out int ownerTypeHash, 
            out DateTime creationDate, bool useSalt)
        {
            // проверяем корректность имени
            CheckFileName(uniqueFileName);
            // инициализируем out параметры
            ownerTypeHash = 0;
            creationDate = DateTime.MinValue;
            var fileName = Path.GetFileNameWithoutExtension(uniqueFileName);
            // разбиваем имя на составные части
            var fileNameParts = fileName.Split(
                new[] {PARTS_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries);
            var partsCount = fileNameParts.Length;
            // определяем индексы интересующих нас частей
            var minPartsCount = useSalt ? 4 : 3;
            var datePartInd = useSalt ? partsCount - 2 : partsCount - 1;
            var ownerTypeHashInd = useSalt ? partsCount - 3 : partsCount - 2;
            // проверяем минимально допустиоме количество частей
            if (partsCount < minPartsCount)
            {
                LOG.WarnFormat("Can not parse file name '{0}'. Invalid parts count: {1}",
                               uniqueFileName, fileNameParts.Length);
                return false;
            }
            try
            {
                // пытаемся выделить значения частей
                creationDate = DateTime.ParseExact(fileNameParts[datePartInd], DATE_FORMAT_STRING, null).ToLocalTime();
                ownerTypeHash = Int32.Parse(fileNameParts[ownerTypeHashInd]);
                return true;
            }
            catch (Exception e)
            {
                // ошибки пишем в лог
                LOG.WarnFormat("Can not parse file name '{0}': {1}", uniqueFileName, e.Message);
                return false;
            }
        }

        #endregion
    }
}