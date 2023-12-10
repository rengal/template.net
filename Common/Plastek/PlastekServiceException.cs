using System;

namespace Resto.Common.Plastek
{
    public class PlastekServiceException : ApplicationException
    {
        private readonly PlastekServiceError errorCode;
        public PlastekServiceError ErrorCode
        {
            get { return errorCode; }
        }

        public PlastekServiceException(PlastekServiceError errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.errorCode = errorCode;
        }
    }
}