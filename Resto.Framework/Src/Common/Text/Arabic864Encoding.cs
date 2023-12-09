using System;
using System.Text;
using System.Collections.Generic;

namespace Resto.Framework.Text
{
    /// <summary>
    /// Arabic864Encoding class. Преобразовать Unicode символы в последовательность байтов в 
    /// арабской кодировке IBM864 (стандартными средствами не работает)
    /// </summary>
    public class Arabic864Encoding : Encoding
    {
        internal static readonly string Name = "Arabic864";
        private static readonly byte QuestionMarkByte = 0x3F;

        #region System.Text.Encoding methods

        public override string EncodingName
        {
            get
            {
                return Name;
            }
        }

        public override int CodePage
        {
            get
            {
                return 864;
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
                throw new ArgumentNullException(nameof(chars));
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
                throw new ArgumentNullException(nameof(chars));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (charIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(charIndex), charIndex, "Invalid value specified");
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount), charCount, "Invalid value specified");
            if (charIndex + charCount > chars.Length)
                throw new ArgumentOutOfRangeException("charIndex + charCount > chars.Length", charIndex + charCount, "Invalid charIndex and charCount specified");

            var bytesCount = GetByteCount(chars, charIndex, charCount);
            if (byteIndex + bytesCount < bytes.Length)
                throw new ArgumentOutOfRangeException("byteIndex + bytesCount > bytes.Length", byteIndex + bytesCount, "Invalid byteIndex and byteCount specified");

            const char leftToRightMark = '\u200E';
            var byteCurIndex = byteIndex;
            var byteStartIndex = byteCurIndex; //индекс "начала" предложения (leftToRightMark)
            var byteEndIndex = -1; //индекс "конца" предложения (leftToRightMark)
            var isRightToLeft = false;
            var prevArabicChar = 0; //предыдущий не "прозрачный" арабский символ или 0
            for (var index = charIndex; index < charIndex + charCount; index++)
            {
                var symbol = chars[index];
                if (symbol == leftToRightMark)
                {
                    byteStartIndex = byteCurIndex + 1;
                    byteEndIndex = -1;
                }
                else if (byteCurIndex == byteStartIndex) // о направленности судим по первому символу
                {
                    isRightToLeft = !IsLatinOrControl(symbol);
                    prevArabicChar = 0;
                    if (isRightToLeft)
                    {
                        //найти "конец" предложения, исключая пробелы
                        var lastIndex = charIndex + charCount;
                        for (var i = index; i < lastIndex && chars[i] != leftToRightMark && chars[i] != '\n'; ++i)
                        {
                            if (chars[i] != ' ')
                                byteEndIndex = i;
                        }
                    }
                }
                var bytePos = isRightToLeft && byteEndIndex >= byteCurIndex && byteEndIndex > (byteCurIndex - byteStartIndex)
                            ? byteEndIndex - byteCurIndex + byteStartIndex
                            : byteCurIndex;
                if (IsLatinOrControl(symbol))
                {   //латинский символ
                    bytes[bytePos] = ConvertLatinCharToByte(symbol);
                    prevArabicChar = 0;
                }
                else if (tableUnicodeFormBToIbm864.ContainsKey(symbol))
                {   //символ из Presentation Form B
                    bytes[bytePos] = tableUnicodeFormBToIbm864[symbol];
                    prevArabicChar = 0;
                }
                else if (tableUnicodeToIbm864.ContainsKey(symbol))
                {   //символ из Presentation Form A - учет позиции
                    //определить следующий не "прозрачный" арабский символ
                    var nextArabicChar = 0; //следущий не "прозрачный" арабский символ или 0
                    for (var i = index + 1; i < charIndex + charCount && (byteEndIndex < byteCurIndex || i < byteEndIndex); ++i)
                    {
                        if (!transparentUniSymbol.Contains(chars[i]))
                        {
                            var nextSymbol = chars[i];
                            if (tableUnicodeToIbm864.ContainsKey(nextSymbol)
                                && (tableUnicodeToIbm864[nextSymbol].Medial != 0 || tableUnicodeToIbm864[nextSymbol].Final != 0))
                                nextArabicChar = nextSymbol;
                            break;
                        }
                    }
                    if (tableUnicodeToIbm864[symbol].Initial == 0 && tableUnicodeToIbm864[symbol].Medial == 0)
                        prevArabicChar = 0;
                    var charRepresentation = tableUnicodeToIbm864[symbol];

                    //комбинированный арабский символ
                    if (arabicComboInitial == symbol && nextArabicChar != 0 && tableUnicodeComboToIbm864.ContainsKey(nextArabicChar))
                    {
                        if (prevArabicChar != 0)
                            bytes[bytePos] = tableUnicodeComboToIbm864[nextArabicChar].Final;
                        else
                            bytes[bytePos] = tableUnicodeComboToIbm864[nextArabicChar].Isolated;
                    }
                    else if (arabicComboInitial == prevArabicChar && tableUnicodeComboToIbm864.ContainsKey(symbol))
                    {
                        bytes[bytePos] = 0; //уже обработан ранее
                    }
                    //определить позицию символа
                    else if (prevArabicChar != 0 && nextArabicChar != 0 && charRepresentation.Medial != 0)
                        bytes[bytePos] = charRepresentation.Medial; //символ в середине
                    else if (prevArabicChar != 0 && charRepresentation.Final != 0)
                        bytes[bytePos] = charRepresentation.Final; //завершающий символ
                    else if (nextArabicChar != 0 && charRepresentation.Initial != 0)
                        bytes[bytePos] = charRepresentation.Initial; //начальный символ
                    else
                        bytes[bytePos] = charRepresentation.Isolated; //отдельностоящий символ

                    if (!transparentUniSymbol.Contains(symbol))
                        prevArabicChar = symbol;
                }
                else if (transparentUniSymbol.Contains(symbol))
                {   //часть арабского символа, если не обработан ранее, считаем невидимым
                    bytes[bytePos] = 0;
                }
                else
                {   //неизвестный символ
                    bytes[bytePos] = QuestionMarkByte;
                    prevArabicChar = 0;
                }

                ++byteCurIndex;
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
                throw new ArgumentNullException(nameof(bytes));
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
                throw new ArgumentNullException(nameof(chars));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (byteIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(byteIndex), byteIndex, "Invalid value specified");
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount), byteCount, "Invalid value specified");
            if (byteIndex + byteCount > chars.Length)
                throw new ArgumentOutOfRangeException("byteIndex + byteCount > chars.Length", byteIndex + byteCount, "Invalid byteIndex and byteCount specified");

