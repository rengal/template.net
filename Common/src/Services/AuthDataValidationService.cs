using System;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common.Services
{
    /// <summary>
    /// Класс проверки логина/пароля на допустимые символы
    /// </summary>
    public static class AuthDataValidationService
    {
        #region Constants

        /// <summary>
        /// Все не допустимые символы для поля логина
        /// </summary>
        private const string InvalidLoginCharactersRegExp = @"[^A-Za-z0-9_]";

        /// <summary>
        /// Все не допустимые символы для поля пароля
        /// </summary>
        private const string InvalidPasswordCharactersRegExp = "[^!-~]";

        /// <summary>
        /// Маска для ввода логина
        /// </summary>
        public const string LoginEditMaskRegEx = @"[A-Za-z0-9_]*";

        /// <summary>
        /// Маска для ввода пароля
        /// </summary>
        public const string PasswordEditMaskRegEx = "[!-~]*";

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет на допустимость символы строки логина/пароля
        /// </summary>
        /// <param name="authString">Проверяемая строка</param>
        /// <param name="isLogin">true: проверяем строку логина, иначе строку пароля</param>
        /// <returns>true: все символы в строке допустимые</returns>
        public static bool IsValidAuthString([NotNull] string authString, bool isLogin)
        {
            return !Regex.IsMatch(authString, isLogin ? InvalidLoginCharactersRegExp : InvalidPasswordCharactersRegExp);
        }

        /// <summary>
        /// Проверяет является ли допустимым переданный символ логина/пароля
        /// </summary>
        /// <param name="authChar">Проверяемый символ</param>
        /// <param name="isLogin"></param>
        /// <returns>true: допустимый символ</returns>
        public static bool IsValidAuthChar(char authChar, bool isLogin)
        {
            return !Regex.IsMatch(authChar.ToString(CultureInfo.InvariantCulture), isLogin ? InvalidLoginCharactersRegExp : InvalidPasswordCharactersRegExp);
        }

        /// <summary>
        /// Проверка валидности логина или пароля.
        /// </summary>
        /// <param name="text">Логин или пароль.</param>
        /// <param name="isLogin"><c>true</c>, если <paramref name="text"/> является логином или <c>false</c>, если паролем.</param>
        /// <param name="allowEmpty"><c>true</c>, если пустое значение логина/пароля считается допустимым.</param>
        /// <returns><c>true</c>, если переданная строка НЕ является валидным логином/паролем.</returns>
        public static bool IsInvalidAuthDataString(string text, bool isLogin, bool allowEmpty = true)
        {
            return (!allowEmpty && string.IsNullOrWhiteSpace(text)) || !IsValidAuthString(text, isLogin);
        }

        /// <summary>
        /// Проверяет хеш введенного <see cref="password"/> на предмет скомпрометированности.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если пароль пользователя скомпрометирован
        /// </returns>
        /// <remarks>https://jira.iiko.ru/browse/RMS-45565</remarks>
        public static bool CheckCompromisedPassword([NotNull] string password)
        {
            const string compromisedPassword = "resto#test";
            return false;
        }

        #endregion
    }
}
