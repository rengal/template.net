using System;
using System.Runtime.Serialization;

namespace Resto.Framework.Common
{
    [Serializable]
    public class RestoException : Exception
    {
        public RestoException()
        {
        }

        public RestoException(string message) : base(message)
        {
        }

        public RestoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RestoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}