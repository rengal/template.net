using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization
{
    [Serializable]
    public sealed class SerializerException : Exception
    {
        [PublicAPI]
        public SerializerException()
        { }

        public SerializerException(string message)
            : base(message)
        { }

        public SerializerException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}