using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Хэлпер для работы с zip-архивами
    /// </summary>
    public static class ZipHelper
    {
        // дефолтное расширение для файлов
        public const string ZipExtension = ".zip";

        private static readonly string Separator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        #region Упаковка

        /// <summary>
        /// Добавить в архив содержимое папки, включая все вложенные папки и их содержимое
        /// </summary>
        /// <param name="zipFileName">Путь к новому или существующему файлу архива</param>
        /// <param name="sourceDir">Папка, содерждимое которой необходимо добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить данные</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <param name="overwrite">Перезаписывать существующие в архиве записи при совпадении имен</param>
        /// <param name="filter">Фильтр файлов, которые нужно включить в архив</param>
        public static void ZipDir([NotNull] string zipFileName, [NotNull] string sourceDir, [CanBeNull] string archiveDirectory = null, bool safeMode = false, bool overwrite = false, Func<FileSystemInfo, bool> filter = null)
        {
            if (zipFileName == null)
                throw new ArgumentNullException(nameof(zipFileName));

            if (sourceDir == null)
                throw new ArgumentNullException(nameof(sourceDir));

            if (File.Exists(zipFileName))
            {
                ZipDirExisting(zipFileName, sourceDir, archiveDirectory, overwrite, safeMode, filter);
            }
            else
            {
                using (var archive = System.IO.Compression.ZipFile.Open(zipFileName, ZipArchiveMode.Create))
                {
                    archive.CreateEntriesFromDirectory(sourceDir, archiveDirectory, safeMode, filter);
                }
            }
        }

        /// <summary>
        /// Добавить файл в архив
        /// </summary>
        /// <param name="zipFileName">Путь к новому или существующему файлу архива</param>
        /// <param name="sourceFile">Файл, который нужно добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить данные</param>
        /// <param name="safeMode">Безопасный режим (Копирование перед архивацией)</param>
        /// <param name="overwrite">Перезаписывать существующие в архиве записи при совпадении имен</param>
        public static void ZipFile([NotNull] string zipFileName, [NotNull] string sourceFile, [CanBeNull] string archiveDirectory = null, bool safeMode = false, bool overwrite = false)
        {
            if (zipFileName == null)
                throw new ArgumentNullException(nameof(zipFileName));

            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (File.Exists(zipFileName))
            {
                ZipFileExisting(zipFileName, sourceFile, archiveDirectory, overwrite, safeMode);
            }
            else
            {
                using (var archive = System.IO.Compression.ZipFile.Open(zipFileName, ZipArchiveMode.Create))
                {
                    archive.AddFile(sourceFile, archiveDirectory, safeMode);
                }
            }
        }

        /// <summary>
        /// Добавить файлы в архив
        /// </summary>
        /// <param name="zipFileName">Путь к новому или существующему файлу архива</param>
        /// <param name="sourceFiles">Файлы, которые нужно добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить файлы</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <param name="overwrite">Перезаписывать существующие в архиве записи при совпадении имен</param>
        public static void ZipFiles([NotNull] string zipFileName, [NotNull] IEnumerable<string> sourceFiles, [CanBeNull] string archiveDirectory = null, bool safeMode = false, bool overwrite = false)
        {
            if (zipFileName == null)
                throw new ArgumentNullException(nameof(zipFileName));

            if (sourceFiles == null)
                throw new ArgumentNullException(nameof(sourceFiles));

            if (File.Exists(zipFileName))
            {
                ZipFilesExisting(zipFileName, sourceFiles, archiveDirectory, overwrite, safeMode);
            }
            else
            {
                using (var archive = System.IO.Compression.ZipFile.Open(zipFileName, ZipArchiveMode.Create))
                {
                    foreach (var sourceFile in sourceFiles)
                    {
                        archive.AddFile(sourceFile, archiveDirectory, safeMode);
                    }
                }
            }
        }

        /// <summary>
        /// Создать в памяти архив и поместить в него указанный файл
        /// </summary>
        /// <param name="sourceFile">Файл, который нужно добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить файл</param>
        /// <param name="encoding">Кодировка записей в архиве</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <returns>Созданный архив</returns>
        public static byte[] ZipFileInMem([NotNull] string sourceFile, [CanBeNull] string archiveDirectory = null, Encoding encoding = null, bool safeMode = false)
        {
            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            using (var stream = new MemoryStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true, encoding))
                {
                    archive.AddFile(sourceFile, archiveDirectory, safeMode);
                }
                return stream.GetBuffer();
            }
        }

        /// <summary>
        /// Создать в памяти архив и поместить в него указанные файлы
        /// </summary>
        /// <param name="sourceFiles">Файлы, которые нужно добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить файл</param>
        /// <param name="encoding">Кодировка записей в архиве</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <returns>Созданный архив</returns>
        public static byte[] ZipFilesInMem([NotNull] IEnumerable<string> sourceFiles, [CanBeNull] string archiveDirectory = null, Encoding encoding = null, bool safeMode = false)
        {
            if (sourceFiles == null)
                throw new ArgumentNullException(nameof(sourceFiles));

            using (var stream = new MemoryStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true, encoding))
                {
                    foreach (var sourceFile in sourceFiles)
                    {
                        archive.AddFile(sourceFile, archiveDirectory, safeMode);
                    }
                }
                return stream.GetBuffer();
            }
        }

        /// <summary>
        /// Создать в памяти архив и поместить в него содержимое указанной папки
        /// </summary>
        /// <param name="sourceDir">Папка, которую необходимо добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить данные</param>
        /// <param name="encoding">Кодировка записей в архиве</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <returns>Созданный архив</returns>
        public static byte[] ZipDirInMem([NotNull] string sourceDir, [CanBeNull] string archiveDirectory = null, Encoding encoding = null, bool safeMode = false)
        {
            if (sourceDir == null)
                throw new ArgumentNullException(nameof(sourceDir));

            if (!Directory.Exists(sourceDir))
                throw new ArgumentException($"Directory '{sourceDir}' not exists");

            using (var stream = new MemoryStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true, encoding))
                {
                    archive.CreateEntriesFromDirectory(sourceDir, archiveDirectory, safeMode, null);
                }
                return stream.ToArray();
            }
        }

        #endregion

        #region Распаковка

        /// <summary>
        /// Распаковать содержимое архива в указанную папку
        /// </summary>
        /// <param name="zipFileName">Путь к файлу архиву</param>
        /// <param name="path">Папка, в которую нужно распаковать архив</param>
        /// <param name="overwrite">Перезаписывать существующие файлы</param>
        /// <returns>Список распакованных файлов</returns>
        public static IEnumerable<string> ExtractZipFile([NotNull] string zipFileName, [NotNull] string path, bool overwrite = false)
        {
            if (zipFileName == null)
                throw new ArgumentNullException(nameof(zipFileName));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var archive = System.IO.Compression.ZipFile.OpenRead(zipFileName))
            {
                return archive.ExtractToDirectory(path, overwrite);
            }
        }

        /// <summary>
        /// Распаковать содержимое архива в указанную папку
        /// </summary>
        /// <param name="zipData">Архив</param>
        /// <param name="path">Папка, в которую нужно распаковать архив</param>
        /// <param name="overwrite">Перезаписывать существующие файлы</param>
        /// <param name="encoding">Кодировка записей в архиве</param> 
        /// <returns>Список распакованных файлов</returns>   
        public static IEnumerable<string> ExtractZipFile([NotNull] byte[] zipData, [NotNull] string path, bool overwrite = false, Encoding encoding = null)
        {
            if (zipData == null)
                throw new ArgumentNullException(nameof(zipData));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var stream = new MemoryStream(zipData))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, false, encoding))
            {
                return archive.ExtractToDirectory(path, overwrite);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Добавить в существующий архив содержимое папки, включая все вложенные папки и их содержимое
        /// </summary>
        /// <param name="zipFileName">Путь к новому или существующему файлу архива</param>
        /// <param name="sourceDir">Папка, содерждимое которой необходимо добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить данные</param>
        /// <param name="overwrite">Перезаписывать существующие в архиве записи при совпадении имен</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <param name="filter">Фильтр файлов, которые нужно включить в архив</param>
        private static void ZipDirExisting([NotNull] string zipFileName, [NotNull] string sourceDir, [CanBeNull] string archiveDirectory, bool overwrite, bool safeMode, [CanBeNull] Func<FileSystemInfo, bool> filter)
        {
            var tmpFile = Path.GetTempFileName();

            try
            {
                using (var source = new ZipArchive(File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read), ZipArchiveMode.Read))
                using (var copy = new ZipArchive(File.Open(tmpFile, FileMode.Create), ZipArchiveMode.Create))
                {
                    var entries = copy.CreateEntriesFromDirectory(sourceDir, archiveDirectory, safeMode, filter);

                    foreach (var entry in source.Entries)
                    {
                        if (!entries.Contains(entry.FullName))
                        {
                            var copyEntry = copy.CreateEntry(entry.FullName);

                            using (var sourceStream = entry.Open())
                            using (var targetStream = copyEntry.Open())
                            {
                                sourceStream.CopyTo(targetStream);
                            }
                        }
                        else if (!overwrite)
                        {
                            throw new ArgumentException($"File '{entry.Name}' already exists in archive");
                        }
                    }
                }

                File.Delete(zipFileName);
                File.Move(tmpFile, zipFileName);
            }
            finally
            {
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }
        }

        /// <summary>
        /// Добавить файл в существующий архив
        /// </summary>
        /// <param name="zipFileName">Путь к новому или существующему файлу архива</param>
        /// <param name="sourceFile">Файл, который нужно добавить в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо поместить данные</param>
        /// <param name="overwrite">Перезаписывать существующие в архиве записи при совпадении имен</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        private static void ZipFileExisting([NotNull] string zipFileName, [NotNull] string sourceFile, [CanBeNull] string archiveDirectory, bool overwrite, bool safeMode)
        {
            var tmpFile = Path.GetTempFileName();

            try
            {
                using (var source = new ZipArchive(File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read), ZipArchiveMode.Read))
                using (var copy = new ZipArchive(File.Open(tmpFile, FileMode.Create), ZipArchiveMode.Create))
                {
                    var entryName = copy.AddFile(sourceFile, archiveDirectory, safeMode);

                    foreach (var entry in source.Entries)
                    {
                        if (!string.Equals(entryName, entry.FullName))
                        {
                            var copyEntry = copy.CreateEntry(entry.FullName);

                            using (var sourceStream = entry.Open())
                            using (var targetStream = copyEntry.Open())
                            {
                                sourceStream.CopyTo(targetStream);
                            }
                        }
                        else if (!overwrite)
                        {
                            throw new ArgumentException($"File '{entry.Name}' already exists in archive");
                        }
                    }
                }

                File.Delete(zipFileName);
                File.Move(tmpFile, zipFileName);
            }
            finally
            {
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }
        }

        /// <summary>
        /// Добавить файлы в существующий архив
        /// </summary>
        /// <param name="zipFileName">Путь к новому или существующему файлу архива</param>
        /// <param name="sourceFiles">Файлы, которые нужно добавить в архив</param>
        /// <param name="archiveDirectory"></param>
        /// <param name="overwrite">Перезаписывать существующие в архиве записи при совпадении имен</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        private static void ZipFilesExisting([NotNull] string zipFileName, [NotNull] IEnumerable<string> sourceFiles, [CanBeNull] string archiveDirectory, bool overwrite, bool safeMode)
        {
            var tmpFile = Path.GetTempFileName();

            try
            {
                using (var source = new ZipArchive(File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read), ZipArchiveMode.Read))
                using (var copy = new ZipArchive(File.Open(tmpFile, FileMode.Create), ZipArchiveMode.Create))
                {
                    var entries = new HashSet<string>();

                    foreach (var sourceFile in sourceFiles)
                    {
                        var entryName = copy.AddFile(sourceFile, archiveDirectory, safeMode);
                        entries.Add(entryName);
                    }

                    foreach (var entry in source.Entries)
                    {
                        if (!entries.Contains(entry.FullName))
                        {
                            var copyEntry = copy.CreateEntry(entry.FullName);

                            using (var sourceStream = entry.Open())
                            using (var targetStream = copyEntry.Open())
                            {
                                sourceStream.CopyTo(targetStream);
                            }
                        }
                        else if (!overwrite)
                        {
                            throw new ArgumentException($"File '{entry.Name}' already exists in archive");
                        }
                    }
                }

                File.Delete(zipFileName);
                File.Move(tmpFile, zipFileName);
            }
            finally
            {
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }
        }

        /// <summary>
        /// Заполняет архив записями на основе содержимого указанной директории
        /// </summary>
        /// <param name="archive">Архив для заполнения</param>
        /// <param name="sourceDirectoryName">Папка для добавления в архив</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую будут сохранены данные</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <param name="filter">Фильтр файлов, которые нужно включить в архив</param>
        /// <returns>Набор имен добавленных записей</returns>
        private static HashSet<string> CreateEntriesFromDirectory([NotNull] this ZipArchive archive,
            [NotNull] string sourceDirectoryName,
            [CanBeNull] string archiveDirectory,
            bool safeMode,
            [CanBeNull] Func<FileSystemInfo, bool> filter)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));
            if (string.IsNullOrEmpty(sourceDirectoryName))
                throw new ArgumentNullException(nameof(sourceDirectoryName));
            if (!Directory.Exists(sourceDirectoryName))
                throw new ArgumentException($"Directory '{sourceDirectoryName}' doesn't exist");

            var createdEntries = new HashSet<string>();
            var directoryInfo = new DirectoryInfo(sourceDirectoryName);
            var rootPath = directoryInfo.FullName;
            var entries = filter == null
                ? directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                : EnumerateFileSystemInfos(directoryInfo, filter);

            foreach (var fileSystemInfo in entries)
            {
                var length = fileSystemInfo.FullName.Length - rootPath.Length;

                var entryName =
                    fileSystemInfo.FullName.Substring(rootPath.Length, length)
                        .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                if (!string.IsNullOrEmpty(archiveDirectory))
                {
                    entryName = Path.Combine(archiveDirectory, entryName);
                }

                if (fileSystemInfo is DirectoryInfo)
                {
                    entryName = $@"{entryName}{Separator}";

                    if (archive.Mode != ZipArchiveMode.Create && archive.GetEntry(entryName) != null)
                    {
                        continue;
                    }

                    archive.CreateEntry(entryName);
                }
                else
                {
                    archive.CreateEntryFromFile(fileSystemInfo.FullName, entryName, safeMode);
                    createdEntries.Add(entryName);
                }
            }

            return createdEntries;
        }

        [NotNull, Pure]
        private static IEnumerable<FileSystemInfo> EnumerateFileSystemInfos([NotNull] DirectoryInfo dir, [NotNull] Func<FileSystemInfo, bool> filter)
        {
            return dir.EnumerateDirectories()
                .Where<DirectoryInfo>(filter)
                .SelectMany(subdir => EnumerateFileSystemInfos(subdir, filter))
                .Concat(dir.EnumerateFiles().Where(filter));
        }

        /// <summary>
        /// Извлекает архив в указанную папку
        /// </summary>
        /// <param name="archive">Архив для распаковки</param>
        /// <param name="directory">Папка, в которую необходимо распаковать архив</param>
        /// <param name="overwrite">Переписывать существующие файлы</param>
        /// <returns>Список добавленных файлов</returns>
        private static IEnumerable<string> ExtractToDirectory([NotNull] this ZipArchive archive, [NotNull] string directory, bool overwrite)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            var fullDirectoryName = Directory.CreateDirectory(directory).FullName;

            var files = new List<string>();

            foreach (var entry in archive.Entries)
            {
                string fileName = entry.FullName;
                // если имя начинается со slash символа, то обрезаем его, это нужно для того, чтобы Path.Combine отработывал нормально
                // если выполнить Path.Combine("C:temp", "file.zip"), получим "C:temp\file.zip"
                // если выполнить Path.Combine("C:temp", "\file.zip"), получим "\file.zip", т.к.данный путь принимается как абсолютный
                if (Path.IsPathRooted(fileName))
                {
                    fileName = fileName.TrimStart(Path.DirectorySeparatorChar);
                    fileName = fileName.TrimStart(Path.AltDirectorySeparatorChar);
                }
                string fullPath = Path.GetFullPath(Path.Combine(fullDirectoryName, fileName));

                if (!fullPath.StartsWith(fullDirectoryName, StringComparison.OrdinalIgnoreCase))
                    continue; // элемент находится вне папки назначения

                if (Path.GetFileName(fullPath).Length == 0)
                {
                    if (entry.Length != 0L)
                        continue; // элемент прикидывается папкой, но содержит информацию

                    Directory.CreateDirectory(fullPath);
                }
                else
                {
                    var entryDirectory = Path.GetDirectoryName(fullPath);

                    if (!string.IsNullOrEmpty(entryDirectory))
                        Directory.CreateDirectory(entryDirectory);

                    entry.ExtractToFile(fullPath, overwrite);

                    files.Add(fullPath);
                }
            }

            return files;
        }


        public static string AddFile([NotNull] this ZipArchive archive, [NotNull] string sourceFile, bool safeMode)
        {
            return AddFile(archive, sourceFile, null, safeMode);
        }

        /// <summary>
        /// Добавить файл в архив
        /// </summary>
        /// <param name="archive">Архив для добавления</param>
        /// <param name="sourceFile">Файл, который необходимо добавить</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо добавить файл</param>
        /// <param name="safeMode">Безопасный режим (копирование файла перед архивацией)</param>
        /// <returns>Имя добавленной записи</returns>
        public static string AddFile([NotNull] this ZipArchive archive, [NotNull] string sourceFile, [CanBeNull] string archiveDirectory, bool safeMode)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (!File.Exists(sourceFile))
                throw new ArgumentException($"File '{sourceFile}' not exists");

            var entryName = Path.GetFileName(sourceFile);

            if (!string.IsNullOrEmpty(archiveDirectory))
            {
                entryName = Path.Combine(archiveDirectory, entryName);
            }

            archive.CreateEntryFromFile(sourceFile, entryName, safeMode);

            return entryName;
        }



        /// <summary>
        /// Добавить файлы в архив
        /// </summary>
        /// <param name="archive">Архив для добавления</param>
        /// <param name="sourceFiles">Файлы, которые необходимо добавить</param>
        /// <param name="archiveDirectory">Папка в архиве, в которую необходимо добавить файл</param>
        /// <param name="safeMode">Безопасный режим (копирование перед архивацией)</param>
        /// <returns>Имя добавленной записи</returns>
        public static HashSet<string> AddFiles([NotNull] this ZipArchive archive, [NotNull] IEnumerable<string> sourceFiles, [CanBeNull] string archiveDirectory = null, bool safeMode = false)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            if (sourceFiles == null)
                throw new ArgumentNullException(nameof(sourceFiles));

            var entries = new HashSet<string>();

            foreach (var sourceFile in sourceFiles)
            {
                if (sourceFile == null)
                    throw new ArgumentException("File name can't be null");

                if (!File.Exists(sourceFile))
                    throw new ArgumentException($"File '{sourceFile}' not exists");

                var entryName = Path.GetFileName(sourceFile);

                if (!string.IsNullOrEmpty(archiveDirectory))
                {
                    entryName = Path.Combine(archiveDirectory, entryName);
                }

                archive.CreateEntryFromFile(sourceFile, entryName, safeMode);
                entries.Add(entryName);
            }

            return entries;
        }



        public static string AddEntry([NotNull] this ZipArchive archive, [NotNull] string entryName, [NotNull] byte[] data, [CanBeNull] string archiveDirectory = null)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            if (entryName == null)
                throw new ArgumentNullException(nameof(entryName));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!string.IsNullOrEmpty(archiveDirectory))
            {
                entryName = Path.Combine(archiveDirectory, entryName);
            }

            var entry = archive.CreateEntry(entryName);

            using (var stream = entry.Open())
            {
                stream.Write(data, 0, data.Length);
            }

            return entryName;
        }

        public static string AddEntry([NotNull] this ZipArchive archive, [NotNull] string entryName, [NotNull] Stream data, [CanBeNull] string archiveDirectory = null)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            if (entryName == null)
                throw new ArgumentNullException(nameof(entryName));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!string.IsNullOrEmpty(archiveDirectory))
            {
                entryName = Path.Combine(archiveDirectory, entryName);
            }

            var entry = archive.CreateEntry(entryName);

            using (var stream = entry.Open())
            {
                data.CopyTo(stream);
            }

            return entryName;
        }



        public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, bool safeMode)
        {
            return CreateEntryFromFile(destination, sourceFileName, entryName, null, safeMode);
        }

        public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionLevel? compressionLevel, bool safeMode)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (sourceFileName == null)
                throw new ArgumentNullException(nameof(sourceFileName));
            if (entryName == null)
                throw new ArgumentNullException(nameof(entryName));

            var fileName = sourceFileName;

            if (safeMode)
            {
                var tempFilename = Path.GetTempFileName();
                File.Copy(sourceFileName, tempFilename, true);
                fileName = tempFilename;
            }

            ZipArchiveEntry zipArchiveEntry;

            using (var stream = (Stream)File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                zipArchiveEntry = compressionLevel.HasValue ? destination.CreateEntry(entryName, compressionLevel.Value) : destination.CreateEntry(entryName);

                var dateTime = File.GetLastWriteTime(fileName);

                if (dateTime.Year < 1980 || dateTime.Year > 2107)
                    dateTime = new DateTime(1980, 1, 1, 0, 0, 0);

                zipArchiveEntry.LastWriteTime = dateTime;

                using (Stream destination1 = zipArchiveEntry.Open())
                {
                    stream.CopyTo(destination1);
                }
            }

            if (safeMode)
            {
                File.Delete(fileName);
            }

            return zipArchiveEntry;
        }

        #endregion

    }
}