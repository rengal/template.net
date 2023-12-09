using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Resto.Framework.Attributes.JetBrains;
using log4net.Core;
using Resto.Framework.Common;

namespace log4net.Appender
{
    /// <summary>
    /// Наследник RollingFileAppender - выполняет сжатие старого лог-файла после переключения Appender'a на новый.
    /// <p/>
    /// Формат даты в названиях логов и их архивов дублируется в сборщике логов (https://wiki.iiko.ru/pages/viewpage.action?pageId=50168066):
    /// <p/>
    /// <c>\dev\Troubleshooter\Troubleshooter.sln -></c>
    /// <list type="bullet">
    /// <item><c>Resto.Troubleshooter.Core.DataCollectors.FrontLogsCollector</c></item>
    /// <item><c>Resto.Troubleshooter.Core.DataCollectors.BackOfficeLogsCollector</c></item>
    /// <item><c>Resto.Troubleshooter.Core.DataCollectors.ChainOfficeLogsCollector</c></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Текущий формат даты через дефис был добавлен в версии 2.2 в задаче https://jira.iiko.ru/browse/RMS-23667.
    /// </remarks>
    public class ZippedRollingFileAppender : RollingFileAppender
    {
        private const string DateSuffixPattern = ".yyyy-MM-dd";
        private static readonly Lazy<Regex> FileNameWithoutDateRegex = new(() => new Regex(@"(.*)\.(\d{4}-\d{2}-\d{2})(.*)", RegexOptions.Compiled));
        private static readonly Lazy<Regex> LogFilesDateRegex = new(() => new Regex(@"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})", RegexOptions.Compiled));

        private static readonly Func<RollingFileAppender, string> ScheduledNameFieldGetter = GetScheduledNameFieldGetter();

        private int logFileAgeDays;

        [UsedImplicitly] // может задаваться через конфиг
        public bool AutoCompress { get; set; }

        [UsedImplicitly] // может задаваться через конфиг
        public int LogFileAgeDays
        {
            get { return logFileAgeDays; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "LogFileAgeDays must be nonnegative value");
                logFileAgeDays = value;
            }
        }

