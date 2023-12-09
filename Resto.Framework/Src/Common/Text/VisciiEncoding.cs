using System;
using System.Collections.Generic;
using System.Text;

namespace Resto.Framework.Text
{
    internal sealed class VisciiEncoding : Encoding
    {
        #region Const

        internal const string Name = "Viscii";
        private const byte UnknownCharCode = 0x3F;
        private const char UnknownChar = '?';

        #endregion

        #region Private Fields

        private static readonly Dictionary<byte, char> MapBytesToUnicode = new Dictionary<byte, char>
            {
                {0x02, '\u1EB2'},
                {0x05, '\u1EB4'},
                {0x06, '\u1EAA'},
                {0x14, '\u1EF6'},
                {0x19, '\u1EF8'},
                {0x1E, '\u1EF4'},
                {0x80, '\u1EA0'},
                {0x81, '\u1EAE'},
                {0x82, '\u1EB0'},
                {0x83, '\u1EB6'},
                {0x84, '\u1EA4'},
                {0x85, '\u1EA6'},
                {0x86, '\u1EA8'},
                {0x87, '\u1EAC'},
                {0x88, '\u1EBC'},
                {0x89, '\u1EB8'},
                {0x8A, '\u1EBE'},
                {0x8B, '\u1EC0'},
                {0x8C, '\u1EC2'},
                {0x8D, '\u1EC4'},
                {0x8E, '\u1EC6'},
                {0x8F, '\u1ED0'},
                {0x90, '\u1ED2'},
                {0x91, '\u1ED4'},
                {0x92, '\u1ED6'},
                {0x93, '\u1ED8'},
                {0x94, '\u1EE2'},
                {0x95, '\u1EDA'},
                {0x96, '\u1EDC'},
                {0x97, '\u1EDE'},
                {0x98, '\u1ECA'},
                {0x99, '\u1ECE'},
                {0x9A, '\u1ECC'},
                {0x9B, '\u1EC8'},
                {0x9C, '\u1EE6'},
                {0x9D, '\u0168'},
                {0x9E, '\u1EE4'},
                {0x9F, '\u1EF2'},
                {0xA0, '\u00D5'},
                {0xA1, '\u1EAF'},
                {0xA2, '\u1EB1'},
                {0xA3, '\u1EB7'},
                {0xA4, '\u1EA5'},
                {0xA5, '\u1EA7'},
                {0xA6, '\u1EA9'},
                {0xA7, '\u1EAD'},
                {0xA8, '\u1EBD'},
                {0xA9, '\u1EB9'},
                {0xAA, '\u1EBF'},
                {0xAB, '\u1EC1'},
                {0xAC, '\u1EC3'},
                {0xAD, '\u1EC5'},
                {0xAE, '\u1EC7'},
                {0xAF, '\u1ED1'},
                {0xB0, '\u1ED3'},
                {0xB1, '\u1ED5'},
                {0xB2, '\u1ED7'},
                {0xB3, '\u1EE0'},
                {0xB4, '\u01A0'},
                {0xB5, '\u1ED9'},
                {0xB6, '\u1EDD'},
                {0xB7, '\u1EDF'},
                {0xB8, '\u1ECB'},
                {0xB9, '\u1EF0'},
                {0xBA, '\u1EE8'},
                {0xBB, '\u1EEA'},
                {0xBC, '\u1EEC'},
                {0xBD, '\u01A1'},
                {0xBE, '\u1EDB'},
                {0xBF, '\u01AF'},
                {0xC0, '\u00C0'},
                {0xC1, '\u00C1'},
                {0xC2, '\u00C2'},
                {0xC3, '\u00C3'},
                {0xC4, '\u1EA2'},
                {0xC5, '\u0102'},
                {0xC6, '\u1EB3'},
                {0xC7, '\u1EB5'},
                {0xC8, '\u00C8'},
                {0xC9, '\u00C9'},
                {0xCA, '\u00CA'},
                {0xCB, '\u1EBA'},
                {0xCC, '\u00CC'},
                {0xCD, '\u00CD'},
                {0xCE, '\u0128'},
                {0xCF, '\u1EF3'},
                {0xD0, '\u0110'},
                {0xD1, '\u1EE9'},
                {0xD2, '\u00D2'},
                {0xD3, '\u00D3'},
                {0xD4, '\u00D4'},
                {0xD5, '\u1EA1'},
                {0xD6, '\u1EF7'},
                {0xD7, '\u1EEB'},
                {0xD8, '\u1EED'},
                {0xD9, '\u00D9'},
                {0xDA, '\u00DA'},
                {0xDB, '\u1EF9'},
                {0xDC, '\u1EF5'},
                {0xDD, '\u00DD'},
                {0xDE, '\u1EE1'},
                {0xDF, '\u01B0'},
                {0xE0, '\u00E0'},
                {0xE1, '\u00E1'},
                {0xE2, '\u00E2'},
                {0xE3, '\u00E3'},
                {0xE4, '\u1EA3'},
                {0xE5, '\u0103'},
                {0xE6, '\u1EEF'},
                {0xE7, '\u1EAB'},
                {0xE8, '\u00E8'},
                {0xE9, '\u00E9'},
                {0xEA, '\u00EA'},
                {0xEB, '\u1EBB'},
                {0xEC, '\u00EC'},
                {0xED, '\u00ED'},
                {0xEE, '\u0129'},
                {0xEF, '\u1EC9'},
                {0xF0, '\u0111'},
                {0xF1, '\u1EF1'},
                {0xF2, '\u00F2'},
                {0xF3, '\u00F3'},
                {0xF4, '\u00F4'},
                {0xF5, '\u00F5'},
                {0xF6, '\u1ECF'},
                {0xF7, '\u1ECD'},
                {0xF8, '\u1EE5'},
                {0xF9, '\u00F9'},
                {0xFA, '\u00FA'},
                {0xFB, '\u0169'},
                {0xFC, '\u1EE7'},
                {0xFD, '\u00FD'},
                {0xFE, '\u1EE3'},
                {0xFF, '\u1EEE'}
            };