            var charCount = GetCharCount(bytes, byteIndex, byteCount);
            if (charIndex + charCount < chars.Length)
                throw new ArgumentOutOfRangeException("charIndex + charCount > chars.Length", charIndex + charCount, "Invalid charIndex and charCount specified");

            const byte zeroMark = 0;
            var charCurIndex = charIndex;
            var charStartIndex = charCurIndex; //индекс "начала" предложения (zeroMark)
            var charEndIndex = -1; //индекс "конца" предложения (zeroMark)
            var isRightToLeft = false;
            for (var index = byteIndex; index < byteIndex + byteCount; index++)
            {
                if (bytes[index] == zeroMark)
                {
                    charStartIndex = charCurIndex + 1;
                    charEndIndex = -1;
                }
                else if (charCurIndex == charStartIndex) // о направленности судим по первому символу
                {
                    isRightToLeft = bytes[index] > 127; // не строгое условие арабского символа
                    if (isRightToLeft)
                    {
                        //найти "конец" предложения, исключая пробелы
                        var lastIndex = charIndex + charCount;
                        for (var i = index; i < lastIndex && bytes[i] != zeroMark && bytes[i] != '\n'; ++i)
                        {
                            if (bytes[i] != ' ')
                                charEndIndex = i;
                        }
                    }
                }
                var charPos = isRightToLeft && charEndIndex >= charCurIndex && charEndIndex > (charCurIndex - charStartIndex)
                            ? charEndIndex - charCurIndex + charStartIndex
                            : charCurIndex;
                chars[charPos] = ConvertByteToChar(bytes[index]);
                ++charCurIndex;
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
            return new Arabic864Decoder(this);
        }
        public override Encoder GetEncoder()
        {
            return new Arabic864Encoder(this);
        }
        #endregion

