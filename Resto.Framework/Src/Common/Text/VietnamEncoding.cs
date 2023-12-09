using System;
using System.Text;

namespace Resto.Framework.Text
{
    class VietnamEncoding : Encoding
    {
        internal static readonly string Name = "Vietnam";

        #region System.Text.Encoding methods

        public override string EncodingName
        {
            get
            {
                return Name;
            }
        }

        public override bool IsSingleByte
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Вернуть количество байт необходимых для кодирования последовательности символов
        /// </summary>
        /// <param name="chars">Массив символов для кодирования.</param>
        /// <param name="index">Стартовый индекс в массиве символов с коротого начинать кодирование.</param>
        /// <param name="count">Количество символов для кодирования.</param>
        /// <returns>Число байт требуемое для кодирования указанной последовательности символов.</returns>
        /// <exception cref="ArgumentNullException">Если массив символов нулевая ссылка</exception>
        /// <exception cref="ArgumentOutOfRangeException">Если index или count меньше нуля,
        /// или index и count не описывают допустимый диапазон символов
        /// </exception>
        public override int GetByteCount(char[] chars, int index, int count)
        {
            if (chars == null)
                throw new ArgumentNullException("Null arrays specified");
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Invalid value specified");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Invalid value specified");
            if (index + count > chars.Length)
                throw new ArgumentOutOfRangeException("index + count > chars.Length", index + count, "Invalid index and count specified");
            return count;
        }

