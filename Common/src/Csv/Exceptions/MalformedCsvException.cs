//  Resto.Framework.Common.IO.Csv.MalformedCsvException
//  Copyright (c) 2005 Sébastien Lorion
//
//  MIT license (http://en.wikipedia.org/wiki/MIT_License)
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights 
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do so, 
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all 
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//  PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//  FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Resto.Common.Csv.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when a CSV file is malformed.
    /// </summary>
    [Serializable]
    public class MalformedCsvException : Exception
    {
        #region Fields

        /// <summary>
        /// Contains the message that describes the error.
        /// </summary>
        private string m_message;

        /// <summary>
        /// Contains the raw data when the error occurred.
        /// </summary>
        private string m_rawData;

        /// <summary>
        /// Contains the current field index.
        /// </summary>
        private int m_currentFieldIndex;

        /// <summary>
        /// Contains the current record index.
        /// </summary>
        private long m_currentRecordIndex;

        /// <summary>
        /// Contains the current position in the raw data.
        /// </summary>
        private int m_currentPosition;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedCsvException"/> class.
        /// </summary>
        public MalformedCsvException() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedCsvException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MalformedCsvException(string message) : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedCsvException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MalformedCsvException(string message, Exception innerException) : base(String.Empty, innerException)
        {
            m_message = message ?? string.Empty;

            m_rawData = string.Empty;
            m_currentPosition = -1;
            m_currentRecordIndex = -1;
            m_currentFieldIndex = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedCsvException"/> class.
        /// </summary>
        /// <param name="rawData">The raw data when the error occurred.</param>
        /// <param name="currentPosition">The current position in the raw data.</param>
        /// <param name="currentRecordIndex">The current record index.</param>
        /// <param name="currentFieldIndex">The current field index.</param>
        public MalformedCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex)
            : this(rawData, currentPosition, currentRecordIndex, currentFieldIndex, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedCsvException"/> class.
        /// </summary>
        /// <param name="rawData">The raw data when the error occurred.</param>
        /// <param name="currentPosition">The current position in the raw data.</param>
        /// <param name="currentRecordIndex">The current record index.</param>
        /// <param name="currentFieldIndex">The current field index.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MalformedCsvException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex, Exception innerException)
            : base(String.Empty, innerException)
        {
            m_rawData = rawData ?? string.Empty;
            m_currentPosition = currentPosition;
            m_currentRecordIndex = currentRecordIndex;
            m_currentFieldIndex = currentFieldIndex;

            m_message = String.Format(CultureInfo.InvariantCulture, ExceptionMessage.MalformedCsvException, m_currentRecordIndex, m_currentFieldIndex, m_currentPosition, m_rawData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedCsvException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected MalformedCsvException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_message = info.GetString("MyMessage");

            m_rawData = info.GetString("RawData");
            m_currentPosition = info.GetInt32("CurrentPosition");
            m_currentRecordIndex = info.GetInt64("CurrentRecordIndex");
            m_currentFieldIndex = info.GetInt32("CurrentFieldIndex");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the raw data when the error occurred.
        /// </summary>
        /// <value>The raw data when the error occurred.</value>
        public string RawData
        {
            get { return m_rawData; }
        }

        /// <summary>
        /// Gets the current position in the raw data.
        /// </summary>
        /// <value>The current position in the raw data.</value>
        public int CurrentPosition
        {
            get { return m_currentPosition; }
        }

        /// <summary>
        /// Gets the current record index.
        /// </summary>
        /// <value>The current record index.</value>
        public long CurrentRecordIndex
        {
            get { return m_currentRecordIndex; }
        }

        /// <summary>
        /// Gets the current field index.
        /// </summary>
        /// <value>The current record index.</value>
        public int CurrentFieldIndex
        {
            get { return m_currentFieldIndex; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value>A message that describes the current exception.</value>
        public override string Message
        {
            get { return m_message; }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MyMessage", m_message);

            info.AddValue("RawData", m_rawData);
            info.AddValue("CurrentPosition", m_currentPosition);
            info.AddValue("CurrentRecordIndex", m_currentRecordIndex);
            info.AddValue("CurrentFieldIndex", m_currentFieldIndex);
        }

        #endregion
    }
}