        internal class Arabic864Encoder : Encoder
        {
            private readonly Encoding encoding = null;
            internal Arabic864Encoder(Arabic864Encoding encoding)
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
        internal class Arabic864Decoder : Decoder
        {
            private readonly Encoding encoding = null;
            internal Arabic864Decoder(Arabic864Encoding encoding)
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
        /// Набор "прозрачных" частей unicode-символов для представления арабского письма
        /// </summary>
        private static readonly HashSet<int> transparentUniSymbol = new HashSet<int> {
            0x0610, /* ARABIC SIGN SALLALLAHOU ALAYHE WASSALLAM */
            0x0612, /* ARABIC SIGN ALAYHE ASSALLAM */
            0x0613, /* ARABIC SIGN RADI ALLAHOU ANHU */
            0x0614, /* ARABIC SIGN TAKHALLUS */
            0x0615, /* ARABIC SMALL HIGH TAH */
            0x064B, /* ARABIC FATHATAN */
            0x064C, /* ARABIC DAMMATAN */
            0x064D, /* ARABIC KASRATAN */
            0x064E, /* ARABIC FATHA */
            0x064F, /* ARABIC DAMMA */
            0x0650, /* ARABIC KASRA */
            0x0651, /* ARABIC SHADDA */
            0x0652, /* ARABIC SUKUN */
            0x0653, /* ARABIC MADDAH ABOVE */
            0x0654, /* ARABIC HAMZA ABOVE */
            0x0655, /* ARABIC HAMZA BELOW */
            0x0656, /* ARABIC SUBSCRIPT ALEF */
            0x0657, /* ARABIC INVERTED DAMMA */
            0x0658, /* ARABIC MARK NOON GHUNNA */
            0x0670, /* ARABIC LETTER SUPERSCRIPT ALEF */
            0x06D6, /* ARABIC SMALL HIGH LIGATURE SAD WITH LAM WITH ALEF MAKSURA */
            0x06D7, /* ARABIC SMALL HIGH LIGATURE QAF WITH LAM WITH ALEF MAKSURA */
            0x06D8, /* ARABIC SMALL HIGH MEEM INITIAL FORM */
            0x06D9, /* ARABIC SMALL HIGH LAM ALEF */
            0x06DA, /* ARABIC SMALL HIGH JEEM */
            0x06DB, /* ARABIC SMALL HIGH THREE DOTS */
            0x06DC, /* ARABIC SMALL HIGH SEEN */
            0x06DF, /* ARABIC SMALL HIGH ROUNDED ZERO */
            0x06E0, /* ARABIC SMALL HIGH UPRIGHT RECTANGULAR ZERO */
            0x06E1, /* ARABIC SMALL HIGH DOTLESS HEAD OF KHAH */
            0x06E2, /* ARABIC SMALL HIGH MEEM ISOLATED FORM */
            0x06E3, /* ARABIC SMALL LOW SEEN */
            0x06E4, /* ARABIC SMALL HIGH MADDA */
            0x06E7, /* ARABIC SMALL HIGH YEH */
            0x06E8, /* ARABIC SMALL HIGH NOON */
            0x06EA, /* ARABIC EMPTY CENTRE LOW STOP */
            0x06EB, /* ARABIC EMPTY CENTRE HIGH STOP */
            0x06EC, /* ARABIC ROUNDED HIGH STOP WITH FILLED CENTRE */
            0x06ED /* ARABIC SMALL LOW MEEM */
        };

        /// <summary>
        /// Представление арабского символа в виде IBM864 байта 
        /// </summary>
        internal struct ArabicCharByte
        {
            public ArabicCharByte(byte isolated, byte initial, byte medial, byte final)
            {
                Isolated = isolated;
                Initial = initial;
                Medial = medial;
                Final = final;
            }

            public byte Isolated; //отдельностоящий символ
            public byte Initial;  //начальный символ
            public byte Medial;   //символ в середине
            public byte Final;    //завершающий символ
        }

