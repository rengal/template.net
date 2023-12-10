using System;
using System.Runtime.Serialization;

namespace Resto.Data
{
    [Serializable]
    public class RestoServiceException : Exception
    {
        /// <summary>
        /// Текст ошибки, сформированный сервером.
        /// Для DisplayableException --- локализованный.
        /// Начиная с 6.1. RMS-44702 содержит и description, и message локализуемых ошибок DisplayableException.
        /// См. на сервере: resto.rpc.ServiceResult.errorString, resto.rpc.ServiceResult.wrapError().
        /// </summary>
        private readonly string additionalMessage;

        public RestoServiceException(string message)
            : base(message)
        {
        }

        public RestoServiceException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public RestoServiceException(string message, string additionalMessage)
            : base(message)
        {
            this.additionalMessage = additionalMessage;
        }

        public RestoServiceException(string message, Exception exception, string additionalMessage)
            : base(message, exception)
        {
            this.additionalMessage = additionalMessage;
        }

        protected RestoServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string AdditionalMesssage
        {
            get { return additionalMessage; }
        }

        public bool IsWarning { get; set; }
    }
}
