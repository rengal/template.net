using System;
using System.Security.Cryptography;
using System.Text;

namespace Resto.Common
{
    public static class CryptographyUtils
    {
        private static readonly Lazy<MD5> md5 = new Lazy<MD5>(MD5.Create);

        public static string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.Value.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
