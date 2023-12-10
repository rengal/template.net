using System;
using System.Security.Cryptography;
using System.Text;
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

        public static string GenerateShortHash(this Guid id)
        {
            const int hashCodeLength = 6;
            byte[] hashCode = id.ToString().ToLower().CalcHashCode(hashCodeLength);
            string shortToken = Convert.ToBase64String(hashCode).Replace('+', '-').Replace('/', '_');
            return shortToken;
        }

        private static byte[] CalcHashCode(this string input, int length)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                byte[] hash = new byte[length];
                Array.Copy(data, hash, length);
                return hash;
            }
        }
    }
}