        private static readonly Dictionary<char, byte> MapUnicodeToBytes = new Dictionary<char, byte>
            {
                {'\u00C0', 0xC0},
                {'\u00C1', 0xC1},
                {'\u00C2', 0xC2},
                {'\u00C3', 0xC3},
                {'\u00C8', 0xC8},
                {'\u00C9', 0xC9},
                {'\u00CA', 0xCA},
                {'\u00CC', 0xCC},
                {'\u00CD', 0xCD},
                {'\u00D2', 0xD2},
                {'\u00D3', 0xD3},
                {'\u00D4', 0xD4},
                {'\u00D5', 0xA0},
                {'\u00D9', 0xD9},
                {'\u00DA', 0xDA},
                {'\u00DD', 0xDD},
                {'\u00E0', 0xE0},
                {'\u00E1', 0xE1},
                {'\u00E2', 0xE2},
                {'\u00E3', 0xE3},
                {'\u00E8', 0xE8},
                {'\u00E9', 0xE9},
                {'\u00EA', 0xEA},
                {'\u00EC', 0xEC},
                {'\u00ED', 0xED},
                {'\u00F2', 0xF2},
                {'\u00F3', 0xF3},
                {'\u00F4', 0xF4},
                {'\u00F5', 0xF5},
                {'\u00F9', 0xF9},
                {'\u00FA', 0xFA},
                {'\u00FD', 0xFD},
                {'\u0102', 0xC5},
                {'\u0103', 0xE5},
                {'\u0110', 0xD0},
                {'\u0111', 0xF0},
                {'\u0128', 0xCE},
                {'\u0129', 0xEE},
                {'\u0168', 0x9D},
                {'\u0169', 0xFB},
                {'\u01A0', 0xB4},
                {'\u01A1', 0xBD},
                {'\u01AF', 0xBF},
                {'\u01B0', 0xDF},
                {'\u1EA0', 0x80},
                {'\u1EA1', 0xD5},
                {'\u1EA2', 0xC4},
                {'\u1EA3', 0xE4},
                {'\u1EA4', 0x84},
                {'\u1EA5', 0xA4},
                {'\u1EA6', 0x85},
                {'\u1EA7', 0xA5},
                {'\u1EA8', 0x86},
                {'\u1EA9', 0xA6},
                {'\u1EAA', 0x06},
                {'\u1EAB', 0xE7},
                {'\u1EAC', 0x87},
                {'\u1EAD', 0xA7},
                {'\u1EAE', 0x81},
                {'\u1EAF', 0xA1},
                {'\u1EB0', 0x82},
                {'\u1EB1', 0xA2},
                {'\u1EB2', 0x02},
                {'\u1EB3', 0xC6},
                {'\u1EB4', 0x05},
                {'\u1EB5', 0xC7},
                {'\u1EB6', 0x83},
                {'\u1EB7', 0xA3},
                {'\u1EB8', 0x89},
                {'\u1EB9', 0xA9},
                {'\u1EBA', 0xCB},
                {'\u1EBB', 0xEB},
                {'\u1EBC', 0x88},
                {'\u1EBD', 0xA8},
                {'\u1EBE', 0x8A},
                {'\u1EBF', 0xAA},
                {'\u1EC0', 0x8B},
                {'\u1EC1', 0xAB},
                {'\u1EC2', 0x8C},
                {'\u1EC3', 0xAC},
                {'\u1EC4', 0x8D},
                {'\u1EC5', 0xAD},
                {'\u1EC6', 0x8E},
                {'\u1EC7', 0xAE},
                {'\u1EC8', 0x9B},
                {'\u1EC9', 0xEF},
                {'\u1ECA', 0x98},
                {'\u1ECB', 0xB8},
                {'\u1ECC', 0x9A},
                {'\u1ECD', 0xF7},
                {'\u1ECE', 0x99},
                {'\u1ECF', 0xF6},
                {'\u1ED0', 0x8F},
                {'\u1ED1', 0xAF},
                {'\u1ED2', 0x90},
                {'\u1ED3', 0xB0},
                {'\u1ED4', 0x91},
                {'\u1ED5', 0xB1},
                {'\u1ED6', 0x92},
                {'\u1ED7', 0xB2},
                {'\u1ED8', 0x93},
                {'\u1ED9', 0xB5},
                {'\u1EDA', 0x95},
                {'\u1EDB', 0xBE},
                {'\u1EDC', 0x96},
                {'\u1EDD', 0xB6},
                {'\u1EDE', 0x97},
                {'\u1EDF', 0xB7},
                {'\u1EE0', 0xB3},
                {'\u1EE1', 0xDE},
                {'\u1EE2', 0x94},
                {'\u1EE3', 0xFE},
                {'\u1EE4', 0x9E},
                {'\u1EE5', 0xF8},
                {'\u1EE6', 0x9C},
                {'\u1EE7', 0xFC},
                {'\u1EE8', 0xBA},
                {'\u1EE9', 0xD1},
                {'\u1EEA', 0xBB},
                {'\u1EEB', 0xD7},
                {'\u1EEC', 0xBC},
                {'\u1EED', 0xD8},
                {'\u1EEE', 0xFF},
                {'\u1EEF', 0xE6},
                {'\u1EF0', 0xB9},
                {'\u1EF1', 0xF1},
                {'\u1EF2', 0x9F},
                {'\u1EF3', 0xCF},
                {'\u1EF4', 0x1E},
                {'\u1EF5', 0xDC},
                {'\u1EF6', 0x14},
                {'\u1EF7', 0xD6},
                {'\u1EF8', 0x19},
                {'\u1EF9', 0xDB},
            };

