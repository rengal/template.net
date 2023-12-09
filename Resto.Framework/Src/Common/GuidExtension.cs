using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class GuidExtension                            
    {
        [Pure]
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Генерирует 8-и символьный строковый токен в Base64 кодировке переданной строки.
        /// См. аналоги на
        /// JS dev/Server/Resto/web/js/hashUtils.js#generateShortToken
        /// Java resto.utils.log4j.MdcUtils#generateShortToken
        /// </summary>
        /// <param name="id">Исходная строка</param>
        /// <returns>8-символьный хеш в кодировке Base64 (URLSafe алфавит)</returns>
        [NotNull]
        [Pure]
        public static string GenerateShortHash(this Guid id)
        {
            const int hashCodeLength = 6;
            byte[] hashCode = id.ToString().ToLower().CalcHashCode(hashCodeLength);
            string shortToken = Convert.ToBase64String(hashCode).Replace('+', '-').Replace('/', '_');
            return shortToken;
        }
    }
}
