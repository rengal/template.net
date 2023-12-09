using System;

namespace Resto.Framework.Xml
{
    /// <summary>
    /// Класс отвечает за создание исключения, которое кидается если объект не содержится в кэше.
    /// </summary>
    public sealed class NotInCacheException : Exception
    {
        public NotInCacheException()
        {}

        public NotInCacheException(string message)
            : base(message)
        {}

        public NotInCacheException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}