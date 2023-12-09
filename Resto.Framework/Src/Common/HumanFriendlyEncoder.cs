using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс конвертации string (которое содержит только цифры) - string, причем string содержит только те символы,
    /// которые удобно вводить (без буквы "o", "l", "i").
    /// Символ "z" тоже исключен и используется в качестве разделителя.
    /// String (содержит только цифры) конвертируется по основанию 32 (32 символа).
    /// </summary>
    public static class HumanFriendlyEncoder
    {
        /// <summary>
        /// Алфавит, используемый при конвертации.
        /// </summary>
        private const string dictionary = "123456789abcdefghijkmnpqrstuvwxy";
        /// <summary>
        /// Символ используемый для разделения строк.
        /// </summary>
        private const string separator = "z";
        /// <summary>
        /// Символ, (не содержится в алфавите) который заменяется на нули перед первым символом строки не равным 0.
        /// К примеру "0111". Перейти к новому алфавиту для числа 111 не составляет труда, но тогда мы потеряем нули, 
        /// а это уже различные коды.
        /// </summary>
        private const char zero = '0';
        private static readonly Random rand = new Random();

        private static string Decode(string stringToDecode)
        {
            var length = dictionary.Length;
            var num = 0;

            foreach (var ch in stringToDecode.Replace(zero.ToString(), string.Empty).ToCharArray())
            {
                num *= length;
                num += dictionary.IndexOf(ch);
            }
            var result = new StringBuilder();
            result.Append(num);
            for (var i = 0; i < stringToDecode.Count(c => c == zero); i++)
            {
                result.Insert(0, zero);
            }
            return result.ToString();
        }

        public static List<string> DecodeInList(string stringToDecode)
        {
            return stringToDecode.Split(separator.ToCharArray()).Select(Decode).ToList();
        }

        private static string Encode(string stringToEncode)
        {
            var length = dictionary.Length;
            var sb = new StringBuilder();

            long numToEncode;
            if(!long.TryParse(stringToEncode, out numToEncode))
                throw new InvalidOperationException("Can encode only integer based number.");

            while (numToEncode != 0)
            {
                sb.Append(dictionary.ToCharArray()[(int)(numToEncode % length)]);
                numToEncode /= length;
            }
            var zerosBeforeNumber = stringToEncode.TakeWhile(c => c == zero).Count();
            for (var i = 0; i < zerosBeforeNumber; i++)
            {
                // Функции расшифрования все равно, где будут нули (важно только их кол-во).
                sb.Insert(rand.Next(0, sb.Length), zero);
            }

            var array = sb.ToString().ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string Encode(params string[] stringsToEncode)
        {
            var result = stringsToEncode.Aggregate(string.Empty, (current, el) => current + (Encode(el) + separator));
            return result.Trim(separator.ToCharArray());
        }
    }
}
