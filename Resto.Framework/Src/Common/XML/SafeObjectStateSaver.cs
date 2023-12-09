using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;
using log4net;
using log4net.Core;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.Log;

namespace Resto.Framework.Xml
{
    /// <summary>
    /// Класс для безопасного сохранения/загрузки файлов. Сохранение использует временные файл для предыдущего состояния 
    /// и принудительный сброс дисковых буферов операционной системы
    ///  </summary>
    public static partial class SafeObjectStateSaver
    {
        #region Общие
        /// <summary>
        /// Словарь расширений для основного и временного файлов
        /// </summary>
        private static readonly IReadOnlyDictionary<string, string> FileExtensions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {".xml", ".oxml"},
            {".config",".oconfig"}
        };

        private static readonly LogWrapper LogWrapper = new LogWrapper(typeof(SafeObjectStateSaver));
        #endregion

        #region Сохранение

        /// <summary>
        /// Сохранить состояние объекта в файл
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="obj">Объект, состояние которого нужно сохранить</param>
        /// <exception cref="ArgumentException">Расширение файла не поддерживается</exception>
        /// <exception cref="Exception">Проблемы с сохранением файла</exception>
        public static void SaveToFile([NotNull] string filePath, [NotNull] object obj)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (!FileExtensions.TryGetValue(Path.GetExtension(filePath), out var tempExtension))
                throw new ArgumentException($"The file extension {Path.GetExtension(filePath)} is not supported.", nameof(filePath));

            // формируем полный путь к временному файлу
            var tmpFilePath = Path.ChangeExtension(filePath, tempExtension);

            // если существует предыдущее состояние
            if (File.Exists(filePath))
            {
                // удаляем предыдущий временный файл
                ForceDeleteFile(tmpFilePath, LogWrapper.Log);
                // делаем предыдущее состояние временным файлом
                File.Move(filePath, tmpFilePath);
            }

