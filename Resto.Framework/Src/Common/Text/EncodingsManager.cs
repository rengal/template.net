using System;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Framework.Text
{
    /// <summary>
    /// Вспомогательный класс для работы с различными кодировками, включая нестандартные (такие как армянская или китайская).
    /// </summary>
    public static class EncodingsManager
    {
        #region Fields
        private const string LeftToRightMark = "\u200E";
        private const byte QuestionMark = 0x3F;

        private static readonly ThreadSafeCache<string, Encoding> Encodings = new ThreadSafeCache<string, Encoding>(GetEncoding); 

        /// <summary>
        /// Коллекция признаков поддержки кодировкой сложных языков (чтение справа налево).
        /// </summary>
        private static readonly ThreadSafeCache<string, bool> DoesEncodingSupportRightToLeft = new ThreadSafeCache<string, bool>(CalculateRightToLeftSupport);
        #endregion

        #region Private Methods
        [CanBeNull]
        private static Encoding GetEncoding([NotNull] string codePageName)
        {
            if (String.IsNullOrEmpty(codePageName))
                throw new ArgumentException("Invalid code page name specified", nameof(codePageName));

            try
            {
                int codePage;
                return int.TryParse(codePageName, out codePage)
                    ? Encoding.GetEncoding(codePage)
                    : Encoding.GetEncoding(codePageName);
            }
            catch (ArgumentException)
            {
                return GetNonStandardEncoding(codePageName);
            }
            catch (NotSupportedException)
            {
                return GetNonStandardEncoding(codePageName);
            }
        }

        [CanBeNull]
        private static Encoding GetNonStandardEncoding([NotNull] string codePageName)
        {
            if (String.Equals(codePageName, ArmSCII8Encoding.Name, StringComparison.OrdinalIgnoreCase))
                return new ArmSCII8Encoding();
            if (String.Equals(codePageName, ArmSCII8aEncoding.Name, StringComparison.OrdinalIgnoreCase))
                return new ArmSCII8aEncoding();
            if (String.Equals(codePageName, VietnamEncoding.Name, StringComparison.OrdinalIgnoreCase))
                return new VietnamEncoding();
            if (String.Equals(codePageName, VisciiEncoding.Name, StringComparison.OrdinalIgnoreCase))
                return new VisciiEncoding();
            if (String.Equals(codePageName, Arabic864Encoding.Name, StringComparison.OrdinalIgnoreCase))
                return new Arabic864Encoding();
            // другие нестандартные кодировки добавлять тут
            return null;
        }

        private static bool CalculateRightToLeftSupport(string codePageName)
        {
            var encoding = Encodings[codePageName];
            if (encoding == null)
                return false;

            return encoding.GetBytes(LeftToRightMark + "abc")[0] != QuestionMark;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Поддерживается ли кодировкой чтение справа налево.
        /// </summary>
        /// <param name="codePageName">Название кодировки (см. соответсвующую документацию по <see cref="Encoding.GetEncoding(string)"/>).</param>
        /// <returns><c>true</c>, если поддерживается, <c>false</c>, если не поддерживается, или кодировка не найдена.</returns>
        public static bool IsRightToLeftSupported(string codePageName)
        {
            return !String.IsNullOrEmpty(codePageName) && DoesEncodingSupportRightToLeft[codePageName];
        }

        /// <summary>
        /// Возвращает кодировку по текстовому имени или, если кодировка не найдена, кодировку по умолчанию.
        /// </summary>
        /// <param name="codePageName">Название кодировки (см. соответсвующую документацию по <see cref="Encoding.GetEncoding(string)"/>).</param>
        /// <param name="defaultEncoding">Кодировка по умолчанию.</param>
        [NotNull]
        public static Encoding GetEncoding(string codePageName, [NotNull] Encoding defaultEncoding)
        {
            if (defaultEncoding == null)
                throw new ArgumentNullException(nameof(defaultEncoding));

            if (String.IsNullOrEmpty(codePageName))
                return defaultEncoding;
            return Encodings[codePageName] ?? defaultEncoding;
        }
        #endregion
    }
}