        /// <summary>
        /// Таблица соответствия unicode-символа и IBM864 байта (Presentation Form A or Base Form)
        /// </summary>
        private static readonly Dictionary<int, ArabicCharByte> tableUnicodeToIbm864 = new Dictionary<int, ArabicCharByte> {
            {0x0621, new ArabicCharByte(0xC1,   0,   0,  0)},       //0xFE80,0,0,0                     HAMZA
            {0x0622, new ArabicCharByte(0xC2,   0,   0,  0xA2)},    //0xFE81,0,0,0xFE82                ALEF_MADDA
            {0x0623, new ArabicCharByte(0xC3,   0,   0,  0xA5)},    //0xFE83,0,0,0xFE84                ALEF_HAMZA_ABOVE
            {0x0624, new ArabicCharByte(0xC4,   0,   0,  0)},       //0xFE85,0,0,0xFE86                WAW_HAMZA
            //{0x0625, new ArabicCharByte(0,   0,   0,  0)},        //0xFE87,0,0,0xFE88                ALEF_HAMZA_BELOW, не предcтавлен в IBM864
            {0x0626, new ArabicCharByte(0xC6,0xC6,   0,  0)},       //0xFE89, 0xFE8B, 0xFE8C, 0xFE8A   YEH_HAMZA, Isolated должна быть всегда определена
            {0x0627, new ArabicCharByte(0xC7,   0,   0,   0xA8)},    //0xFE8D,0,0,0xFE8E                ALEF 
            {0x0628, new ArabicCharByte(0xA9,0xC8,   0,   0)},       //0xFE8F, 0xFE91, 0xFE92, 0xFE90   BEH 
            {0x0629, new ArabicCharByte(0xC9,   0,   0,   0)},       //0xFE93,0,0,0xFE94                TEH_MARBUTA
            {0x062A, new ArabicCharByte(0xAA,0xCA,   0,   0)},       //0xFE95, 0xFE97, 0xFE98, 0xFE96   TEH
            {0x062B, new ArabicCharByte(0xAB,0xCB,   0,   0)},       //0xFE99, 0xFE9B, 0xFE9C, 0xFE9A   THEH
            {0x062C, new ArabicCharByte(0xAD,0xCC,   0,   0)},       //0xFE9D, 0xFE9F, 0xFEA0, 0xFE9E   JEEM
            {0x062D, new ArabicCharByte(0xAE,0xCD,   0,   0)},       //0xFEA1, 0xFEA3, 0xFEA4, 0xFEA2   HAH
            {0x062E, new ArabicCharByte(0xAF,0xCE,   0,   0)},       //0xFEA5, 0xFEA7, 0xFEA8, 0xFEA6   KHAH
            {0x062F, new ArabicCharByte(0xCF,   0,   0,   0)},       //0xFEA9,0,0,0xFEAA                DAL
            {0x0630, new ArabicCharByte(0xD0,   0,   0,   0)},       //0xFEAB,0,0,0xFEAC                THAL
            {0x0631, new ArabicCharByte(0xD1,   0,   0,   0)},       //0xFEAD,0,0,0xFEAE                REH
            {0x0632, new ArabicCharByte(0xD2,   0,   0,   0)},       //0xFEAF,0,0,0xFEB0                ZAIN
            {0x0633, new ArabicCharByte(0xBC,0xD3,   0,   0)},       //0xFEB1, 0xFEB3, 0xFEB4, 0xFEB2   SEEN
            {0x0634, new ArabicCharByte(0xBD,0xD4,   0,   0)},       //0xFEB5, 0xFEB7, 0xFEB8, 0xFEB6   SHEEN
            {0x0635, new ArabicCharByte(0xBE,0xD5,   0,   0)},       //0xFEB9, 0xFEBB, 0xFEBC, 0xFEBA   SAD
            {0x0636, new ArabicCharByte(0xEB,0xD6,   0,   0)},       //0xFEBD, 0xFEBF, 0xFEC0, 0xFEBE   DAD
            {0x0637, new ArabicCharByte(0xD7,   0,   0,   0)},       //0xFEC1, 0xFEC3, 0xFEC4, 0xFEC2   TAH
            {0x0638, new ArabicCharByte(0xD8,   0,   0,   0)},       //0xFEC5, 0xFEC7, 0xFEC8, 0xFEC6   ZAH
            {0x0639, new ArabicCharByte(0xDF,0xD9,0xEC,0xC5)},       //0xFEC9, 0xFECB, 0xFECC, 0xFECA   AIN
            {0x063A, new ArabicCharByte(0xEE,0xDA,0xF7,0xED)},       //0xFECD, 0xFECF, 0xFED0, 0xFECE   GHAIN
            {0x0640, new ArabicCharByte(0xE0,   0,   0,   0)},       //0x0640, 0, 0, 0                  TATWEEL
            {0x0641, new ArabicCharByte(0xBA,0xE1,   0,   0)},       //0xFED1, 0xFED3, 0xFED4, 0xFED2   FEH
            {0x0642, new ArabicCharByte(0xF8,0xE2,   0,   0)},       //0xFED5, 0xFED7, 0xFED8, 0xFED6   QAF
            {0x0643, new ArabicCharByte(0xFC,0xE3,   0,   0)},       //0xFED9, 0xFEDB, 0xFEDC, 0xFEDA   KAF
            {0x0644, new ArabicCharByte(0xFB,0xE4,   0,   0)},       //0xFEDD, 0xFEDF, 0xFEE0, 0xFEDE   LAM
            {0x0645, new ArabicCharByte(0xEF,0xE5,   0,   0)},       //0xFEE1, 0xFEE3, 0xFEE4, 0xFEE2   MEEM
            {0x0646, new ArabicCharByte(0xF2,0xE6,   0,   0)},       //0xFEE5, 0xFEE7, 0xFEE8, 0xFEE6   NOON
            {0x0647, new ArabicCharByte(0xF3,0xE7,0xF4,   0)},       //0xFEE9, 0xFEEB, 0xFEEC, 0xFEEA   HEH
            {0x0648, new ArabicCharByte(0xE8,   0,   0,   0)},       //0xFEED, 0, 0, 0xFEEE             WAW
            {0x0649, new ArabicCharByte(0xE9,   0,   0,0xF5)},       //0xFEEF, 0, 0, 0xFEF0             ALEF_MAKSURA
            {0x064A, new ArabicCharByte(0xFD,0xEA,   0,0xF6)}        //0xFEF1, 0xFEF3, 0xFEF4, 0xFEF2   YEH
        };

