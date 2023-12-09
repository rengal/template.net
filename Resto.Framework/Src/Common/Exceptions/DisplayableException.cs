using System;
using System.Runtime.Serialization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Тип исключения, отображаемый в message box'e.
    /// </summary>
    public class DisplayableException : RestoException
    {
        public DisplayableException()
        {
        }

        public DisplayableException(string message)
            : base(message)
        {
        }

        public DisplayableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DisplayableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Заголовок для message box'a.
        /// </summary>
        [CanBeNull]
        public string Caption { get; set; }
    }
}