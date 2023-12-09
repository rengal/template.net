using System;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class HexStringHelper
    {
        /// <summary>
        /// Преобразование бинарных данных в hex-строку
        /// </summary>
        [NotNull]
        public static string ToHexString([NotNull] this byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return data.ToHexString(0, data.Length);
        }

        [NotNull]
        public static string ToHexString([NotNull] this byte[] data, int offset, int count)
        {
            return BitConverter.ToString(data, offset, count).Replace("-", string.Empty);
        }

        [NotNull]
        public static byte[] ToByteArray([NotNull] this string value)
        {
            return Enumerable
                    .Range(0, value.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                    .ToArray();
        }
    }
}