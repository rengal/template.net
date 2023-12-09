using System;
using System.Security.Cryptography;
using System.Text;

namespace Resto.Framework.Common
{
    public static class GuidGenerator
    {
        public static Guid Next()
        {
            return Guid.NewGuid();
        }

        public static Guid GetHashGuid(string str)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.GetEncoding("utf-16LE").GetBytes(str));
                return new Guid(string.Concat(hash.ToHexString()));
            }
        }

        public static Guid ParseGuid(string text)
        {
            if (text[0] == '\'')
            {
                var arr = new byte[16];
                if (text.Length > 17)
                {
                    throw new RestoException("Too long custom uuid string: " + text);
                }
                for (var i = 1; i < text.Length - 1; i++)
                {
                    var c = text[i];
                    if ((c & 0xFF) != c)
                    {
                        throw new RestoException("Char out of range: '" + c + "'");
                    }
                    arr[i] = (byte)c;
                }
                return new Guid(arr);
            }
            return new Guid(text);
        }

        public static string FormatGuid(Guid id)
        {
            return id.ToString();
        }
    }
}