        #endregion

        #region System.Text.Encoding Methods

        public override string EncodingName
        {
            get { return Name; }
        }

        public override bool IsSingleByte
        {
            get { return true; }
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(index < 0 ? "index" : "count");
            if (chars.Length < index + count)
                throw new ArgumentOutOfRangeException(nameof(chars));
            return count;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (charIndex < 0 || charCount < 0)
                throw new ArgumentOutOfRangeException(charIndex < 0 ? "charIndex" : "charCount");
            if (chars.Length < charIndex + charCount)
                throw new ArgumentOutOfRangeException(nameof(chars));
            if (bytes.Length < byteIndex + charCount)
                throw new ArgumentOutOfRangeException(nameof(bytes));

            var curByteIndex = byteIndex;
            for (var i = charIndex; i < charIndex + charCount; i++)
            {
                bytes[curByteIndex++] = ConvertCharToByte(chars[i]);
            }
            return charCount;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(index < 0 ? "index" : "count");
            if (bytes.Length < index + count)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            return count;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            if (byteIndex < 0 || byteCount < 0)
                throw new ArgumentOutOfRangeException(byteIndex < 0 ? "byteIndex" : "byteCount");
            if (chars.Length < byteIndex + byteCount)
                throw new ArgumentOutOfRangeException(nameof(chars));
            var charCount = GetCharCount(bytes, byteIndex, byteCount);
            if (chars.Length < charIndex + charCount)
                throw new ArgumentOutOfRangeException(nameof(chars));

            var charCurIndex = charIndex;
            for (var i = byteIndex; i < byteIndex + byteCount; i++)
            {
                chars[charCurIndex++] = ConvertByteToChar(bytes[i]);
            }
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }

        #endregion

        #region Private Methods

        private static byte ConvertCharToByte(char value)
        {
            byte result;
            return MapUnicodeToBytes.TryGetValue(value, out result)
                ? result
                : value <= 0x7F ? System.Convert.ToByte(value) : UnknownCharCode;
        }

        private static char ConvertByteToChar(byte value)
        {
            char result;
            return MapBytesToUnicode.TryGetValue(value, out result)
                ? result
                : value <= 0x7F ? System.Convert.ToChar(value) : UnknownChar;
        }

        #endregion
    }
}