        /// <summary>
        /// Преобразовать последовательность массив Unicode символов в байтовый массив 
        /// </summary>
        /// <param name="chars">Символьный массив.</param>
        /// <param name="charIndex">Начальный индекс символьного массива.</param>
        /// <param name="charCount">Количество символов для кодировки.</param>
        /// <param name="bytes">Байтовый массив принимающий результат</param>
        /// <param name="byteIndex">Стартовый индекс байтового массива с которого будет храниться результат кодировки.</param>
        /// <returns>Число байтов записанных в байтовый массив.</returns>
        /// <exception cref="ArgumentException">
        /// Байтовый массив содержит недостаточное количество памяти для хранения результата
        /// </exception>
        /// <exception cref="ArgumentNullException">В качестве массива указан null</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// charIndex, charCount или byteIndex меньше нуля, или charIndex и charCount не определяют
        /// допустимую последовательность
        /// </exception>
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            if (chars == null)
                throw new ArgumentNullException("Null char arrays specified");
            if (bytes == null)
                throw new ArgumentNullException("Null bytes arrays specified");
            if (charIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(charIndex), charIndex, "Invalid value specified");
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount), charCount, "Invalid value specified");
            if (charIndex + charCount > chars.Length)
                throw new ArgumentOutOfRangeException("charIndex + charCount > chars.Length", charIndex + charCount, "Invalid charIndex and charCount specified");

            var bytesCount = GetByteCount(chars, charIndex, charCount);
            if (byteIndex + bytesCount < bytes.Length)
                throw new ArgumentOutOfRangeException("byteIndex + bytesCount > bytes.Length", byteIndex + bytesCount, "Invalid byteIndex and byteCount specified");

            var byteCurIndex = byteIndex;
            for (var index = charIndex; index < charIndex + charCount; index++)
            {
                bytes[byteCurIndex++] = ConvertCharToByte(chars[index]);
            }
            return bytesCount;
        }

        /// <summary>
        /// Вернуть максимальное число байт необходимое для конвертации указанного числа символов
        /// </summary>
        /// <param name="charCount">Число символов для конвертации.</param>
        /// <returns>Максимальное число байт необходимое для конвертации.</returns>
        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        /// <summary>
        /// Вернуть число символов которые получаться после декодирования байтового массива
        /// </summary>
        /// <param name="bytes">Байтовый массив для декодирования.</param>
        /// <param name="index">Начальный индекс массива.</param>
        /// <param name="count">Число байт для декодирования.</param>
        /// <returns>Число символов получившихся после декодирования.</returns>
        /// <exception cref="ArgumentNullException">Если bytes - нулевая ссылка</exception>
        /// <exception cref="ArgumentOutOfRangeException">index или count мееньше нуля,
        /// или index и count не определяют допустимую последовательность.
        /// </exception>
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            if (bytes == null)
                throw new ArgumentNullException("Null arrays specified");
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Invalid value specified");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Invalid value specified");
            if (index + count > bytes.Length)
                throw new ArgumentOutOfRangeException("index + count > chars.Length", index + count, "Invalid index and count specified");
            return count;
        }

        /// <summary>
        /// Преобразовать последовательность байт в массив символов
        /// </summary>
        /// <param name="bytes">Байтовый массив для декодирования.</param>
        /// <param name="byteIndex">Начальный индекс байтового массива.</param>
        /// <param name="byteCount">Число байт для декодирования.</param>
        /// <param name="chars">Массив символов для хранения результатов декодирования.</param>
        /// <param name="charIndex">Начальный индекс символьного массива.</param>
        /// <returns>Число символов записанных в символьный массив.</returns>
        /// <exception cref="ArgumentException">
        /// chars не содержит достаточного количества памяти для хранения результата
        /// </exception>
        /// <exception cref="ArgumentNullException">Либо chars либо bytes - нулевая ссылка</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// byteIndex, byteCount или charIndex меньше нуля, или byteIndex и byteCount не определяют валидную последовательность.
        /// </exception>
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            if (chars == null)
                throw new ArgumentNullException("Null char arrays specified");
            if (bytes == null)
                throw new ArgumentNullException("Null bytes arrays specified");
            if (byteIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(byteIndex), byteIndex, "Invalid value specified");
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount), byteCount, "Invalid value specified");
            if (byteIndex + byteCount > chars.Length)
                throw new ArgumentOutOfRangeException("byteIndex + byteCount > chars.Length", byteIndex + byteCount, "Invalid byteIndex and byteCount specified");

            var charCount = GetCharCount(bytes, byteIndex, byteCount);
            if (charIndex + charCount < chars.Length)
                throw new ArgumentOutOfRangeException("charIndex + charCount > chars.Length", charIndex + charCount, "Invalid charIndex and charCount specified");

            var charCurIndex = charIndex;
            for (var index = byteIndex; index < byteIndex + byteCount; index++)
            {
                chars[charCurIndex++] = ConvertByteToChar(bytes[index]);
            }
            return charCount;
        }

        /// <summary>
        /// Вернуть максимальное число символов необходимое для конвертации указанного числа байт
        /// </summary>
        /// <param name="byteCount">Число байт для конвертации.</param>
        /// <returns>Максимальное число симолов необходимое для конвертации.</returns>
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
        public override Decoder GetDecoder()
        {
            return new VietnamDecoder(this);
        }
        public override Encoder GetEncoder()
        {
            return new VietnamEncoder(this);
        }
        #endregion

        internal class VietnamEncoder : Encoder
        {
            private readonly Encoding encoding;
            internal VietnamEncoder(Encoding encoding)
            {
                this.encoding = encoding;
            }
            public override int GetByteCount(char[] chars, int index, int count, bool flush)
            {
                return encoding.GetByteCount(chars, index, count);
            }
            public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
            {
                return encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
            }
        }
        internal class VietnamDecoder : Decoder
        {
            private readonly Encoding encoding;
            internal VietnamDecoder(Encoding encoding)
            {
                this.encoding = encoding;
            }
            public override int GetCharCount(byte[] chars, int index, int count)
            {
                return encoding.GetCharCount(chars, index, count);
            }
            public override int GetChars(byte[] chars, int charIndex, int charCount, char[] bytes, int byteIndex)
            {
                return encoding.GetChars(chars, charIndex, charCount, bytes, byteIndex);
            }
        }

        /// <summary>
        /// Преобразовать unicode-символ в armScii-8 байт 
        /// </summary>
        /// <param name="symbol">unicode-символ</param>
        /// <returns>armScii-8 байт </returns>
        private static byte ConvertCharToByte(int symbol)
        {
            return (byte)Decode((char)symbol);
        }

        /// <summary>
        /// Преобразовать unicode-символ в armScii-8 байт 
        /// </summary>
        /// <param name="symbol">unicode-символ</param>
        /// <returns>armScii-8 байт </returns>
        private static char ConvertByteToChar(byte symbol)
        {
            return Decode((char)symbol);
        }


        private static readonly char[] uniChars =
        {
            '\x0102', '\x00C2', '\x0110', '\x00CA', '\x00D4', '\x01A0', '\x01AF', '\x0103', '\x00E2', '\x0111',
            '\x00EA', '\x00F4', '\x01A1', '\x01B0', '\x00E0', '\x00E1', '\x1EA1', '\x1EA3', '\x00E3', '\x00C0',
            '\x00C1', '\x1EA0', '\x1EA2', '\x00C3', '\x1EB1', '\x1EAF', '\x1EB7', '\x1EB3', '\x1EB5', '\x1EB0',
            '\x1EAE', '\x1EB6', '\x1EB2', '\x1EB4', '\x1EA7', '\x1EA5', '\x1EAD', '\x1EA9', '\x1EAB', '\x1EA6',
            '\x1EA4', '\x1EAC', '\x1EA8', '\x1EAA', '\x00E8', '\x00E9', '\x1EB9', '\x1EBB', '\x1EBD', '\x00C8',
            '\x00C9', '\x1EB8', '\x1EBA', '\x1EBC', '\x1EC1', '\x1EBF', '\x1EC7', '\x1EC3', '\x1EC5', '\x1EC0',
            '\x1EBE', '\x1EC6', '\x1EC2', '\x1EC4', '\x00EC', '\x00ED', '\x1ECB', '\x1EC9', '\x0129', '\x00CC',
            '\x00CD', '\x1ECA', '\x1EC8', '\x0128', '\x00F2', '\x00F3', '\x1ECD', '\x1ECF', '\x00F5', '\x00D2',
            '\x00D3', '\x1ECC', '\x1ECE', '\x00D5', '\x1ED3', '\x1ED1', '\x1ED9', '\x1ED5', '\x1ED7', '\x1ED2',
            '\x1ED0', '\x1ED8', '\x1ED4', '\x1ED6', '\x1EDD', '\x1EDB', '\x1EE3', '\x1EDF', '\x1EE1', '\x1EDC',
            '\x1EDA', '\x1EE2', '\x1EDE', '\x1EE0', '\x00F9', '\x00FA', '\x1EE5', '\x1EE7', '\x0169', '\x00D9',
            '\x00DA', '\x1EE4', '\x1EE6', '\x0168', '\x1EEB', '\x1EE9', '\x1EF1', '\x1EED', '\x1EEF', '\x1EEA',
            '\x1EE8', '\x1EF0', '\x1EEC', '\x1EEE', '\x1EF3', '\x00FD', '\x1EF5', '\x1EF7', '\x1EF9', '\x1EF2',
            '\x00DD', '\x1EF4', '\x1EF6', '\x1EF8'
        };

        private static readonly char[] ansiChars =
        {
            '\x88', '\xC2', '\xF1', '\xCA', '\xD4', '\xF7', '\xD0', '\xE6', '\xE2', '\xC7',
            '\xEA', '\xF4', '\xD6', '\xDC', '\xE0', '\xE1', '\xE5', '\xE4', '\xE3', '\x80',
            '\xC1', '\x5B', '\x81', '\x82', '\xA2', '\xA1', '\xA5', '\xA3', '\xA4', '\x8E',
            '\x8D', '\x5D', '\x8F', '\xF0', '\xC0', '\xC3', '\xC6', '\xC4', '\xC5', '\x84',
            '\x83', '\x5C', '\x85', '\x92', '\xE8', '\xE9', '\xCB', '\xC8', '\xEB', '\xD7',
            '\xC9', '\x7B', '\xDE', '\xFE', '\x8A', '\x89', '\x8C', '\x8B', '\xCD', '\x93',
            '\x90', '\x7D', '\x94', '\x95', '\xEC', '\xED', '\xCE', '\xCC', '\xEF', '\xB5',
            '\xB4', '\xDF', '\xB7', '\xB8', '\xF2', '\xF3', '\x86', '\xD5', '\xF5', '\xBC',
            '\xB9', '\xE7', '\xBD', '\xBE', '\xD2', '\xD3', '\xB6', '\xB0', '\x87', '\x97',
            '\x96', '\xEE', '\x98', '\xA6', '\xA9', '\xA7', '\xAE', '\xAA', '\xAB', '\x9E',
            '\x9D', '\xF6', '\x9F', '\x99', '\xF9', '\xFA', '\xF8', '\xFB', '\xDB', '\xA8',
            '\xDA', '\xFC', '\xD1', '\xAC', '\xD8', '\xD9', '\xBF', '\xBA', '\xBB', '\xAF',
            '\xAD', '\x7E', '\xB1', '\xA0', '\xFF', '\x9A', '\x9C', '\x9B', '\xCF', '\xB2',
            '\xDD', '\x91', '\xFD', '\xB3'
        };

        private static char Decode(char utfChar)
        {
            var index = GetCharIndex(utfChar);
            return (index >= 0) ? ansiChars[index] : GetSimpleAscii(utfChar);
        }

        private static int GetCharIndex(char utfChar)
        {
            for (var index = 0; index < uniChars.Length; ++index)
                if (uniChars[index] == utfChar)
                    return index;
            return -1;
        }

        private static char GetSimpleAscii(char utfChar)
        {
            // <ip> Проще пока не придумала. Будут идеи, исправьте пожалуйста.
            return ASCII.GetChars(UTF8.GetBytes(new[] { utfChar }))[0];
        }

    }
}