        /// <summary>
        /// Таблица соответствия unicode-символа и IBM864 байта (Presentation Form B)
        /// </summary>
        private static readonly Dictionary<int, byte> tableUnicodeFormBToIbm864 = new Dictionary<int, byte> {
            {0x066A, 0x25},
            {0x00B0, 0x80},
            {0x00B7, 0x81},
            {0x2219, 0x82},
            {0x221A, 0x83},
            {0x2592, 0x84},
            {0x2500, 0x85},
            {0x2502, 0x86},
            {0x253C, 0x87},
            {0x2524, 0x88},
            {0x252C, 0x89},
            {0x251C, 0x8A},
            {0x2534, 0x8B},
            {0x2510, 0x8C},
            {0x250C, 0x8D},
            {0x2514, 0x8E},
            {0x2518, 0x8F},
            {0x03B2, 0x90},
            {0x221E, 0x91},
            {0x03C6, 0x92},
            {0x00B1, 0x93},
            {0x00BD, 0x94},
            {0x00BC, 0x95},
            {0x2248, 0x96},
            {0x00AB, 0x97},
            {0x00BB, 0x98},
            {0xFEF7, 0x99},
            {0xFEF5, 0xF9},
            {0xFEF6, 0xFA},
            {0xFEF8, 0x9A},
            {0xFEFB, 0x9D},
            {0xFEFC, 0x9E},
            {0x00A0, 0xA0},
            {0x00AD, 0xA1},
            {0x00A3, 0xA3},
            {0x00A4, 0xA4},
            {0x20AC, 0xA7},
            {0x060C, 0xAC},
            {0x0660, 0xB0},
            {0x0661, 0xB1},
            {0x0662, 0xB2},
            {0x0663, 0xB3},
            {0x0664, 0xB4},
            {0x0665, 0xB5},
            {0x0666, 0xB6},
            {0x0667, 0xB7},
            {0x0668, 0xB8},
            {0x0669, 0xB9},
            {0x061B, 0xBB},
            {0x061F, 0xBF},
            {0x00A2, 0xC0},
            {0x00A6, 0xDB},
            {0x00AC, 0xDC},
            {0x00F7, 0xDD},
            {0x00D7, 0xDE},
            {0xFE7D, 0xF0},
            {0x25A0, 0xFE},
            {0x0651, 0xF1},

            {0xFE80, 0xC1},
            {0xFE81, 0xC2},
            {0xFE82, 0xA2},
            {0xFE83, 0xC3},
            {0xFE84, 0xA5},
            {0xFE85, 0xC4},
            //{0xFE86, 0}, //не предcтавлен в IBM864
            //{0xFE87, 0}, //не предcтавлен в IBM864
            //{0xFE88, 0}, //не предcтавлен в IBM864
            //{0xFE89, 0}, //не предcтавлен в IBM864
            {0xFE8B, 0xC6},
            //{0xFE8C, 0}, //не предcтавлен в IBM864
            //{0xFE8A, 0}, //не предcтавлен в IBM864
            {0xFE8D, 0xC7},
            {0xFE8E, 0xA8},
            {0xFE8F, 0xA9},
            {0xFE91, 0xC8},
            //{0xFE90, 0}, //не предcтавлен в IBM864
            //{0xFE92, 0}, //не предcтавлен в IBM864
            {0xFE93, 0xC9},
            //{0xFE94, 0}, //не предcтавлен в IBM864
            {0xFE95, 0xAA},
            {0xFE97, 0xCA},
            //{0xFE98, 0}, //не предcтавлен в IBM864
            //{0xFE96, 0}, //не предcтавлен в IBM864
            {0xFE99, 0xAB},
            {0xFE9B, 0xCB},
            //{0xFE9A, 0}, //не предcтавлен в IBM864
            //{0xFE9C, 0}, //не предcтавлен в IBM864
            {0xFE9D, 0xAD},
            {0xFE9F, 0xCC},
            //{0xFEA0, 0}, //не предcтавлен в IBM864
            //{0xFE9E, 0}, //не предcтавлен в IBM864
            {0xFEA1, 0xAE},
            {0xFEA3, 0xCD},
            //{0xFEA2, 0}, //не предcтавлен в IBM864
            //{0xFEA4, 0}, //не предcтавлен в IBM864
            {0xFEA5, 0xAF},
            {0xFEA7, 0xCE},
            //{0xFEA6, 0}, //не предcтавлен в IBM864
            //{0xFEA8, 0}, //не предcтавлен в IBM864
            {0xFEA9, 0xCF},
            //{0xFEAA, 0}, //не предcтавлен в IBM864
            {0xFEAB, 0xD0},
            //{0xFEAC, 0}, //не предcтавлен в IBM864
            {0xFEAD, 0xD1},
            //{0xFEAE, 0}, //не предcтавлен в IBM864
            {0xFEAF, 0xD2},
            //{0xFEB0, 0}, //не предcтавлен в IBM864
            {0xFEB1, 0xBC},
            {0xFEB3, 0xD3},
            //{0xFEB2, 0}, //не предcтавлен в IBM864
            //{0xFEB4, 0}, //не предcтавлен в IBM864
            {0xFEB5, 0xBD},
            {0xFEB7, 0xD4},
            //{0xFEB6, 0}, //не предcтавлен в IBM864
            //{0xFEB8, 0}, //не предcтавлен в IBM864
            {0xFEB9, 0xBE},
            {0xFEBB, 0xD5},
            //{0xFEBC, 0}, //не предcтавлен в IBM864
            //{0xFEBA, 0}, //не предcтавлен в IBM864
            {0xFEBD, 0xEB},
            {0xFEBF, 0xD6},
            //{0xFEC0, 0}, //не предcтавлен в IBM864
            //{0xFEBE, 0}, //не предcтавлен в IBM864
            {0xFEC1, 0xD7},
            //{0xFEC2, 0}, //не предcтавлен в IBM864
            //{0xFEC3, 0}, //не предcтавлен в IBM864
            //{0xFEC4, 0}, //не предcтавлен в IBM864
            {0xFEC5, 0xD8},
            //{0xFEC6, 0}, //не предcтавлен в IBM864
            //{0xFEC7, 0}, //не предcтавлен в IBM864
            //{0xFEC8, 0}, //не предcтавлен в IBM864
            {0xFEC9, 0xDF},
            {0xFECB, 0xD9},
            {0xFECC, 0xEC},
            {0xFECA, 0xC5},
            {0xFECD, 0xEE},
            {0xFECF, 0xDA},
            {0xFED0, 0xF7},
            {0xFECE, 0xED},
            {0xFED1, 0xBA},
            {0xFED3, 0xE1},
            //{0xFED4, 0}, //не предcтавлен в IBM864
            //{0xFED2, 0}, //не предcтавлен в IBM864
            {0xFED5, 0xF8},
            {0xFED7, 0xE2},
            //{0xFED8, 0}, //не предcтавлен в IBM864
            //{0xFED6, 0}, //не предcтавлен в IBM864
            {0xFED9, 0xFC},
            {0xFEDB, 0xE3},
            //{0xFEDC, 0}, //не предcтавлен в IBM864
            //{0xFEDA, 0}, //не предcтавлен в IBM864
            {0xFEDD, 0xFB},
            {0xFEDF, 0xE4},
            //{0xFEE0, 0}, //не предcтавлен в IBM864
            //{0xFEDE, 0}, //не предcтавлен в IBM864
            {0xFEE1, 0xEF},
            {0xFEE3, 0xE5},
            //{0xFEE4, 0}, //не предcтавлен в IBM864
            //{0xFEE2, 0}, //не предcтавлен в IBM864
            {0xFEE5, 0xF2},
            {0xFEE7, 0xE6},
            //{0xFEE8, 0}, //не предcтавлен в IBM864
            //{0xFEE6, 0}, //не предcтавлен в IBM864
            {0xFEE9, 0xF3},
            {0xFEEB, 0xE7},
            {0xFEEC, 0xF4},
            //{0xFEEA, 0}, //не предcтавлен в IBM864
            {0xFEED, 0xE8},
            //{0xFEEE, 0}, //не предcтавлен в IBM864
            {0xFEEF, 0xE9},
            {0xFEF0, 0xF5},
            {0xFEF1, 0xFD},
            {0xFEF3, 0xEA},
            //{0xFEF4, 0}, //не предcтавлен в IBM864
            {0xFEF2, 0xF6}
        };

