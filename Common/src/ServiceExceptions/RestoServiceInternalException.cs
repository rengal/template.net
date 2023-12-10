using System;
using System.Runtime.Serialization;

namespace Resto.Data
{
    [Serializable]
    public class RestoServiceInternalException : RestoServiceException
    {
        public RestoServiceInternalException(string message)
            : base(message)
        {
        }

        public RestoServiceInternalException(string message, Exception exception)
            : base(message, exception)
        {
        }

        protected RestoServiceInternalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