            // сохраняем состояние объекта в файл с использованием внешнего сериализатора
            try
            {
                const int bufferSize = 1024 * 4; // Размер буфера, который по умолчанию использует FileStream, одна страница памяти

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, FileOptions.WriteThrough))
                using (var writer = new StreamWriter(fileStream))
                {
                    ObjectStateSaver.SaveObjectStateUnsafe(writer, obj);
                }
            }
            catch
            {
                LogWrapper.Log.Error($"Can not save object '{obj.GetType()}' to file '{filePath}'");
                throw;
            }

            // если запись завершена - удаляем временный файл
            ForceDeleteFile(tmpFilePath, LogWrapper.Log);
        }
        #endregion

        #region Загрузка

        /// <summary>
        /// Загрузить состояние объекта из файла
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="log">Сторонний логгерs=</param>
        /// <param name="obj">Объект</param>
        /// <returns>Состояние загрузки</returns>
        private static LoadState LoadFromFile<T>([NotNull] string filePath, ILog log, out T obj)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            obj = default;

            // если файла для объекта еще нет - говорим о том, что он новый
            if (!File.Exists(filePath))
                return LoadState.IsNew;

            LoadState result;
            try
            {
                // пытаемся десериализовать состояние с помощью внешнего десериализатора
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(stream))
                {
                    result = ObjectStateSaver.LoadObjectWithLogger(new XmlTextReader(reader), typeof(T), log, out obj);
                }
            }
            catch (Exception e)
            {
                log.Error($"Error on load object from file '{filePath}'", e);
                result = LoadState.Corrupt;
            }

            if (result == LoadState.Corrupt)
            {
                SaveCorruptedFileContentToLog(filePath, log);
            }
            return result;
        }

        /// <summary>
        /// Загрузить состояние объекта из файла
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="obj">Восстанавливаемый объект</param>
        /// <returns>Состояние загрузки</returns>
        /// <exception cref="ArgumentException">Расширение файла не поддерживается</exception>
        public static LoadState TryLoadFromFileSafe<T>([NotNull] string filePath, out T obj)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            return TryLoadFromFileSafeWithLogger(filePath, LogWrapper.Log, out obj);
        }

        /// <summary>
        /// Загрузить состояние объекта из файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="obj">Восстанавливаемый объект</param>
        /// <returns>Состояние загрузки</returns>
        /// <exception cref="ArgumentException">Расширение файла не поддерживается</exception>
        public static (LoadState state, LoggingEvent[] logEvents) TryLoadFromFileSafeWithLogEvents<T>([NotNull] string filePath, out T obj)
        {
            using (var context = new MemoryLoggerContext())
            {
                var state = TryLoadFromFileSafeWithLogger(filePath, context.Logger, out obj);
                return (state, context.PopAllEvents());
            }
        }

        /// <summary>
        /// Загрузить состояние объекта из файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="log">Сторонний логгер</param>
        /// <param name="obj">Восстанавливаемый объект</param>
        /// <returns>Состояние загрузки</returns>
        /// <exception cref="ArgumentException">Расширение файла не поддерживается</exception>
        private static LoadState TryLoadFromFileSafeWithLogger<T>([NotNull] string filePath, [NotNull] ILog log, out T obj)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            if (!FileExtensions.TryGetValue(Path.GetExtension(filePath), out var tempExtension))
                throw new ArgumentException($"The file extension {Path.GetExtension(filePath)} is not supported.", nameof(filePath));

            obj = default;

            try
            {
                // пытаемся загрузить из обычного файла
                var result = LoadState.IsNew;
                if (File.Exists(filePath))
                {
                    result = LoadFromFile(filePath, log, out obj);
                    if (result == LoadState.Normal)
                    {
                        // если удалось - стираем временный
                        // отключено для ускорения запуска фронта, см. RMS-18212 (временный файл и так затрется при следующем сохранении или удалении основного файла)
                        // ForceDeleteFile(tempFilePath);
                        return result;
                    }
                }

                // формируем полный путь к временному файлу
                var tempFilePath = Path.ChangeExtension(filePath, tempExtension);

                // если не удалось - пытаемся загрузить из временного файла
                if (File.Exists(tempFilePath))
                {
                    result = LoadFromFile(tempFilePath, log, out obj);
                    if (result == LoadState.Normal)
                    {
                        // переименовываем временный в основной
                        ForceDeleteFile(filePath, log);
                        File.Move(tempFilePath, filePath);
                        log.Debug($"File '{Path.GetFileName(filePath)}' restored from previous state");
                        return result;
                    }
                }

                return result;

            }
            catch (Exception e)
            {
                // в случае исключений при загрузке сообщаем о том, что файл поврежден
                log.Error($"Unhandled exception on '{Path.GetFileName(filePath)}' loading", e);
                return LoadState.Corrupt;
            }
        }

        /// <summary>
        /// Удалить основной и временный файлы
        /// </summary>
        /// <param name="filePath">Полный путь файла</param>
        public static void DeleteOriginalAndBackupFiles([NotNull] string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));
            if (!FileExtensions.TryGetValue(Path.GetExtension(filePath), out var tempExtension))
                throw new ArgumentException($"The file extension {Path.GetExtension(filePath)} does not supported.", nameof(filePath));

            if (File.Exists(filePath))
            {
                ForceDeleteFile(filePath, LogWrapper.Log);
            }

            var tempFilePath = Path.ChangeExtension(filePath, tempExtension);
            if (File.Exists(tempFilePath))
            {
                ForceDeleteFile(tempFilePath, LogWrapper.Log);
            }
        }

        /// <summary>
        /// Сохранить повреждённый файл в лог
        /// </summary>
        /// <param name="filePath">Полный путь файла</param>
        /// <param name="log">Сторонний логгер</param>
        private static void SaveCorruptedFileContentToLog(string filePath, ILog log)
        {
            try
            {
                log.Trace($"Content of the corrupted file '{filePath}':{Environment.NewLine}{File.ReadAllText(filePath)}{Environment.NewLine}End of content of the corrupted file '{filePath}'");
            }
            catch (Exception e)
            {
                log.Error($"Cannot read file: {filePath}", e);
            }
        }
        #endregion

        #region Удаление
        /// <summary>
        /// Попытаться удалить файл (игнорируются все исключения, кроме ThreadAbortException)
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="log">Сторонний логгер</param>
        private static void ForceDeleteFile([NotNull] string filePath, ILog log)
        {
            Debug.Assert(filePath != null);

            try
            {
                for (var i = 1; ; i++)
                {
                    try
                    {
                        File.Delete(filePath);
                        break;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // RMS-28329 и RMS-34077: повторные попытки удалить файл
                        // 
                        // Иногда операция удаления только что созданного файла приводит к исключению UnauthorizedAccessException 
                        // из-за особенностей реализации ядра Windows или кратковременной блокировки файла другими процессами (обычно антивирусом).
                        // Многие проекты обходят эту проблему, выполняя несколько попыток (гарантии нет, но по вероятности проблема решается).
                        const int retryCount = 10;
                        var retryDelay = TimeSpan.FromMilliseconds(500);

                        if (i < retryCount)
                        {
                            Thread.Sleep(retryDelay);
                            continue;
                        }
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error($"Can not delete file '{filePath}'", e);
            }
        }
        #endregion
    }
}