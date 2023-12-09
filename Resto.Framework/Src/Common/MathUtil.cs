using System;
using System.Security.Cryptography;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class MathUtil
    {
        private const int base10 = 10;
        private const int asciiDiff = 48;
        private static readonly char[] hexDigits = "0123456789abcdef".ToCharArray();
        private static readonly char[] cHexa = { 'A', 'B', 'C', 'D', 'E', 'F' };
        private static readonly int[] iHexaNumeric = { 10, 11, 12, 13, 14, 15 };

        /// <summary>
        /// Возвращает хешированное значение <see cref="str"/>.
        /// </summary>
        /// <remarks>В случае изменения способа хеширования проверить все usage на предмет сравнения полученного хеша
        /// с готовым строковым представлением другого хеша.</remarks>
        public static string HexSHA1(string str)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                return ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(str)));
            }
        }

        public static string ToHexString(byte[] hash)
        {
            var hashStr = new char[hash.Length * 2];
            for (var i = 0; i < hash.Length; i++)
            {
                var b = hash[i];
                hashStr[i * 2] = hexDigits[b / 16];
                hashStr[i * 2 + 1] = hexDigits[b % 16];
            }
            return new string(hashStr);
        }

        public static byte[] FromHexString(string str)
        {
            var slen = str.Length;
            var array = new byte[(slen + 1) / 2];
            for (var i = 0; i < array.Length; i++)
            {
                var b = hexValue(str[slen - i * 2 - 1]);
                if (slen - i - 2 >= 0)
                {
                    b += (byte)(hexValue(str[slen - i * 2 - 2]) << 4);
                }
                array[array.Length - i - 1] = b;
            }
            return array;
        }

        private static byte hexValue(char c)
        {
            if (c >= '0' && c <= '9') return (byte)(c - '0');
            if (c >= 'a' && c <= 'f') return (byte)(c - 'a' + 10);
            if (c >= 'A' && c <= 'F') return (byte)(c - 'A' + 10);
            throw new RestoException("Illegal hex digit: " + c);
        }


        public static string DecimalToBase(Int64 iDec, int numbase)
        {
            var strBin = "";
            var result = new int[64];
            var MaxBit = 64;
            for (; iDec > 0; iDec /= numbase)
            {
                var rem = (int)(iDec % numbase);
                result[--MaxBit] = rem;
            }
            for (var i = 0; i < result.Length; i++)
                if ((int)result.GetValue(i) >= base10)
                    strBin += cHexa[(int)result.GetValue(i) % base10];
                else
                    strBin += result.GetValue(i);
            strBin = strBin.TrimStart(new[] { '0' });
            return strBin;
        }

        public static int BaseToDecimal(string sBase, int numbase)
        {
            var dec = 0;
            int b;
            var iProduct = 1;
            var sHexa = "";
            if (numbase > base10)
                for (var i = 0; i < cHexa.Length; i++)
                    sHexa += cHexa.GetValue(i).ToString();
            for (var i = sBase.Length - 1; i >= 0; i--, iProduct *= numbase)
            {
                var sValue = sBase[i].ToString();
                if (sValue.IndexOfAny(cHexa) >= 0)
                    b = iHexaNumeric[sHexa.IndexOf(sBase[i])];
                else
                    b = sBase[i] - asciiDiff;
                dec += (b * iProduct);
            }
            return dec;
        }

        /// <summary>
        /// Возвращает преобразованное значение к граничным величинам <paramref name="min"/>,
        /// <paramref name="max"/>, если значение выходит за их пределы, иначе - возвращает его же.
        /// </summary>
        [Pure]
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }

            return value;
        }
    }
}