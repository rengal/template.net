using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using Resto.Data;
using Resto.Framework.Common;
using Timer = System.Timers.Timer;

namespace Resto.Common
{
    /// <summary>
    /// Предоставляет функционал для сохранения файла кэша в архив, а так же удаления устаревших архивов кэша.
    /// Настройки формируются в Администрирование -> Системные настройки.
    /// </summary>
    public class EntitiesCacheArchiver : IDisposable
    {
        #region Fields

        private readonly ILog log;
        private readonly string сacheFilePath;
        private readonly string dailyCacheFolderPath;
        private readonly EntitiesCacheScheduler scheduler;
        private readonly Regex cacheFilesDateRegex = new Regex(@"\d{4}\.\d{2}\.\d{2}");

        private static EntitiesCacheArchiver instance;

        private bool dailyCacheSavingEnabled;
        private int archivingInProgress;
        private TimeSpan cacheSavingTime;

        #endregion

        #region Constructors

        private EntitiesCacheArchiver()
        {
            сacheFilePath = RMSEntityManager.GetCacheFilePath();
            dailyCacheFolderPath = Path.GetDirectoryName(сacheFilePath);
            cacheSavingTime = TimeSpan.FromMinutes(CommonConfig.Instance.CacheSavingMinutesTime);
            dailyCacheSavingEnabled = CommonConfig.Instance.DailyCacheSaving;

            log = LogFactory.Instance.GetLogger(GetType());
            scheduler = new EntitiesCacheScheduler();
            scheduler.SetParameters(cacheSavingTime, () =>
            {
                log.Info("Scheduled archiving.");
                ProcessArchiveCache();
            });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает экземпляр архиватора.
        /// </summary>
        public static EntitiesCacheArchiver Instance => instance ?? (instance = new EntitiesCacheArchiver());

        /// <summary>
        /// Время ежедневной выгрузки кэша.
        /// </summary>
        public TimeSpan CacheSavingTime
        {
            get { return cacheSavingTime; }
            set
            {
                cacheSavingTime = value;
                scheduler.SetParameters(cacheSavingTime);
            }
        }

        /// <summary>
        /// Ежедневное сохранение кэша доступно.
        /// </summary>
        public bool DailyCacheSavingEnabled
        {
            get { return dailyCacheSavingEnabled; }
            set
            {
                dailyCacheSavingEnabled = value;
                if (dailyCacheSavingEnabled)
                {
                    scheduler.StartInTimeExecution();
                }
                else
                {
                    scheduler.StopInTimeExecution();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Работа с кэшем:
        /// - Архивация кэша;
        /// - Удаление устаревших архивов.
        /// </summary>
        /// <param name="needToCheckPrevDayArchive">Проверка наличия вчерашнего архива с кэшем</param>
        /// <remarks>Кэш архивируется при закрытии бэка, при открытии кэш архивируется в случае, если нет вчерашнего архива.</remarks>
        public void ProcessArchiveCache(bool needToCheckPrevDayArchive = false)
        {
            if (Interlocked.Exchange(ref archivingInProgress, 1) == 1)
            {
                return;
            }

            Task.Factory
                .StartNew(() =>
                {
                    if (!Directory.Exists(dailyCacheFolderPath))
                    {
                        Directory.CreateDirectory(dailyCacheFolderPath);
                    }

                    if (dailyCacheSavingEnabled)
                    {
                        ZipArchiveCache(needToCheckPrevDayArchive);
                    }

                    ZipDeleteOldCache();
                }, TaskCreationOptions.LongRunning)
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        task.Exception.Flatten().Handle(exception =>
                        {
                            var exceptionMessage = string.Format("{0}. {1}", exception.Message,
                                exception.InnerException == null ? string.Empty : exception.InnerException.Message);
                            log.Warn(exceptionMessage);
                            return true;
                        });
                    }

                    Interlocked.Exchange(ref archivingInProgress, 0);
                });
        }

        /// <summary>
        /// Архивирует кэш.
        /// </summary>        
        /// <param name="needToCheckPrevDayArchive">Проверка наличия вчерашнего архива с кэшем</param>
        /// <remarks>Кэш архивируется при закрытии бэка, при открытии кэш архивируется в случае, если нет вчерашнего архива.</remarks>
        private void ZipArchiveCache(bool needToCheckPrevDayArchive = false)
        {
            var zipFilePath = CreateZipFilePath(DateTime.Today);
            if (File.Exists(zipFilePath))
            {
                log.Info(string.Format("Archiving won't be processed. {0} is already exists.", zipFilePath));
                return;
            }

            if (needToCheckPrevDayArchive && File.Exists(CreateZipFilePath(DateTime.Today.AddDays(-1))))
            {
                return;
            }

            try
            {
                log.Info("Start archiving cache.");
                ZipHelper.ZipFile(zipFilePath, сacheFilePath, safeMode: true);
                log.Info(string.Format("{0} was created.", zipFilePath));
            }
            catch (Exception e)
            {
                log.Warn("Error during cache archiving.", e);
                // ZipHelper сначала создает пустой архив, а потом уже добавляет в него файлы.
                // Поэтому удаляем пустой архив.
                File.Delete(zipFilePath);
            }
        }

        /// <summary>
        /// Удаляет устаревшие архивы кэша.
        /// </summary>
        private void ZipDeleteOldCache()
        {
            var lastCacheDay = DateTime.Today.AddDays(-CommonConfig.Instance.CacheSavingPeriod);
            var mask = string.Format("*-{0}{1}", RMSEntityManager.CacheFileName, ZipHelper.ZipExtension);
            var files = Directory.GetFiles(dailyCacheFolderPath, mask);

            try
            {
                log.Info("Start deleting old cache archives.");
                if (HasFilesToDelete(files, lastCacheDay))
                {
                    foreach (var file in files)
                    {
                        var fileDate = ExtractDate(file);
                        if (!fileDate.HasValue || fileDate.Value.Date > lastCacheDay.Date)
                        {
                            continue;
                        }

                        File.Delete(file);
                        log.Info(string.Format("'{0}' was deleted.", file));
                    }
                }
                else
                {
                    log.Info("Nothing to delete.");
                }
            }
            catch (Exception e)
            {
                log.Warn("Error during cache deleting.", e);
            }
        }

        /// <summary>
        /// Проверка наличия файлов на удаление в <paramref name="files"/>, удовлетворяющих
        /// <paramref name="lastCacheDay"/>
        /// </summary>
        /// <returns><c>true</c>, если есть файлы на удаление, иначе - <c>false</c></returns>
        private bool HasFilesToDelete(IReadOnlyList<string> files, DateTime lastCacheDay)
        {
            return files.Any(file =>
            {
                var fileDate = ExtractDate(files[0]);
                return fileDate.HasValue && fileDate.Value.Date <= lastCacheDay.Date;
            });
        }

        /// <summary>
        /// Возвращает дату, содержащуюся в имени <paramref name="file"/>.
        /// </summary>
        [CanBeNull]
        private DateTime? ExtractDate(string file)
        {
            return cacheFilesDateRegex.Match(file).Success ? File.GetCreationTime(file) : (DateTime?) null;
        }

        /// <summary>
        /// На основе <paramref name="date"/> создает путь для файла архива кэша и возвращает его.
        /// </summary>
        private string CreateZipFilePath(DateTime date)
        {
            var zipFileName = string.Format("{0:yyyy.MM.dd}-{1}{2}", date, RMSEntityManager.CacheFileName,
                ZipHelper.ZipExtension);
            return Path.Combine(dailyCacheFolderPath, zipFileName);
        }

        public void Dispose()
        {
            scheduler.StopInTimeExecution();
            instance = null;
        }

        #endregion

        #region Nested type EntitiesCacheArchiverScheduler

        /// <summary>
        /// Предоставляет функционал выполнения действий в определенный момент времени.
        /// </summary>
        private sealed class EntitiesCacheScheduler
        {
            #region Constants

            /// <summary>
            /// Интервал срабатывания таймера.
            /// </summary>
            /// <remarks>1 минута</remarks>
            private const int Interval = 60000;

            #endregion

            #region Fields

            /// <summary>
            /// Таймер.
            /// </summary>
            private readonly Timer timer;

            /// <summary>
            /// Время выполнения <see cref="action"/>.
            /// </summary>
            private TimeSpan executingTime;

            /// <summary>
            /// Действие, выполняемое в момент времени <see cref="executingTime"/>.
            /// </summary>
            private Action action;

            /// <summary>
            /// Таймер уже запущен.
            /// </summary>
            private bool isAlreadyStarted;

            #endregion

            #region Constructors

            /// <summary>
            /// Конструктор.
            /// </summary>
            public EntitiesCacheScheduler()
            {
                timer = new Timer(Interval);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Устанавливает действие и время его выполнения.
            /// </summary>
            public void SetParameters(TimeSpan? time = null, Action currentAction = null)
            {
                if (time.HasValue)
                {
                    executingTime = time.Value;
                }

                if (currentAction == null)
                {
                    return;
                }

                action = currentAction;
                StartInTimeExecution();
            }

            /// <summary>
            /// Запускает выполнение.
            /// </summary>
            public void StartInTimeExecution()
            {
                if (isAlreadyStarted)
                {
                    return;
                }

                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                isAlreadyStarted = true;
            }

            /// <summary>
            /// Прекращает выполнение.
            /// </summary>
            public void StopInTimeExecution()
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                isAlreadyStarted = false;
            }

            /// <summary>
            /// По прерыванию таймера выполняется <see cref="action"/>.
            /// </summary>
            private void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                var currentMinutes = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMinutes);
                if ((int) executingTime.TotalMinutes == currentMinutes)
                {
                    action();
                }
            }

            #endregion
        }

        #endregion
    }
}