        public ZippedRollingFileAppender()
        {
            // устанавливаем параметры appender'a
            // ..тип - по дате 
            RollingStyle = RollingMode.Date;
            // ..формат даты, приписываемой к имени файла "yyyy-MM-dd"
            DatePattern = DateSuffixPattern;
            Encoding = Encoding.UTF8;
            PreserveLogFileNameExtension = true;
            // .. максимально допустимый размер после которого будет выполнено переключение
            //MaxFileSize = MAX_LOG_SIZE;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="autoCompress">Выполнять ли автоархивацию</param>
        /// <param name="logFileAgeDays">Сколько дней хранятся старые зазипованные логи</param>
        public ZippedRollingFileAppender(bool autoCompress, int logFileAgeDays)
            : this()
        {
            if (logFileAgeDays < 0)
                throw new ArgumentOutOfRangeException(nameof(logFileAgeDays), logFileAgeDays, "logFileAgeDays must be nonnegative value");

            AutoCompress = autoCompress;
            this.logFileAgeDays = logFileAgeDays;
        }

        /// <summary>
        /// Получить имя файла, куда будет сброшен текущий лог при выполнении условий проверки
        /// </summary>
        private string GetScheduledName()
        {
            return ScheduledNameFieldGetter(this);
        }

        /// <summary>
        /// Перекрытый метод инициализации
        /// </summary>
        public override void ActivateOptions()
        {
            //вызываем базовый метод - он может выполнить переключение LOG-файла
            base.ActivateOptions();
            // если автосжатие не используется - ничего не делаем
            if (!AutoCompress)
                return;

            // получаем все файлы из текущего каталога лога, имя которых подобно имени лога
            var logDir = Path.GetDirectoryName(File);
            var curFile = Path.GetFileName(File);
            var logSearchMask = Path.GetFileNameWithoutExtension(File) + ".*" + ".*";
            var files = Directory.GetFiles(logDir, logSearchMask);
            // проверяем каждый - не нужно ли его сжать?
            foreach (var file in files)
            {
                // если это текущий лог - сжимать не нужно
                if (StringComparer.InvariantCultureIgnoreCase.Compare(curFile, Path.GetFileName(file)) == 0)
                    continue;
                // если файл уже сжат - сжимать не нужно
                var ext = Path.GetExtension(file);
                if (StringComparer.InvariantCultureIgnoreCase.Compare(ext, ZipHelper.ZipExtension) == 0)
                    continue;
                // сжимаем файл
                ZipLog(file);
            }
            Task.Factory.StartNew(() => DeleteOldZippedLogs(File, logFileAgeDays), TaskCreationOptions.LongRunning);
        }

        private void DeleteOldZippedLogs(string logFile, int ageDays)
        {
            // крайняя дата, до которой старые логи удаляются
            var utmostDate = DateTime.Today.AddDays(-ageDays);
            // получаем все zip-файлы из текущего каталога лога, имя которых содержит имя лога
            var logDir = Path.GetDirectoryName(logFile);
            var logSearchMask = "*" + Path.GetFileName(logFile) + "*" + ZipHelper.ZipExtension;
            var files = Directory.GetFiles(logDir, logSearchMask);
            // проверяем дату каждого - не нужно ли удалить?
            foreach (var file in files)
            {
                var match = LogFilesDateRegex.Value.Match(file);
                if (!match.Success)
                    return;
                var dayGroup = match.Groups["day"];
                var monthGroup = match.Groups["month"];
                var yearGroup = match.Groups["year"];
                if (dayGroup.Success && monthGroup.Success && yearGroup.Success)
                {
                    var zippedLogDate = new DateTime(int.Parse(yearGroup.Value), int.Parse(monthGroup.Value), int.Parse(dayGroup.Value));
                    if (zippedLogDate < utmostDate)
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (IOException)
                        { }
                        catch (UnauthorizedAccessException)
                        { }
                }
            }
        }

        private void ZipLog(string logName)
        {
            Task.Factory.StartNew(
                state =>
                {
                    var fileName = (string)state;
                    // архивируем файл
                    ZipHelper.ZipFile(GetZipFileName(fileName), fileName, safeMode: true);
                    // удаляем файл
                    System.IO.File.Delete(fileName);
                },
                logName, TaskCreationOptions.LongRunning
            ).HandleExceptions(_ => { });
        }

        /// <summary>
        /// Перекрытый метод добавления записи в лог
        /// </summary>
        protected override void Append(LoggingEvent loggingEvent)
        {
            // запоминаем текущее имя файла (1), куда будет сброшен текущий лог
            var curScheduledName = GetScheduledName();
            // выполняем метод родительского класса
            base.Append(loggingEvent);
            // если нужно - проверяем, был ли выполнен сброс текущего лога
            // Признак - изменилось имя файла (1)
            if ((AutoCompress) && (GetScheduledName() != curScheduledName))
            {
                ZipLog(curScheduledName);
            }
        }

        /// <summary>
        /// Делает из названия файла вида "*.yyyy-mm-dd", имя вида "yyyy-mm-dd.*" если возможно
        /// </summary>
        private string GetZipFileName([NotNull] string logName)
        {
            if (logName == null)
                throw new ArgumentNullException(nameof(logName));

            var zipFileName = logName;
            var match = FileNameWithoutDateRegex.Value.Match(Path.GetFileName(logName));
            if (match.Success)
            {
                var directory = Path.GetDirectoryName(logName);
                var fileNamePart = match.Groups[1].Value;
                var datePart = match.Groups[2].Value;
                zipFileName = Path.Combine(directory, datePart + "-" + fileNamePart);
            }
            return zipFileName + ZipHelper.ZipExtension;
        }

        [Pure]
        private static Func<RollingFileAppender, string> GetScheduledNameFieldGetter()
        {
            // запоминаем FieldInfo private поля базового класса.
            // Оно будет использоваться для определения факта переключения на новый лог-файл
            var scheduledNameField = typeof(RollingFileAppender).GetField("m_scheduledFilename", BindingFlags.NonPublic | BindingFlags.Instance);
            return ExpressionHelper.GetFieldGetter<RollingFileAppender, string>(scheduledNameField);
        }
    }
}