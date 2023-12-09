using System;
using System.Text;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Text
{
    public abstract class ArmenianEncoding : Encoding
    {
        private readonly Encoding defaultEncoding = GetEncoding(437);

        #region Inner Types
        private sealed class ArmenianEncoder : Encoder
        {
            private readonly ArmenianEncoding encoding;

            internal ArmenianEncoder(ArmenianEncoding encoding)
            {
                this.encoding = encoding;
            }

            public override int GetByteCount(char[] chars, int index, int count, bool flush)
            {
                return encoding.GetByteCount(chars, index, count);
            }

            public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex,
                                         bool flush)
            {
                return encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
            }
        }

        private sealed class ArmenianDecoder : Decoder
        {
            private readonly ArmenianEncoding encoding;

            internal ArmenianDecoder(ArmenianEncoding encoding)
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
        #endregion

        #region Fields

        private readonly string encodingName;

        #endregion

        #region Encoding members override

        protected ArmenianEncoding(string encodingName)
        {
            this.encodingName = encodingName;
        }

        public sealed override string EncodingName
        {
            get { return encodingName; }
        }

        public sealed override bool IsSingleByte
        {
            get { return true; }
        }

        public sealed override int GetByteCount([NotNull] char[] chars, int index, int count)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be nonnegative");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be nonnegative");
            if (index >= chars.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index, string.Format("Index must be less than chars.Length ({0})", chars.Length));
            if (index + count > chars.Length)
                throw new ArgumentOutOfRangeException(nameof(count), count, string.Format("Count + index ({0}) must be less or equal than chars.Length ({1})", index, chars.Length));

            return count;
        }

        public sealed override int GetBytes([NotNull] char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (byteIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(byteIndex), byteIndex, "Byte index must be nonnegative");
            if (byteIndex >= bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(byteIndex), byteIndex, string.Format("Byte index must be less than bytes.Length ({0})", bytes.Length));

            var bytesCount = GetByteCount(chars, charIndex, charCount);
            if (byteIndex + bytesCount > bytes.Length)
                throw new ArgumentException(string.Format("Bytes (length {0}) does not have enough capacity from byteIndex to the end of the array to accommodate the resulting bytes", bytes.Length), nameof(bytes));

            for (int currentCharIndex = charIndex, currentByteIndex = byteIndex; currentCharIndex < charIndex + charCount; currentCharIndex++, currentByteIndex++)
            {
                bytes[currentByteIndex] = ConvertCharToByte(chars[currentCharIndex]);
            }

            return bytesCount;
        }

        public sealed override int GetMaxByteCount(int charCount)
        {
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount), charCount, "Char count must be nonnegative");

            return charCount;
        }

        public sealed override int GetCharCount([NotNull] byte[] bytes, int index, int count)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be nonnegative");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be nonnegative");
            if (index >= bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index, string.Format("Index must be less than bytes.Length ({0})", bytes.Length));
            if (index + count > bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(count), count, string.Format("Count + index ({0}) must be less or equal than bytes.Length ({1})", index, bytes.Length));

            return count;
        }

        public sealed override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            if (charIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(charIndex), charIndex, "Char index must be nonnegative");
            if (charIndex >= chars.Length)
                throw new ArgumentOutOfRangeException(nameof(charIndex), charIndex, string.Format("Char index must be less than chars.Length ({0})", chars.Length));

            var charCount = GetCharCount(bytes, byteIndex, byteCount);
            if (charIndex + charCount > chars.Length)
                throw new ArgumentException(string.Format("Chars (length {0}) does not have enough capacity from charIndex to the end of the array to accommodate the resulting characters", chars.Length), nameof(chars));

            for (int currentByteIndex = byteIndex, currentCharIndex = charIndex; currentByteIndex < byteIndex + byteCount; currentByteIndex++, currentCharIndex++)
            {
                chars[currentCharIndex] = ConvertByteToChar(bytes[currentByteIndex]);
            }

            return charCount;
        }

        public sealed override int GetMaxCharCount(int byteCount)
        {
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount), byteCount, "Byte count must be nonnegative");

            return byteCount;
        }

        public sealed override Decoder GetDecoder()
        {
            return new ArmenianDecoder(this);
        }

        public sealed override Encoder GetEncoder()
        {
            return new ArmenianEncoder(this);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Encode a Unicode character into to byte representing corresponding character in ArmSCII-8 codepage
        /// </summary>
        /// <param name="symbol">Unicode character to encode</param>
        /// <returns>byte representing character in ArmSCII-8 codepage </returns>
        protected virtual byte ConvertCharToByte(int symbol)
        {
            // Uppercase chars
            if (symbol >= 0x0531 && symbol <= 0x0556)
                return (byte)((symbol - 0x0531) * 2 + 0xB2);

            // Lowercase chars
            if (symbol >= 0x0561 && symbol <= 0x0586)
                return (byte)((symbol - 0x0561) * 2 + 0xB3);

            switch (symbol)
            {
                case 0x00A7: //armsection
                    return 0xA2;
                case 0x00AB: //armquotleft
                    return 0xA7;
                case 0x00BB: //armquotright
                    return 0xA6;
                case 0x0559: //apostrophe (appriximated)
                    return 0x90;
                case 0x055A: //armapostrophe
                    return 0xFE;
                case 0x055B: //armaccent
                    return 0xB0;
                case 0x055C: //armexclam
                    return 0xAF;
                case 0x055E: //armquestion
                    return 0xB1;
                case 0x055F: //armabbrev
                    return 0xAA;
                case 0x0587: //armew
                    return 0xA2;
                case 0x0589: //armfullstop
                    return 0xA3;
                case 0x058A: //armyentamna
                    return 0xAD;
                case 0x055D: //armsep
                    return 0xAA;
                case 0x2010: //armendash
                    return 0xAC;
                case 0x2012: //figure dash
                    return 0x2D;
                case 0x2013: //en dash
                    return 0x2D;
                case 0x2014: //em dash
                    return 0x2D;
                case 0x2015: //em dash
                    return 0xA8;
                case 0x201E: //double low-9 quotation mark
                    return 0x83;
                case 0x2020: //Dagger
                    return 0x85;
                case 0x2021: //Double Dagger 
                    return 0x86;
                case 0x2030: //per mile sign
                    return 0x88;
                case 0x2024: //armdot
                    return 0xA9;
                case 0x2026: //armellipsis 
                    return 0xAE;
                case 0x2033: //quotation mark
                    return 0x22;
                case 0x2116: //№ -> N
                    return 0x4E;
                default:
                    if (symbol <= 0x7F)
                        return (byte)symbol;
                    var result = defaultEncoding.GetBytes(new[] {(char)symbol});
                    return result.Length > 0 
                        ? result[0] 
                        : (byte)0x3F; // print "?" for unknown characters
            }
        }

        /// <summary>
        /// Decodes byte representing character in ArmSCII-8 codepage into corresponding Unicode character
        /// </summary>
        /// <param name="symbol">byte representing character in ArmSCII-8 codepage</param>
        /// <returns>Unicode character</returns>
        protected virtual char ConvertByteToChar(byte symbol)
        {
            //Uppercase and lowercase characters
            if (symbol >= 0xB2 && symbol <= 0xFD)
                return (char)(0x0531 + (symbol - 0xB2) / 2 + (((symbol - 0xB2) % 2) == 0 ? 0 : 0x30));

            switch (symbol)
            {
                case 0x2D: //figure dash
                    return (char)0x2012;
                case 0x83: //double low-9 quotation mark
                    return (char)0x201E;
                case 0x85: //dagger
                    return (char)0x2020;
                case 0x86: //double dagger 
                    return (char)0x2021;
                case 0x88: //per mile sign
                    return (char)0x2030;
                case 0xA2: //armsection
                    return (char)0x00A7;
                case 0xA3: //armfullstop
                    return (char)0x0589;
                case 0xA6: //armquotright
                    return (char)0x00BB;
                case 0xA7: //armquotleft
                    return (char)0x00AB;
                case 0xA8: //armemdash
                    return (char)0x2015;
                case 0xA9: //armdot
                    return (char)0x2024;
                case 0xAA: //armsep
                    return (char)0x055D;
                case 0xAC: //armendash
                    return (char)0x2010;
                case 0xAD: //armyentamna
                    return (char)0x058A;
                case 0xAE: //armellipsis 
                    return (char)0x2026;
                case 0xFE: //armapostrophe
                    return (char)0x055A;
                case 0xB0: //armaccent
                    return (char)0x055B;
                case 0xAF: //armexclam
                    return (char)0x055C;
                case 0xB1: //armquestion
                    return (char)0x055E;
                default:
                    if (symbol <= 0x7F)
                        return (char)symbol;
                    var result = defaultEncoding.GetChars(new[] {symbol});
                    return result.Length > 0
                        ? result[0]
                        : (char)0x3F; // print "?" for unknown characters
            }
        }
        #endregion
    }

    /// <summary>
    /// Represents ArmSCII-8 encoding
    /// </summary>
    public sealed class ArmSCII8Encoding : ArmenianEncoding
    {
        #region Consts
        public const string Name = "ArmSCII-8";
        #endregion

        #region Methods

        public ArmSCII8Encoding() : base(Name)
        {
        }

        #endregion
    }


    /// <summary>
    /// Represents ArmSCII-8a encoding
    /// </summary>
    public sealed class ArmSCII8aEncoding : ArmenianEncoding
    {
        #region Consts
        public const string Name = "ArmSCII-8a";
        #endregion

        #region Methods
        public ArmSCII8aEncoding() : base(Name)
        {
            
        }

        protected override byte ConvertCharToByte(int symbol)
        {
            switch (symbol)
            {
                case 0x0028: //armparenleft
                    return 0x28;
                case 0x0029: //armparenright
                    return 0x29;
                case 0x00A7: //armsection
                    return 0x26;
                case 0x00AB: //armquotleft
                    return 0xAE;
                case 0x00BB: //armquotright
                    return 0xAF;
                case 0x055B: //armaccent (approximated)
                    return 0x27;
                case 0x055C: //armexclam (approximated)
                    return 0x7E;
                case 0x055D: //armsep
                    return 0x60;
                case 0x055E: //armquestion
                    return 0xDF;
                case 0x0587: //armew (approximated)
                    return 0x26;
                case 0x0589: //armfullstop (approximated)
                    return 0x3A;
                case 0x058A: //armyentamna
                    return 0xDD;
                case 0x2010: //armendash
                    return 0x2D;
                case 0x2015: //armemdash
                    return 0x5F;
                case 0x2024: //armdot
                    return 0x2E;
                case 0x2026: //armellipsis 
                    return 0xDE;
                default:
                    return base.ConvertCharToByte(symbol);
            }
        }

        protected override char ConvertByteToChar(byte symbol)
        {
            switch (symbol)
            {
                case 0x28: //armparenleft
                    return (char)0x0028;
                case 0x29: //armparenright
                    return (char)0x0029;
                case 0x2D: //armendash
                    return (char)0x2010;
                case 0x5F: //armemdash
                    return (char)0x2015;
                case 0x60: //armsep
                    return (char)0x055D;
                case 0x90: //apostrophe
                    return (char)0x0559;
                case 0xAE: //armquotleft
                    return (char)0x00AB;
                case 0xAF: //armquotright
                    return (char)0x00BB;
                case 0xDD: //armyentamna
                    return (char)0x058A;
                case 0xDE: //armellipsis 
                    return (char)0x2026;
                case 0xDF: //armquestion
                    return (char)0x055E;
                default:
                    return base.ConvertByteToChar(symbol);
            }
        }
        #endregion
    }
}