        private static readonly int arabicComboInitial = 0x0644;
        /// <summary>
        /// Таблица соответствия кобминированного unicode-символа (начинающегося с arabicComboInitial и IBM864 байта (Presentation Form A or Base Form)
        /// </summary>
        private static readonly Dictionary<int, ArabicCharByte> tableUnicodeComboToIbm864 = new Dictionary<int, ArabicCharByte> {
            {0x0622, new ArabicCharByte(0xF9,   0,   0,  0xFA)}, //FEF5,0,0,FEF6    LAM_ALEF_MADDA
            {0x0623, new ArabicCharByte(0x99,   0,   0,  0x9A)}, //FEF7,0,0,FEF8    LAM_ALEF_HAMZA_ABOVE
            {0x0627, new ArabicCharByte(0x9D,   0,   0,  0x9E)}  //FEFB,0,0,FEFC    LAM_ALEF
            //0x0625 -> FEF9, FEFA не предcтавлен в IBM864  LAM_ALEF_HAMZA_BELOW
        };

        /// <summary>
        /// Является ли unicode-символ латинским или управляющим символом 
        /// </summary>
        /// <param name="symbol">unicode-символ</param>
        /// <returns>true or false </returns>
        private static bool IsLatinOrControl(int symbol)
        {
            // Если ASCII подмножество то без изменений 
            if (symbol <= 127)
                return true;
            // invisible markers
            if ((symbol >= 0x202A && symbol <= 0x202E) || symbol == 0x200E || symbol == 0x200F)
                return true;
            return false;
        }

