using System;
using System.Net;
using System.Runtime.Serialization;
using log4net;
using Resto.Framework.Common;

namespace Resto.Data
{
    [Serializable]
    public class RestoServiceConnectionException : RestoServiceException
    {
        private static readonly ILog LOG = LogFactory.Instance.GetLogger(typeof(RestoServiceConnectionException));
        private readonly ConnectionResult result;

        public RestoServiceConnectionException(ConnectionResult result)
            : base(result.ToString())
        {
            this.result = result;
        }

        public RestoServiceConnectionException(ConnectionResult result, Exception innerException)
            : base(result.ToString(), innerException)
        {
            this.result = result;
        }

        protected RestoServiceConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ConnectionResult Result
        {
            get { return result; }
        }

        public static RestoServiceConnectionException Create(WebException exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            var authResult = ConnectionResult.CONNECTION_ERROR;
            if (exception.Response != null)
            {
                var authResultStr = exception.Response.Headers.Get(RPCHeaders.AUTH_RESULT.Name);
                try
                {
                    if (authResultStr != null)
                    {
                        authResult = (ConnectionResult) Enum.Parse(typeof(ConnectionResult), authResultStr);
                    }
                }
                catch (Exception ee)
                {
                    LOG.Warn("Cannot parse connection error", ee);
                    return new RestoServiceConnectionException(ConnectionResult.INTERNAL_ERROR, exception);
                }
            }
            return new RestoServiceConnectionException(authResult, exception);
        }
    }
}
