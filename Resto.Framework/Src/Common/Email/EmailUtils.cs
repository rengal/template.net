using System.Text.RegularExpressions;

namespace Resto.Framework.Common.Email
{
    public class EmailUtils
    {
        public const string EmailRegexPattern = @"[^\s]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9.-]+";
        public static readonly Regex EmailRegex = new Regex("^" + EmailRegexPattern + "$");

        // Метод для проверки правильности ввода e-mail
        public static bool IsValidEmail(string email)
        {
            if (email.IsNullOrEmpty())
                return false;
            if (email.NullSafeLength() > CommonConstants.MaxEmailLength)
                return false;
            return EmailRegex.IsMatch(email);
        }
    }
}