        /// <summary>
        /// Преобразовать unicode-символ в ASCII байт (латиница)
        /// </summary>
        /// <param name="symbol">unicode-символ</param>
        /// <returns>ASCII байт </returns>
        private static byte ConvertLatinCharToByte(int symbol)
        {
            // Если ASCII подмножество то без изменений 
            if (symbol <= 127)
                return (byte)symbol;
            // invisible markers
            if ((symbol >= 0x202A && symbol <= 0x202E) || symbol == 0x200E || symbol == 0x200F)
                return 0;

            return QuestionMarkByte;
        }

        /// <summary>
        /// Таблица соответствия IBM864 байта и unicode-символа
        /// </summary>
        private static readonly Dictionary<byte, int> tableAscii2Uni = new Dictionary<byte, int> {
            {0x80, 0x00B0},
            {0x81, 0x00B7},
            {0x82, 0x2219},
            {0x83, 0x221A},
            {0x84, 0x2592},
            {0x85, 0x2500},
            {0x86, 0x2502},
            {0x87, 0x253C},
            {0x88, 0x2524},
            {0x89, 0x252C},
            {0x8A, 0x251C},
            {0x8B, 0x2534},
            {0x8C, 0x2510},
            {0x8D, 0x250C},
            {0x8E, 0x2514},
            {0x8F, 0x2518},
            {0x90, 0x03B2},
            {0x91, 0x221E},
            {0x92, 0x03C6},
            {0x93, 0x00B1},
            {0x94, 0x00BD},
            {0x95, 0x00BC},
            {0x96, 0x2248},
            {0x97, 0x00AB},
            {0x98, 0x00BB},
            {0x99, 0xFEF7},
            {0x9A, 0xFEF8},
            {0x9D, 0xFEFB},
            {0x9E, 0xFEFC},
            {0xA0, 0x00A0},
            {0xA1, 0x00AD},
            {0xA2, 0xFE82},
            {0xA3, 0x00A3},
            {0xA4, 0x00A4},
            {0xA5, 0xFE84},
            {0xA7, 0x20AC},
            {0xA8, 0xFE8E},
            {0xA9, 0xFE8F},
            {0xAA, 0xFE95},
            {0xAB, 0xFE99},
            {0xAC, 0x060C},
            {0xAD, 0xFE9D},
            {0xAE, 0xFEA1},
            {0xAF, 0xFEA5},
            {0xB0, 0x0660},
            {0xB1, 0x0661},
            {0xB2, 0x0662},
            {0xB3, 0x0663},
            {0xB4, 0x0664},
            {0xB5, 0x0665},
            {0xB6, 0x0666},
            {0xB7, 0x0667},
            {0xB8, 0x0668},
            {0xB9, 0x0669},
            {0xBA, 0xFED1},
            {0xBB, 0x061B},
            {0xBC, 0xFEB1},
            {0xBD, 0xFEB5},
            {0xBE, 0xFEB9},
            {0xBF, 0x061F},
            {0xC0, 0x00A2},
            {0xC1, 0xFE80},
            {0xC2, 0xFE81},
            {0xC3, 0xFE83},
            {0xC4, 0xFE85},
            {0xC5, 0xFECA},
            {0xC6, 0xFE8B},
            {0xC7, 0xFE8D},
            {0xC8, 0xFE91},
            {0xC9, 0xFE93},
            {0xCA, 0xFE97},
            {0xCB, 0xFE9B},
            {0xCC, 0xFE9F},
            {0xCD, 0xFEA3},
            {0xCE, 0xFEA7},
            {0xCF, 0xFEA9},
            {0xD0, 0xFEAB},
            {0xD1, 0xFEAD},
            {0xD2, 0xFEAF},
            {0xD3, 0xFEB3},
            {0xD4, 0xFEB7},
            {0xD5, 0xFEBB},
            {0xD6, 0xFEBF},
            {0xD7, 0xFEC1},
            {0xD8, 0xFEC5},
            {0xD9, 0xFECB},
            {0xDA, 0xFECF},
            {0xDB, 0x00A6},
            {0xDC, 0x00AC},
            {0xDD, 0x00F7},
            {0xDE, 0x00D7},
            {0xDF, 0xFEC9},
            {0xE0, 0x0640},
            {0xE1, 0xFED3},
            {0xE2, 0xFED7},
            {0xE3, 0xFEDB},
            {0xE4, 0xFEDF},
            {0xE5, 0xFEE3},
            {0xE6, 0xFEE7},
            {0xE7, 0xFEEB},
            {0xE8, 0xFEED},
            {0xE9, 0xFEEF},
            {0xEA, 0xFEF3},
            {0xEB, 0xFEBD},
            {0xEC, 0xFECC},
            {0xED, 0xFECE},
            {0xEE, 0xFECD},
            {0xEF, 0xFEE1},
            {0xF0, 0xFE7D},
            {0xF1, 0x0651},
            {0xF2, 0xFEE5},
            {0xF3, 0xFEE9},
            {0xF4, 0xFEEC},
            {0xF5, 0xFEF0},
            {0xF6, 0xFEF2},
            {0xF7, 0xFED0},
            {0xF8, 0xFED5},
            {0xF9, 0xFEF5},
            {0xFA, 0xFEF6},
            {0xFB, 0xFEDD},
            {0xFC, 0xFED9},
            {0xFD, 0xFEF1},
            {0xFE, 0x25A0}
        };

        /// <summary>
        /// Преобразовать IBM864 байт в unicode-символ 
        /// </summary>
        /// <param name="symbol">IBM864 байт</param>
        /// <returns>unicode-символ</returns>
        private static char ConvertByteToChar(byte symbol)
        {
            int result = QuestionMarkByte;

            // Если ASCII подмножество то без изменений 
            if (symbol <= 127)
                result = (char)symbol;
            else if (symbol == 0)
                result = 0x200E;
            else if (tableAscii2Uni.ContainsKey(symbol))
                result = tableAscii2Uni[symbol];

            return (char)result;
        }
    }
}
