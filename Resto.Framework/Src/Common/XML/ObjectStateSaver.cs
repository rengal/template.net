using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Framework.Xml
{
    /// <summary>
    /// Состояние загрузки объекта
    /// </summary>
    public enum LoadState
    {
        /// <summary>
        /// Объект восстановлен без ошибок из предыдущего состояния
        /// </summary>
        Normal,

        /// <summary>
        /// Предыдущее состояние отсутствует
        /// </summary>
        IsNew,

        /// <summary>
        /// Предыдущее состояние повреждено
        /// </summary>
        Corrupt,

        /// <summary>
        /// Целостность приложения нарушена
        /// </summary>
        AppError,
    }

    /// <summary>
    /// Класс для сохранения / восстановления объектов с помощью 
    /// стандартных механизмов сериализации
    /// </summary>
    public static class ObjectStateSaver
    {
        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(ObjectStateSaver));

        // кэшированные сериализаторы для классов
        private static readonly ThreadSafeCache<Type, XmlSerializer> Serializers = new ThreadSafeCache<Type, XmlSerializer>(serializedType => new XmlSerializer(serializedType));

        /// <summary>
        /// Возвратить сериализатор для класса
        /// </summary>
        /// <param name="serializedType">Тип для сериализации</param>
        /// <returns></returns>
        private static XmlSerializer CheckType(Type serializedType)
        {
            return Serializers[serializedType];
        }

        #region Сохранение
        /// <summary>
        /// Сохранить состояние объекта в файл
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="fileName">Название файла</param>
        /// <param name="savedObject">Объект для сохранения</param>
        /// <returns>true - объект сохранен / false - объект не сохранен</returns>
        public static bool SaveObjectState<T>(string fileName, T savedObject)
        {
            try
            {
                var serializer = CheckType(savedObject.GetType());
                using (var fs = new FileStream(fileName, FileMode.Create))
                {
                    serializer.Serialize(fs, savedObject);
                }
                return true;
            }
            catch (Exception e)
            {
                logWrapper.Log.Error("Error on save object state: " + e);
                return false;
            }
        }

        /// <summary>
        /// Сохранить состояние объекта в XmlWriter
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="writer">экземпляр XmlWriter</param>
        /// <param name="savedObject">Объект для сохранения</param>
        /// <returns>true - объект сохранен / false - объект не сохранен</returns>
        public static bool SaveObjectState<T>(XmlWriter writer, T savedObject)
        {
            try
            {
                var serializer = CheckType(savedObject.GetType());
                serializer.Serialize(writer, savedObject);
                return true;
            }
            catch (Exception e)
            {
                logWrapper.Log.Error("Error on save object state: " + e);
                return false;
            }
        }

        /// <summary>
        /// Сохранить состояние объекта в TextWriter.
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="writer">экземпляр TextWriter</param>
        /// <param name="savedObject">Объект для сохранения</param>
        /// <returns>true - объект удалось сохранить, false - объект не удалось сохранить</returns>
        /// <remarks>Используется в проекте iiko.Updater.TorrentManager.csproj</remarks>
        [UsedImplicitly]
        public static bool SaveObjectState<T>(TextWriter writer, T savedObject)
        {
            try
            {
                SaveObjectStateUnsafe(writer, savedObject);
                return true;
            }
            catch (Exception e)
            {
                logWrapper.Log.Error("Error on save object state: " + e);
                return false;
            }
        }

        /// <summary>
        /// Сохранить состояние объекта в TextWriter.
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="writer">экземпляр TextWriter</param>
        /// <param name="savedObject">Объект для сохранения</param>
        internal static void SaveObjectStateUnsafe<T>(TextWriter writer, T savedObject)
        {
            var serializer = CheckType(savedObject.GetType());
            serializer.Serialize(writer, savedObject);
        }

        /// <summary>
        /// Сохранить состояние объекта в StringBuilder (для отладки)
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="savedObject">Объект для сохранения</param>
        public static void SaveObjectState(StringBuilder sb, object savedObject)
        {
            var type = savedObject.GetType();
            var propInfos = type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (var pi in propInfos)
            {
                sb.AppendFormat("{0,-30}: {1,-30}{2}", pi.Name, pi.GetValue(savedObject, null), Environment.NewLine);
            }
        }
        #endregion

        #region Загрузка

        #region Obsolete
        public static bool LoadObjectState<T>(string fileName, out T restoredObject)
        {
            return LoadObject(fileName, typeof(T), out restoredObject) == LoadState.Normal;
        }

        public static bool LoadObjectState<T>(string fileName, Type restoredObjectType,
            out T restoredObject)
        {
            return LoadObject(fileName, restoredObjectType, out restoredObject) == LoadState.Normal;
        }

        public static bool LoadObjectState<T>(XmlReader reader, Type restoredObjectType,
            out T restoredObject)
        {
            return LoadObject(reader, restoredObjectType, out restoredObject) == LoadState.Normal;
        }
        #endregion

        /// <summary>
        /// Загрузить объект из файла
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="fileName">Название файла</param>
        /// <param name="restoredObject">Загруженный объект</param>
        /// <returns>true - объект загружен успешно / false - при загрузке объекта произошла ошибка</returns>         
        public static LoadState LoadObject<T>(string fileName, out T restoredObject)
        {
            return LoadObject(fileName, typeof(T), out restoredObject);
        }

        /// <summary>
        /// Загрузить объект из файла
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="fileName">Название файла</param>
        /// <param name="restoredObjectType">Тип объекта (для загрузки классов-наследников)</param>
        /// <param name="restoredObject">Загруженный объект</param>
        /// <returns>состояние загрузки объекта</returns>
        public static LoadState LoadObject<T>(string fileName, Type restoredObjectType, out T restoredObject)
        {
            try
            {
                return LoadObjectUnsafe(fileName, restoredObjectType, out restoredObject);
            }
            catch (Exception e)
            {
                logWrapper.Log.ErrorFormat("Error on load object state ['{0}']: {1}", fileName, e);
                restoredObject = default;
                return LoadState.Corrupt;
            }
        }

        public static LoadState LoadObjectUnsafe<T>(string fileName, Type restoredObjectType, out T restoredObject)
        {
            if (!File.Exists(fileName))
            {
                restoredObject = default;
                return LoadState.IsNew;
            }

            var serializer = CheckType(restoredObjectType);
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                restoredObject = (T)serializer.Deserialize(fs);
                return LoadState.Normal;
            }
        }

        /// <summary>
        /// Загрузить объект из XmlReader
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="reader">Экземпляр XmlReader</param>
        /// <param name="restoredObjectType">Тип объекта (для загрузки классов-наследников)</param>
        /// <param name="restoredObject">Загруженный объект</param>
        /// <returns>состояние загрузки объекта</returns>
        public static LoadState LoadObject<T>(XmlReader reader, Type restoredObjectType, out T restoredObject) => LoadObjectWithLogger(reader, restoredObjectType, logWrapper.Log, out restoredObject);

        /// <summary>
        /// Загрузить объект из XmlReader
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="reader">Экземпляр XmlReader</param>
        /// <param name="restoredObjectType">Тип объекта (для загрузки классов-наследников)</param>
        /// <param name="log">Сторонний логгер</param>
        /// <param name="restoredObject">Загруженный объект</param>
        /// <returns>состояние загрузки объекта</returns>
        internal static LoadState LoadObjectWithLogger<T>(XmlReader reader, Type restoredObjectType, ILog log, out T restoredObject)
        {
            restoredObject = default;
            try
            {
                var serializer = CheckType(restoredObjectType);
                restoredObject = (T)serializer.Deserialize(reader);
                return LoadState.Normal;
            }
            catch (InvalidOperationException e)
            {
                var inner = e.InnerException;
                ConfigurationErrorsException configEx = null;

                while (inner != null)
                {
                    configEx = inner as ConfigurationErrorsException;
                    if (configEx != null)
                        break;
                    
                    inner = inner.InnerException;
                }

                if (configEx != null)
                {
                    log.Error("Configuration error: " + e);
                    return LoadState.AppError;
                }

                log.Error("Error on load object state: " + e);
                return LoadState.Corrupt;
            }
        }

        /// <summary>
        /// Загрузить объект из XmlReader
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="reader">Экземпляр XmlReader</param>
        /// <param name="restoredObjectType">Тип объекта (для загрузки классов-наследников)</param>
        /// <param name="restoredObject">Загруженный объект</param>
        /// <returns>состояние загрузки объекта</returns>
        public static LoadState LoadObject<T>(TextReader reader, Type restoredObjectType, out T restoredObject) where T : class
        {
            restoredObject = default;
            try
            {
                var serializer = CheckType(restoredObjectType);
                restoredObject = (T)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                logWrapper.Log.Error("Error on load object state: " + e);
            }
            return restoredObject == null ? LoadState.Corrupt : LoadState.Normal;
        }
        #endregion
    }
}