using System;
using System.Reflection;

namespace Resto.Framework.Common
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Получить исключение типа <typeparamref name="T"/> из обычного исключения путем приведения или извлечения из InnerException. 
        /// Может быть полезно для извлечения исключений из различных заверток, вроде TargetInvocationException. 
        /// </summary>
        /// <param name="ex">Корневое исключение</param>
        /// <returns>Исключение известного типа извлеченное из InnerException (или приведенное корневое) 
        /// / null - если не удалось найти исключение</returns>
        public static T GetException<T>(this Exception ex) where T : Exception
        {
            if (ex == null)
            {
                return null;
            }
            if (ex is T)
            {
                return (T)ex;
            }
            return GetException<T>(ex.InnerException);
        }

        private static readonly MethodInfo prepForRemoting =
            typeof(Exception).GetMethod("PrepForRemoting", BindingFlags.NonPublic | BindingFlags.Instance);

        public static Exception PrepareForRethrow(this Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            prepForRemoting.Invoke(exception, Array.Empty<object>());
            return exception;
        }
    }
}