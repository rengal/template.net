using System.Collections.Generic;
using System.IO;
using log4net;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Вспомогательный класс для работы с файлами
    /// </summary>
    public static class FileHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileHelper));

        private const char DefaultReplaceChar = ' ';

        /// <summary>
        /// Заменить все недопустимые символы в имени файла
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        /// <param name="replaceChar">Символ, который будет использоваться при замене</param>
        /// <returns>Результирующее имя файла</returns>
        public static string NormalizeFileName(string fileName, char replaceChar = DefaultReplaceChar)
        {
            return NormalizeFileName(fileName, replaceChar, null);
        }

        /// <summary>
        /// Заменить все недопустимые символы в имени файла
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        /// <param name="replaceChar">Символ, который будет использоваться при замене</param>
        /// <param name="additionalInvalidChars">Дополнительные символы, использование которых в имени файлов запрещено</param>
        /// <returns>Результирующее имя файла</returns>
        public static string NormalizeFileName(string fileName, char replaceChar, char[] additionalInvalidChars)
        {
            return NormalizeFileName(fileName, replaceChar.ToString(), additionalInvalidChars);
        }

        /// <summary>
        /// Заменить все недопустимые символы в имени файла
        /// </summary>
        /// <param name="fileName">Исходное имя файла</param>
        /// <param name="replaceString">Символ, который будет использоваться при замене</param>
        /// <param name="additionalInvalidChars">Дополнительные символы, использование которых в имени файлов запрещено</param>
        /// <returns>Результирующее имя файла</returns>
        /// <remarks>Хоть <paramref name="replaceString"/> и является строкой, здесь подразумевается передача одного символа.
        /// У <see cref="char"/> нет аналога <see cref="string.Empty"/>, поэтому используется строковый параметр.</remarks>
        public static string NormalizeFileName(string fileName, string replaceString, char[] additionalInvalidChars = null)
        {
            var invalidChars = new List<char>(Path.GetInvalidFileNameChars());
            if (additionalInvalidChars != null)
            {
                invalidChars.AddRange(additionalInvalidChars);
            }

            var badCharFound = false;
            foreach (var badChar in invalidChars)
            {
                var ind = fileName.IndexOf(badChar);
                if (ind >= 0)
                {
                    if (!badCharFound)
                    {
                        Log.Warn($"File name {fileName} contains invalid characters.");
                    }

                    Log.Warn($"Character code: {(int)badChar}. Character: '{badChar}'. Replacing with '{replaceString}'.");

                    fileName = fileName.Replace(badChar.ToString(), replaceString);
                    badCharFound = true;
                }
            }

            return fileName;
        }
    }
}
