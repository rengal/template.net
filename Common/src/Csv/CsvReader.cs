//  Resto.Framework.Common.IO.Csv.CsvReader
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Resto.Common.Csv.Exceptions;
using Debug = System.Diagnostics.Debug;

namespace Resto.Common.Csv
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to CSV data.
    /// </summary>
    public partial class CsvReader : IDataReader, IEnumerable<string[]>
    {
        #region Constants

        /// <summary>
        /// Defines the default buffer size.
        /// </summary>
        public const int DEFAULT_BUFFER_SIZE = 0x1000;

        /// <summary>
        /// Defines the default delimiter character separating each field.
        /// </summary>
        public const char DEFAULT_DELIMITER = ',';

        /// <summary>
        /// Defines the default quote character wrapping every field.
        /// </summary>
        public const char DEFAULT_QUOTE = '"';

        /// <summary>
        /// Defines the default escape character letting insert quotation characters inside a quoted field.
        /// </summary>
        public const char DEFAULT_ESCAPE = '"';

        /// <summary>
        /// Defines the default comment character indicating that a line is commented out.
        /// </summary>
        public const char DEFAULT_COMMENT = '#';

        #endregion

        #region Fields

        /// <summary>
        /// Contains the field header comparer.
        /// </summary>
        private static readonly StringComparer m_fieldHeaderComparer = StringComparer.CurrentCultureIgnoreCase;

        #region Settings

        /// <summary>
        /// Contains the <see cref="System.IO.TextReader "/> pointing to the CSV file.
        /// </summary>
        private TextReader m_reader;

        /// <summary>
        /// Contains the buffer size.
        /// </summary>
        private int m_bufferSize;

        /// <summary>
        /// Contains the comment character indicating that a line is commented out.
        /// </summary>
        private char m_comment;

        /// <summary>
        /// Contains the escape character letting insert quotation characters inside a quoted field.
        /// </summary>
        private char m_escape;

        /// <summary>
        /// Contains the delimiter character separating each field.
        /// </summary>
        private char m_delimiter;

        /// <summary>
        /// Contains the quotation character wrapping every field.
        /// </summary>
        private char m_quote;

        /// <summary>
        /// Indicates if spaces at the start and end of a field are trimmed.
        /// </summary>
        private bool m_trimSpaces;

        /// <summary>
        /// Indicates if field names are located on the first non commented line.
        /// </summary>
        private bool m_hasHeaders;

        /// <summary>
        /// Contains the default action to take when a parsing error has occurred.
        /// </summary>
        private ParseErrorAction m_defaultParseErrorAction;

        /// <summary>
        /// Contains the action to take when a field is missing.
        /// </summary>
        private MissingFieldAction m_missingFieldAction;

        /// <summary>
        /// Indicates if the reader supports multiline.
        /// </summary>
        private bool m_supportsMultiline;

        /// <summary>
        /// Indicates if the reader will skip empty lines.
        /// </summary>
        private bool m_skipEmptyLines;

        #endregion

        #region State

        /// <summary>
        /// Indicates if the class is initialized.
        /// </summary>
        private bool m_initialized;

        /// <summary>
        /// Contains the field headers.
        /// </summary>
        private string[] m_fieldHeaders;

        /// <summary>
        /// Contains the dictionary of field indexes by header. The key is the field name and the value is its index.
        /// </summary>
        private Dictionary<string, int> m_fieldHeaderIndexes;

        /// <summary>
        /// Contains the current record index in the CSV file.
        /// A value of <see cref="System.Int32.MinValue"/> means that the reader has not been initialized yet.
        /// Otherwise, a negative value means that no record has been read yet.
        /// </summary>
        private long m_currentRecordIndex;

        /// <summary>
        /// Contains the starting position of the next unread field.
        /// </summary>
        private int m_nextFieldStart;

        /// <summary>
        /// Contains the index of the next unread field.
        /// </summary>
        private int m_nextFieldIndex;

        /// <summary>
        /// Contains the array of the field values for the current record.
        /// A null value indicates that the field have not been parsed.
        /// </summary>
        private string[] m_fields;

        /// <summary>
        /// Contains the maximum number of fields to retrieve for each record.
        /// </summary>
        private int m_fieldCount;

        /// <summary>
        /// Contains the read buffer.
        /// </summary>
        private char[] m_buffer;

        /// <summary>
        /// Contains the current read buffer length.
        /// </summary>
        private int m_bufferLength;

        /// <summary>
        /// Indicates if the end of the reader has been reached.
        /// </summary>
        private bool m_eof;

        /// <summary>
        /// Indicates if the first record is in cache.
        /// This can happen when initializing a reader with no headers
        /// because one record must be read to get the field count automatically
        /// </summary>
        private bool m_firstRecordInCache;

        /// <summary>
        /// Indicates if fields are missing for the current record.
        /// Resets after each successful record read.
        /// </summary>
        private bool m_missingFieldsFlag;

        #endregion

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="reader">A <see cref="System.IO.TextReader"/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CsvReader(TextReader reader, bool hasHeaders)
            : this(reader, hasHeaders, DEFAULT_DELIMITER, DEFAULT_QUOTE, DEFAULT_ESCAPE, DEFAULT_COMMENT, true, DEFAULT_BUFFER_SIZE)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="reader">A <see cref="System.IO.TextReader "/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CsvReader(TextReader reader, bool hasHeaders, int bufferSize)
            : this(reader, hasHeaders, DEFAULT_DELIMITER, DEFAULT_QUOTE, DEFAULT_ESCAPE, DEFAULT_COMMENT, true, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="reader">A <see cref="System.IO.TextReader "/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CsvReader(TextReader reader, bool hasHeaders, char delimiter)
            : this(reader, hasHeaders, delimiter, DEFAULT_QUOTE, DEFAULT_ESCAPE, DEFAULT_COMMENT, true, DEFAULT_BUFFER_SIZE)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="reader">A <see cref="System.IO.TextReader "/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CsvReader(TextReader reader, bool hasHeaders, char delimiter, int bufferSize)
            : this(reader, hasHeaders, delimiter, DEFAULT_QUOTE, DEFAULT_ESCAPE, DEFAULT_COMMENT, true, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="reader">A <see cref="System.IO.TextReader "/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="quote">The quotation character wrapping every field (default is ''').</param>
        /// <param name="escape">
        /// The escape character letting insert quotation characters inside a quoted field (default is '\').
        /// If no escape character, set to '\0' to gain some performance.
        /// </param>
        /// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
        /// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     Cannot read from <paramref name="reader"/>.
        /// </exception>
        public CsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces)
            : this(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, DEFAULT_BUFFER_SIZE)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader"/> class.
        /// </summary>
        /// <param name="reader">A <see cref="System.IO.TextReader "/> pointing to the CSV file.</param>
        /// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
        /// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
        /// <param name="quote">The quotation character wrapping every field (default is ''').</param>
        /// <param name="escape">
        /// The escape character letting insert quotation characters inside a quoted field (default is '\').
        /// If no escape character, set to '\0' to gain some performance.
        /// </param>
        /// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
        /// <param name="trimSpaces"><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>. Default is <see langword="true"/>.</param>
        /// <param name="bufferSize">The buffer size in bytes.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="reader"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="bufferSize"/> must be 1 or more.
        /// </exception>
        public CsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces, int bufferSize)
        {
#if DEBUG
            m_allocStack = new StackTrace();
#endif

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bufferSize),
                    bufferSize,
                    ExceptionMessage.BufferSizeTooSmall);
            }

            m_bufferSize = bufferSize;

            if (reader is StreamReader)
            {
                var stream = ((StreamReader) reader).BaseStream;

                if (stream.CanSeek)
                {
                    //
                    // Handle bad implementations returning 0 or less
                    //
                    if (stream.Length > 0)
                    {
                        m_bufferSize = (int)Math.Min(
                            bufferSize,
                            stream.Length);
                    }
                }
            }

            m_reader = reader;
            m_delimiter = delimiter;
            m_quote = quote;
            m_escape = escape;
            m_comment = comment;

            m_hasHeaders = hasHeaders;
            m_trimSpaces = trimSpaces;
            m_supportsMultiline = true;
            m_skipEmptyLines = true;

            m_currentRecordIndex = -1;
            m_defaultParseErrorAction = ParseErrorAction.RaiseEvent;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when there is an error while parsing the CSV stream.
        /// </summary>
        public event EventHandler<ParseErrorEventArgs> ParseError;

        /// <summary>
        /// Raises the <see cref="M:ParseError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="ParseErrorEventArgs"/> that contains the event data.</param>
        protected virtual void OnParseError(ParseErrorEventArgs e)
        {
            var handler = ParseError;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Events

        #region Properties

        #region Settings

        /// <summary>
        /// Gets the comment character indicating that a line is commented out.
        /// </summary>
        /// <value>The comment character indicating that a line is commented out.</value>
        public char Comment
        {
            get
            {
                return m_comment;
            }
        }

        /// <summary>
        /// Gets the escape character letting insert quotation characters inside a quoted field.
        /// </summary>
        /// <value>The escape character letting insert quotation characters inside a quoted field.</value>
        public char Escape
        {
            get
            {
                return m_escape;
            }
        }

        /// <summary>
        /// Gets the delimiter character separating each field.
        /// </summary>
        /// <value>The delimiter character separating each field.</value>
        public char Delimiter
        {
            get
            {
                return m_delimiter;
            }
        }

        /// <summary>
        /// Gets the quotation character wrapping every field.
        /// </summary>
        /// <value>The quotation character wrapping every field.</value>
        public char Quote
        {
            get
            {
                return m_quote;
            }
        }

        /// <summary>
        /// Indicates if field names are located on the first non commented line.
        /// </summary>
        /// <value><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</value>
        public bool HasHeaders
        {
            get
            {
                return m_hasHeaders;
            }
        }

        /// <summary>
        /// Indicates if spaces at the start and end of a field are trimmed.
        /// </summary>
        /// <value><see langword="true"/> if spaces at the start and end of a field are trimmed, otherwise, <see langword="false"/>.</value>
        public bool TrimSpaces
        {
            get
            {
                return m_trimSpaces;
            }
        }

        /// <summary>
        /// Gets the buffer size.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return m_bufferSize;
            }
        }

        /// <summary>
        /// Gets or sets the default action to take when a parsing error has occurred.
        /// </summary>
        /// <value>The default action to take when a parsing error has occurred.</value>
        public ParseErrorAction DefaultParseErrorAction
        {
            get
            {
                return m_defaultParseErrorAction;
            }
            set
            {
                m_defaultParseErrorAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the action to take when a field is missing.
        /// </summary>
        /// <value>The action to take when a field is missing.</value>
        public MissingFieldAction MissingFieldAction
        {
            get
            {
                return m_missingFieldAction;
            }
            set
            {
                m_missingFieldAction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the reader supports multiline fields.
        /// </summary>
        /// <value>A value indicating if the reader supports multiline field.</value>
        public bool SupportsMultiline
        {
            get
            {
                return m_supportsMultiline;
            }
            set
            {
                m_supportsMultiline = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the reader will skip empty lines.
        /// </summary>
        /// <value>A value indicating if the reader will skip empty lines.</value>
        public bool SkipEmptyLines
        {
            get
            {
                return m_skipEmptyLines;
            }
            set
            {
                m_skipEmptyLines = value;
            }
        }

        #endregion Settings

        #region State

        /// <summary>
        /// Gets the maximum number of fields to retrieve for each record.
        /// </summary>
        /// <value>The maximum number of fields to retrieve for each record.</value>
        /// <exception cref="System.ObjectDisposedException">
        ///  The instance has been disposed of.
        /// </exception>
        public int FieldCount
        {
            get
            {
                EnsureInitialize();
                return m_fieldCount;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current stream position is at the end of the stream.
        /// </summary>
        /// <value><see langword="true"/> if the current stream position is at the end of the stream; otherwise <see langword="false"/>.</value>
        public virtual bool EndOfStream
        {
            get
            {
                return m_eof;
            }
        }

        /// <summary>
        /// Gets the field headers.
        /// </summary>
        /// <returns>The field headers or an empty array if headers are not supported.</returns>
        /// <exception cref="System.ObjectDisposedException">
        ///  The instance has been disposed of.
        /// </exception>
        public string[] GetFieldHeaders()
        {
            EnsureInitialize();

            Debug.Assert(m_fieldHeaders != null, "Field headers must be non null.");

            var fieldHeaders = new string[m_fieldHeaders.Length];

            for (var i = 0; i < fieldHeaders.Length; i++)
            {
                fieldHeaders[i] = m_fieldHeaders[i];
            }

            return fieldHeaders;
        }

        /// <summary>
        /// Gets the current record index in the CSV file.
        /// </summary>
        /// <value>The current record index in the CSV file.</value>
        public virtual long CurrentRecordIndex
        {
            get
            {
                return m_currentRecordIndex;
            }
        }

        #endregion Settings

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the field with the specified name and record position. <see cref="M:hasHeaders"/> must be <see langword="true"/>.
        /// </summary>
        /// <value>
        /// The field with the specified name and record position.
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="field"/> is <see langword="null"/> or an empty string.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The CSV does not have headers (<see cref="M:HasHeaders"/> property is <see langword="false"/>).
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="field"/> not found.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Record index must be > 0.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Cannot move to a previous record in forward-only mode.
        /// </exception>
        /// <exception cref="T:EndOfStreamException">
        ///     Cannot read record at <paramref name="record"/>.
        /// </exception>
        /// <exception cref="Resto.Framework.Common.IO.Csv.MalformedCsvException">
        ///     The CSV appears to be corrupt at the current position.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public string this[int record, string field]
        {
            get
            {
                MoveTo(record);
                return this[field];
            }
        }

        /// <summary>
        /// Gets the field at the specified index and record position.
        /// </summary>
        /// <value>
        /// The field at the specified index and record position.
        /// A <see langword="null"/> is returned if the field cannot be found for the record.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="field"/> must be included in [0, <see cref="M:FieldCount"/>[.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Record index must be > 0.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Cannot move to a previous record in forward-only mode.
        /// </exception>
        /// <exception cref="T:EndOfStreamException">
        ///     Cannot read record at <paramref name="record"/>.
        /// </exception>
        /// <exception cref="Resto.Framework.Common.IO.Csv.MalformedCsvException">
        ///     The CSV appears to be corrupt at the current position.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public string this[int record, int field]
        {
            get
            {
                MoveTo(record);
                return this[field];
            }
        }

        /// <summary>
        /// Gets the field with the specified name. <see cref="M:hasHeaders"/> must be <see langword="true"/>.
        /// </summary>
        /// <value>
        /// The field with the specified name.
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="field"/> is <see langword="null"/> or an empty string.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The CSV does not have headers (<see cref="M:HasHeaders"/> property is <see langword="false"/>).
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="field"/> not found.
        /// </exception>
        /// <exception cref="Resto.Framework.Common.IO.Csv.MalformedCsvException">
        ///     The CSV appears to be corrupt at the current position.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public string this[string field]
        {
            get
            {
                if (string.IsNullOrEmpty(field))
                    throw new ArgumentNullException(nameof(field));

                if (!m_hasHeaders)
                {
                    throw new InvalidOperationException(ExceptionMessage.NoHeaders);
                }

                var index = GetFieldIndex(field);

                if (index < 0)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldHeaderNotFound, field),
                        nameof(field));
                }

                return this[index];
            }
        }

        /// <summary>
        /// Gets the field at the specified index.
        /// </summary>
        /// <value>The field at the specified index.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="field"/> must be included in [0, <see cref="M:FieldCount"/>[.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     No record read yet. Call ReadLine() first.
        /// </exception>
        /// <exception cref="Resto.Framework.Common.IO.Csv.MalformedCsvException">
        ///     The CSV appears to be corrupt at the current position.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public virtual string this[int field]
        {
            get
            {
                return ReadField(field, false, false);
            }
        }

        #endregion

        #region Methods

        #region EnsureInitialize

        /// <summary>
        /// Ensures that the reader is initialized.
        /// </summary>
        private void EnsureInitialize()
        {
            if (!m_initialized)
            {
                ReadNextRecord(true, false);
            }

            Debug.Assert(m_fieldHeaders != null);
            Debug.Assert(m_fieldHeaders.Length > 0 ||
                (m_fieldHeaders.Length == 0 && m_fieldHeaderIndexes == null));
        }

        #endregion EnsureInitialize

        #region GetFieldIndex

        /// <summary>
        /// Gets the field index for the provided header.
        /// </summary>
        /// <param name="header">The header to look for.</param>
        /// <returns>The field index for the provided header. -1 if not found.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public int GetFieldIndex(string header)
        {
            EnsureInitialize();

            int index;

            if (m_fieldHeaderIndexes != null &&
                m_fieldHeaderIndexes.TryGetValue(header, out index))
            {
                return index;
            }
            return -1;
        }

        #endregion GetFieldIndex

        #region CopyCurrentRecordTo

        /// <summary>
        /// Copies the field array of the current record to a one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array"> The one-dimensional <see cref="T:Array"/> that is the destination of the fields of the current record.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     The number of fields in the record is greater than the available space from 0 to the end of <paramref name="array"/>.
        /// </exception>
        public void CopyCurrentRecordTo(string[] array)
        {
            CopyCurrentRecordTo(array, 0);
        }

        /// <summary>
        /// Copies the field array of the current record to a one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array"> The one-dimensional <see cref="T:Array"/> that is the destination of the fields of the current record.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is les than zero or is equal to or greater than the length <paramref name="array"/>. 
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// No current record.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     The number of fields in the record is greater than the available space from <paramref name="index"/> to the end of <paramref name="array"/>.
        /// </exception>
        public void CopyCurrentRecordTo(string[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, string.Empty);
            }

            if (m_currentRecordIndex < 0 || !m_initialized)
            {
                throw new InvalidOperationException(ExceptionMessage.NoCurrentRecord);
            }

            if (array.Length - index < m_fieldCount)
            {
                throw new ArgumentException(
                    ExceptionMessage.NotEnoughSpaceInArray,
                    nameof(array));
            }

            for (var i = 0; i < m_fieldCount; i++)
            {
                array[index + i] = this[i];
            }
        }

        #endregion CopyCurrentRecordTo

        #region GetCurrentRawData

        /// <summary>
        /// Gets the current raw CSV data.
        /// </summary>
        /// <remarks>Used for exception handling purpose.</remarks>
        /// <returns>The current raw CSV data.</returns>
        public string GetCurrentRawData()
        {
            if (m_buffer != null && m_bufferLength > 0)
            {
                return new String(
                    m_buffer,
                    0,
                    m_bufferLength);
            }
            return string.Empty;
        }

        #endregion GetCurrentRawData

        #region IsWhiteSpace

        /// <summary>
        /// Indicates whether the specified Unicode character is categorized as white space.
        /// </summary>
        /// <param name="c">A Unicode character.</param>
        /// <returns><see langword="true"/> if <paramref name="c"/> is white space; otherwise, <see langword="false"/>.</returns>
        private bool IsWhiteSpace(char c)
        {
            // Handle cases where the delimiter is a whitespace (e.g. tab)
            if (c == m_delimiter)
            {
                return false;
            }
            else
            {
                // See char.IsLatin1(char c) in Reflector
                if (c <= '\x00ff')
                {
                    return (c == ' ' || c == '\t');
                }
                else
                {
                    return (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator);
                }
            }
        }

        #endregion IsWhiteSpace

        #region MoveTo

        /// <summary>
        /// Moves to the specified record index.
        /// </summary>
        /// <param name="record">The record index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Record index must be > 0.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Cannot move to a previous record in forward-only mode.
        /// </exception>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public virtual void MoveTo(long record)
        {
            if (record < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(record),
                    record,
                    ExceptionMessage.RecordIndexLessThanZero);
            }

            if (record < m_currentRecordIndex)
            {
                throw new InvalidOperationException(ExceptionMessage.CannotMovePreviousRecordInForwardOnly);
            }

            // Get number of record to read

            var offset = record - m_currentRecordIndex;

            if (offset > 0)
            {
                do
                {
                    if (!ReadNextRecord())
                    {
                        throw new EndOfStreamException(
                            String.Format(CultureInfo.InvariantCulture, ExceptionMessage.CannotReadRecordAtIndex, m_currentRecordIndex - offset));
                    }
                }
                while (--offset > 0);
            }
        }

        #endregion MoveTo

        #region ParseNewLine

        /// <summary>
        /// Parses a new line delimiter.
        /// </summary>
        /// <param name="pos">The starting position of the parsing. Will contain the resulting end position.</param>
        /// <returns><see langword="true"/> if a new line delimiter was found; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        private bool ParseNewLine(ref int pos)
        {
            Debug.Assert(pos <= m_bufferLength);

            //
            // Check if already at the end of the buffer
            //
            if (pos == m_bufferLength)
            {
                pos = 0;

                if (!ReadBuffer())
                {
                    return false;
                }
            }

            var c = m_buffer[pos];

            // Treat \r as new line only if it's not the delimiter

            if (c == '\r' && m_delimiter != '\r')
            {
                pos++;

                // Skip following \n (if there is one)

                if (pos < m_bufferLength)
                {
                    if (m_buffer[pos] == '\n')
                        pos++;
                }
                else
                {
                    if (ReadBuffer())
                    {
                        if (m_buffer[0] == '\n')
                            pos = 1;
                        else
                            pos = 0;
                    }
                }

                if (pos >= m_bufferLength)
                {
                    ReadBuffer();
                    pos = 0;
                }

                return true;
            }
            else if (c == '\n')
            {
                pos++;

                if (pos >= m_bufferLength)
                {
                    ReadBuffer();
                    pos = 0;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the character at the specified position is a new line delimiter.
        /// </summary>
        /// <param name="pos">The position of the character to verify.</param>
        /// <returns>
        ///     <see langword="true"/> if the character at the specified position is a new line delimiter; otherwise, <see langword="false"/>.
        /// </returns>
        private bool IsNewLine(int pos)
        {
            Debug.Assert(pos < m_bufferLength);

            var c = m_buffer[pos];

            if (c == '\n')
            {
                return true;
            }
            else if (c == '\r' && m_delimiter != '\r')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion ParseNewLine

        #region ReadBuffer

        /// <summary>
        /// Fills the buffer with data from the reader.
        /// </summary>
        /// <returns><see langword="true"/> if data was successfully read; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        private bool ReadBuffer()
        {
            if (m_eof)
            {
                return false;
            }

            CheckDisposed();

            m_bufferLength = m_reader.Read(
                m_buffer,
                0,
                m_bufferSize);

            if (m_bufferLength > 0)
            {
                return true;
            }
            else
            {
                m_eof = true;
                m_buffer = null;

                return false;
            }
        }

        #endregion ReadBuffer

        #region ReadField

        /// <summary>
        /// Reads the field at the specified index.
        /// Any unread fields with an inferior index will also be read as part of the required parsing.
        /// </summary>
        /// <param name="field">The field index.</param>
        /// <param name="initializing">Indicates if the reader is currently initializing.</param>
        /// <param name="discardValue">Indicates if the value(s) are discarded.</param>
        /// <returns>
        /// The field at the specified index. 
        /// A <see langword="null"/> indicates that an error occurred or that the last field has been reached during initialization.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="field"/> is out of range.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     There is no current record.
        /// </exception>
        /// <exception cref="MissingFieldCsvException">
        ///     The CSV data appears to be missing a field.
        /// </exception>
        /// <exception cref="MalformedCsvException">
        ///     The CSV data appears to be malformed.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        private string ReadField(int field, bool initializing, bool discardValue)
        {
            if (!initializing)
            {
                if (field < 0 || field >= m_fieldCount)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(field),
                        field,
                        String.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, field));
                }

                if (m_currentRecordIndex < 0)
                {
                    throw new InvalidOperationException(ExceptionMessage.NoCurrentRecord);
                }
            }

            // Directly return field if cached
            if (m_fields[field] != null)
            {
                return m_fields[field];
            }
            else if (m_missingFieldsFlag)
            {
                return HandleMissingField(
                    null,
                    field,
                    ref m_nextFieldStart);
            }

            CheckDisposed();

            var index = m_nextFieldIndex;

            while (index < field + 1)
            {
                // Handle case where stated start of field is past buffer
                // This can occur because _nextFieldStart is simply 1 + last char position of previous field
                if (m_nextFieldStart == m_bufferLength)
                {
                    m_nextFieldStart = 0;

                    // Possible EOF will be handled later (see Handle_EOF1)
                    ReadBuffer();
                }

                string value = null;
                var eol = false;

                if (m_missingFieldsFlag)
                {
                    value = HandleMissingField(value, index, ref m_nextFieldStart);
                }
                else if (m_nextFieldStart == m_bufferLength)
                {
                    // Handle_EOF1: Handle EOF here

                    // If current field is the requested field, then the value of the field is "" as in "f1,f2,f3,(\s*)"
                    // otherwise, the CSV is malformed

                    if (index == field)
                    {
                        if (!discardValue)
                        {
                            value = string.Empty;
                            m_fields[index] = value;
                        }
                    }
                    else
                    {
                        value = HandleMissingField(
                            value,
                            index,
                            ref m_nextFieldStart);
                    }
                }
                else
                {
                    // Trim spaces at start
                    if (m_trimSpaces)
                    {
                        SkipWhiteSpaces(ref m_nextFieldStart);
                    }

                    if (m_eof)
                    {
                        value = string.Empty;
                    }
                    else if (m_buffer[m_nextFieldStart] != m_quote)
                    {
                        // Non-quoted field

                        var start = m_nextFieldStart;
                        var pos = m_nextFieldStart;

                        for (; ; )
                        {
                            while (pos < m_bufferLength)
                            {
                                var c = m_buffer[pos];

                                if (c == m_delimiter)
                                {
                                    m_nextFieldStart = pos + 1;

                                    break;
                                }
                                else if (c == '\r' || c == '\n')
                                {
                                    m_nextFieldStart = pos;
                                    eol = true;

                                    break;
                                }
                                else
                                    pos++;
                            }

                            if (pos < m_bufferLength)
                            {
                                break;
                            }
                            else
                            {
                                if (!discardValue)
                                {
                                    value += new string(m_buffer, start, pos - start);
                                }

                                start = 0;
                                pos = 0;
                                m_nextFieldStart = 0;

                                if (!ReadBuffer())
                                    break;
                            }
                        }

                        if (!discardValue)
                        {
                            if (!m_trimSpaces)
                            {
                                if (!m_eof && pos > start)
                                {
                                    value += new string(m_buffer, start, pos - start);
                                }
                            }
                            else
                            {
                                if (!m_eof && pos > start)
                                {
                                    // Do the trimming
                                    pos--;
                                    while (pos > -1 && IsWhiteSpace(m_buffer[pos]))
                                        pos--;
                                    pos++;

                                    if (pos > 0)
                                    {
                                        value += new string(m_buffer, start, pos - start);
                                    }
                                }
                                else
                                    pos = -1;

                                // If pos <= 0, that means the trimming went past buffer start,
                                // and the concatenated value needs to be trimmed too.
                                if (pos <= 0)
                                {
                                    pos = (value == null ? -1 : value.Length - 1);

                                    // Do the trimming
                                    while (pos > -1 && IsWhiteSpace(value[pos]))
                                        pos--;

                                    pos++;

                                    if (pos > 0 && pos != value.Length)
                                    {
                                        value = value.Substring(0, pos);
                                    }
                                }
                            }

                            if (value == null)
                                value = string.Empty;
                        }

                        if (eol || m_eof)
                        {
                            eol = ParseNewLine(ref m_nextFieldStart);

                            // Reaching a new line is ok as long as the parser is initializing or it is the last field
                            if (!initializing && index != m_fieldCount - 1)
                            {
                                if (value != null && value.Length == 0)
                                    value = null;

                                value = HandleMissingField(
                                    value,
                                    index,
                                    ref m_nextFieldStart);
                            }
                        }

                        if (!discardValue)
                        {
                            m_fields[index] = value;
                        }
                    }
                    else
                    {
                        // Quoted field

                        // Skip quote
                        var start = m_nextFieldStart + 1;
                        var pos = start;

                        var quoted = true;
                        var escaped = false;

                        for (; ; )
                        {
                            while (pos < m_bufferLength)
                            {
                                var c = m_buffer[pos];

                                if (escaped)
                                {
                                    escaped = false;
                                    start = pos;
                                }
                                // IF current char is escape AND (escape and quote are different OR next char is a quote)
                                else if (c == m_escape && (m_escape != m_quote || (pos + 1 < m_bufferLength && m_buffer[pos + 1] == m_quote) || (pos + 1 == m_bufferLength && m_reader.Peek() == m_quote)))
                                {
                                    if (!discardValue)
                                    {
                                        value += new string(m_buffer, start, pos - start);
                                    }

                                    escaped = true;
                                }
                                else if (c == m_quote)
                                {
                                    quoted = false;
                                    break;
                                }

                                pos++;
                            }

                            if (!quoted)
                                break;
                            else
                            {
                                if (!discardValue && !escaped)
                                {
                                    value += new string(m_buffer, start, pos - start);
                                }

                                start = 0;
                                pos = 0;
                                m_nextFieldStart = 0;

                                if (!ReadBuffer())
                                {
                                    HandleParseError(new MalformedCsvException(
                                        GetCurrentRawData(),
                                        m_nextFieldStart,
                                        Math.Max(0, m_currentRecordIndex), index),
                                        ref m_nextFieldStart);

                                    return null;
                                }
                            }
                        }

                        if (!m_eof)
                        {
                            // Append remaining parsed buffer content
                            if (!discardValue && pos > start)
                            {
                                value += new string(m_buffer, start, pos - start);
                            }

                            // Skip quote
                            m_nextFieldStart = pos + 1;

                            // Skip whitespaces between the quote and the delimiter/eol
                            SkipWhiteSpaces(ref m_nextFieldStart);

                            // Skip delimiter
                            bool delimiterSkipped;
                            if (m_nextFieldStart < m_bufferLength && m_buffer[m_nextFieldStart] == m_delimiter)
                            {
                                m_nextFieldStart++;
                                delimiterSkipped = true;
                            }
                            else
                            {
                                delimiterSkipped = false;
                            }

                            // Skip new line delimiter if initializing or last field
                            // (if the next field is missing, it will be caught when parsed)
                            if (!m_eof && !delimiterSkipped && (initializing || index == m_fieldCount - 1))
                            {
                                eol = ParseNewLine(ref m_nextFieldStart);
                            }

                            // If no delimiter is present after the quoted field and it is not the last field, then it is a parsing error
                            if (!delimiterSkipped && !m_eof && !(eol || IsNewLine(m_nextFieldStart)))
                            {
                                HandleParseError(new MalformedCsvException(
                                    GetCurrentRawData(),
                                    m_nextFieldStart,
                                    Math.Max(0, m_currentRecordIndex), index),
                                    ref m_nextFieldStart);
                            }
                        }

                        if (!discardValue)
                        {
                            if (value == null)
                                value = string.Empty;

                            m_fields[index] = value;
                        }
                    }
                }

                if (eol)
                {
                    m_nextFieldIndex = 0;
                }
                else
                {
                    m_nextFieldIndex = Math.Max(index + 1, m_nextFieldIndex);
                }

                if (index == field)
                {
                    // If initializing, return null to signify the last field has been reached

                    if (initializing && (eol || m_eof))
                    {
                        return null;
                    }
                    else
                    {
                        return value;
                    }
                }

                index++;
            }

            // Getting here is bad ...
            HandleParseError(new MalformedCsvException(
                GetCurrentRawData(),
                m_nextFieldStart,
                Math.Max(0, m_currentRecordIndex), index),
                ref m_nextFieldStart);

            return null;
        }

        #endregion ReadField

        #region ReadNextRecord

        /// <summary>
        /// Reads the next record.
        /// </summary>
        /// <returns><see langword="true"/> if a record has been successfully reads; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        ///  The instance has been disposed of.
        /// </exception>
        public bool ReadNextRecord()
        {
            return ReadNextRecord(false, false);
        }

        /// <summary>
        /// Reads the next record.
        /// </summary>
        /// <param name="onlyReadHeaders">
        /// Indicates if the reader will proceed to the next record after having read headers.
        /// <see langword="true"/> if it stops after having read headers; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skipToNextLine">
        /// Indicates if the reader will skip directly to the next line without parsing the current one. 
        /// To be used when an error occurs.
        /// </param>
        /// <returns><see langword="true"/> if a record has been successfully reads; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        protected virtual bool ReadNextRecord(bool onlyReadHeaders, bool skipToNextLine)
        {
            if (m_eof)
            {
                if (m_firstRecordInCache)
                {
                    m_firstRecordInCache = false;
                    m_currentRecordIndex++;

                    return true;
                }
                return false;
            }

            CheckDisposed();

            if (!m_initialized)
            {
                m_buffer = new char[m_bufferSize];

                // will be replaced if and when headers are read
                m_fieldHeaders = new string[0];

                if (!ReadBuffer())
                    return false;

                if (!SkipEmptyAndCommentedLines(ref m_nextFieldStart))
                {
                    return false;
                }

                // Keep growing _fields array until the last field has been found
                // and then resize it to its final correct size

                m_fieldCount = 0;
                m_fields = new string[16];

                while (ReadField(m_fieldCount, true, false) != null)
                {
                    m_fieldCount++;

                    if (m_fieldCount == m_fields.Length)
                    {
                        Array.Resize<string>(
                            ref m_fields,
                            (m_fieldCount + 1) * 2);
                    }
                }

                // _fieldCount contains the last field index, but it must contains the field count,
                // so increment by 1
                m_fieldCount++;

                if (m_fields.Length != m_fieldCount)
                {
                    Array.Resize<string>(
                        ref m_fields,
                        m_fieldCount);
                }

                m_initialized = true;

                // If headers are present, call ReadNextRecord again
                if (m_hasHeaders)
                {
                    // Don't count first record as it was the headers
                    m_currentRecordIndex = -1;

                    m_firstRecordInCache = false;

                    m_fieldHeaders = new string[m_fieldCount];
                    m_fieldHeaderIndexes = new Dictionary<string, int>(
                        m_fieldCount,
                        m_fieldHeaderComparer);

                    for (var i = 0; i < m_fields.Length; i++)
                    {
                        var filed = m_fields[i];
                        m_fieldHeaders[i] = filed;
                        m_fieldHeaderIndexes[filed] = i;
                    }

                    //
                    // Proceed to first record
                    //
                    if (!onlyReadHeaders)
                    {
                        // Calling again ReadNextRecord() seems to be simpler, 
                        // but in fact would probably cause many subtle bugs because the derived does not expect a recursive behavior
                        // so simply do what is needed here and no more.

                        if (!SkipEmptyAndCommentedLines(ref m_nextFieldStart))
                        {
                            return false;
                        }

                        Array.Clear(m_fields, 0, m_fields.Length);
                        m_nextFieldIndex = 0;

                        m_currentRecordIndex++;

                        return true;
                    }
                }
                else
                {
                    if (onlyReadHeaders)
                    {
                        m_firstRecordInCache = true;
                        m_currentRecordIndex = -1;
                    }
                    else
                    {
                        m_firstRecordInCache = false;
                        m_currentRecordIndex = 0;
                    }
                }
            }
            else
            {
                // Advance to next line or last field
                if (skipToNextLine)
                {
                    SkipToNextLine(ref m_nextFieldStart);
                }
                else if (m_currentRecordIndex > -1 && !m_missingFieldsFlag)
                {
                    if (m_supportsMultiline)
                    {
                        ReadField(m_fieldCount - 1, false, true);
                    }
                    else if (m_nextFieldIndex > 0)
                    {
                        SkipToNextLine(ref m_nextFieldStart);
                    }
                }

                if (!m_firstRecordInCache &&
                    !SkipEmptyAndCommentedLines(ref m_nextFieldStart))
                {
                    return false;
                }

                // Check to see if the first record is in cache.
                // This can happen when initializing a reader with no headers
                // because one record must be read to get the field count automatically
                if (m_firstRecordInCache)
                {
                    m_firstRecordInCache = false;
                }
                else
                {
                    Array.Clear(m_fields, 0, m_fields.Length);

                    m_nextFieldIndex = 0;
                }

                m_missingFieldsFlag = false;
                m_currentRecordIndex++;
            }

            return true;
        }

        #endregion ReadNextRecord

        #region SkipEmptyAndCommentedLines

        /// <summary>
        /// Skips empty and commented lines.
        /// If the end of the buffer is reached, its content be discarded and filled again from the reader.
        /// </summary>
        /// <param name="pos">
        /// The position in the buffer where to start parsing.
        /// Will contains the resulting position after the operation.
        /// </param>
        /// <returns><see langword="true"/> if the end of the reader has not been reached; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        private bool SkipEmptyAndCommentedLines(ref int pos)
        {
            if (pos < m_bufferLength)
            {
                DoSkipEmptyAndCommentedLines(ref pos);
            }

            while (pos >= m_bufferLength && !m_eof)
            {
                if (ReadBuffer())
                {
                    pos = 0;
                    DoSkipEmptyAndCommentedLines(ref pos);
                }
                else
                {
                    return false;
                }
            }

            return !m_eof;
        }

        /// <summary>
        /// <para>Worker method.</para>
        /// <para>Skips empty and commented lines.</para>
        /// </summary>
        /// <param name="pos">
        /// The position in the buffer where to start parsing.
        /// Will contains the resulting position after the operation.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        private void DoSkipEmptyAndCommentedLines(ref int pos)
        {
            while (pos < m_bufferLength)
            {
                if (m_buffer[pos] == m_comment)
                {
                    pos++;
                    SkipToNextLine(ref pos);
                }
                else if (m_skipEmptyLines && ParseNewLine(ref pos))
                    continue;
                else
                    break;
            }
        }

        #endregion SkipEmptyAndCommentedLines

        #region SkipWhiteSpaces

        /// <summary>
        /// Skips whitespace characters.
        /// </summary>
        /// <param name="pos">The starting position of the parsing. Will contain the resulting end position.</param>
        /// <returns><see langword="true"/> if the end of the reader has not been reached; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        ///  The instance has been disposed of.
        /// </exception>
        private bool SkipWhiteSpaces(ref int pos)
        {
            for (; ; )
            {
                while (pos < m_bufferLength && IsWhiteSpace(m_buffer[pos]))
                    pos++;

                if (pos < m_bufferLength)
                {
                    break;
                }
                pos = 0;

                if (!ReadBuffer())
                    return false;
            }

            return true;
        }

        #endregion SkipWhiteSpaces

        #region SkipToNextLine

        /// <summary>
        /// Skips ahead to the next NewLine character.
        /// If the end of the buffer is reached, its content be discarded and filled again from the reader.
        /// </summary>
        /// <param name="pos">
        /// The position in the buffer where to start parsing.
        /// Will contains the resulting position after the operation.
        /// </param>
        /// <returns><see langword="true"/> if the end of the reader has not been reached; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        private bool SkipToNextLine(ref int pos)
        {
            // ((pos = 0) == 0) is a little trick to reset position inline
            while ((pos < m_bufferLength || (ReadBuffer() &&
                ((pos = 0) == 0))) &&
                !ParseNewLine(ref pos))
            {
                pos++;
            }

            return !m_eof;
        }

        #endregion SkipToNextLine

        #region HandleParseError

        /// <summary>
        /// Handles a parsing error.
        /// </summary>
        /// <param name="error">The parsing error that occurred.</param>
        /// <param name="pos">The current position in the buffer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="error"/> is <see langword="null"/>.
        /// </exception>
        private void HandleParseError(MalformedCsvException error, ref int pos)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            switch (m_defaultParseErrorAction)
            {
                case ParseErrorAction.ThrowException:
                    throw error;

                case ParseErrorAction.RaiseEvent:
                    var e = new ParseErrorEventArgs(error, ParseErrorAction.ThrowException);
                    OnParseError(e);

                    switch (e.Action)
                    {
                        case ParseErrorAction.ThrowException:
                            throw e.Error;

                        case ParseErrorAction.RaiseEvent:
                            throw new InvalidOperationException(
                                String.Format(CultureInfo.InvariantCulture, ExceptionMessage.ParseErrorActionInvalidInsideParseErrorEvent, e.Action),
                                e.Error);

                        case ParseErrorAction.AdvanceToNextLine:
                            if (pos >= 0)
                            {
                                ReadNextRecord(false, true);
                            }
                            break;

                        default:
                            throw new NotSupportedException(
                                String.Format(CultureInfo.InvariantCulture, ExceptionMessage.ParseErrorActionNotSupported, e.Action),
                                e.Error);
                    }
                    break;

                case ParseErrorAction.AdvanceToNextLine:
                    if (pos >= 0)
                    {
                        ReadNextRecord(false, true);
                    }
                    break;

                default:
                    throw new NotSupportedException(
                        String.Format(CultureInfo.InvariantCulture, ExceptionMessage.ParseErrorActionNotSupported, m_defaultParseErrorAction),
                        error);
            }
        }

        #endregion HandleParseError

        #region HandleMissingField

        /// <summary>
      /// Handles a missing field error.
      /// </summary>
      /// <param name="value">The partially parsed value, if available.</param>
      /// <param name="fieldIndex">The missing field index.</param>
      /// <param name="currentPosition">The current position in the raw data.</param>
      /// <returns>
      /// The resulting value according to <see cref="M:MissingFieldAction"/>.
      /// If the action is set to <see cref="T:MissingFieldAction.TreatAsParseError"/>,
      /// then the parse error will be handled according to <see cref="DefaultParseErrorAction"/>.
      /// </returns>
      private string HandleMissingField(string value, int fieldIndex, ref int currentPosition)
      {
        if (fieldIndex < 0 || fieldIndex >= m_fieldCount)
          throw new ArgumentOutOfRangeException(nameof(fieldIndex), fieldIndex, string.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, fieldIndex));

        m_missingFieldsFlag = true;

        for (var i = fieldIndex + 1; i < m_fieldCount; i++)
          m_fields[i] = null;

        if (value != null)
          return value;
        else
        {
          switch (m_missingFieldAction)
          {
            case MissingFieldAction.ParseError:
              HandleParseError(new MissingFieldCsvException(GetCurrentRawData(), currentPosition, Math.Max(0, m_currentRecordIndex), fieldIndex), ref currentPosition);
              return value;

            case MissingFieldAction.ReplaceByEmpty:
              return string.Empty;

            case MissingFieldAction.ReplaceByNull:
              return null;

            default:
              throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, ExceptionMessage.MissingFieldActionNotSupported, m_missingFieldAction));
          }
        }
        }

        #endregion HandleMissingField

        #endregion Methods

        #region IDataReader support methods

        /// <summary>
        /// Validates the state of the data reader.
        /// </summary>
        /// <param name="validations">The validations to accomplish.</param>
        /// <exception cref="System.InvalidOperationException">
        /// No current record.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// This operation is invalid when the reader is closed.
        /// </exception>
        private void ValidateDataReader(DataReaderValidations validations)
        {
            if ((validations & DataReaderValidations.IsInitialized) != 0 &&
                !m_initialized)
            {
                throw new InvalidOperationException(ExceptionMessage.NoCurrentRecord);
            }

            if ((validations & DataReaderValidations.IsNotClosed) != 0 &&
                m_isDisposed)
            {
                throw new InvalidOperationException(ExceptionMessage.ReaderClosed);
            }
        }

        /// <summary>
        /// Copy the value of the specified field to an array.
        /// </summary>
        /// <param name="field">The index of the field.</param>
        /// <param name="fieldOffset">The offset in the field value.</param>
        /// <param name="destinationArray">The destination array where the field value will be copied.</param>
        /// <param name="destinationOffset">The destination array offset.</param>
        /// <param name="length">The number of characters to copy from the field value.</param>
        private long CopyFieldToArray(int field, long fieldOffset, Array destinationArray, int destinationOffset, int length)
        {
            EnsureInitialize();

            if (field < 0 || field >= m_fieldCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(field),
                    field,
                    String.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, field));
            }

            if (fieldOffset < 0 || fieldOffset >= int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldOffset));
            }

            // Array.Copy(...) will do the remaining argument checks

            if (length == 0)
            {
                return 0;
            }

            var value = this[field] ?? string.Empty;

            Debug.Assert(fieldOffset < int.MaxValue);

            Debug.Assert(destinationArray.GetType() == typeof(char[]) || destinationArray.GetType() == typeof(byte[]));

            if (destinationArray.GetType() == typeof(char[]))
            {
                Array.Copy(
                    value.ToCharArray((int)fieldOffset, length),
                    0,
                    destinationArray,
                    destinationOffset,length);
            }
            else
            {
                var chars = value.ToCharArray((int)fieldOffset, length);
                var source = new byte[chars.Length];

                for (var i = 0; i < chars.Length; i++)
                {
                    source[i] = Convert.ToByte(chars[i]);
                }

                Array.Copy(
                    source,
                    0,
                    destinationArray,
                    destinationOffset,
                    length);
            }

            return length;
        }

        #endregion IDataReader support methods

        #region IDataReader Members

        int IDataReader.RecordsAffected
        {
            get
            {
                // For SELECT statements, -1 must be returned.
                return -1;
            }
        }

        bool IDataReader.IsClosed
        {
            get
            {
                return m_eof;
            }
        }

        bool IDataReader.NextResult()
        {
            ValidateDataReader(DataReaderValidations.IsNotClosed);

            return false;
        }

        void IDataReader.Close()
        {
            Dispose();
        }

        bool IDataReader.Read()
        {
            ValidateDataReader(DataReaderValidations.IsNotClosed);

            return ReadNextRecord();
        }

        int IDataReader.Depth
        {
            get
            {
                ValidateDataReader(DataReaderValidations.IsNotClosed);

                return 0;
            }
        }

        DataTable IDataReader.GetSchemaTable()
        {
            EnsureInitialize();
            ValidateDataReader(DataReaderValidations.IsNotClosed);

            var schema = new DataTable("SchemaTable");
            schema.Locale = CultureInfo.InvariantCulture;
            schema.MinimumCapacity = m_fieldCount;

            schema.Columns.Add(SchemaTableColumn.AllowDBNull, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseColumnName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseSchemaName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.BaseTableName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof(int)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ColumnSize, typeof(int)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.DataType, typeof(object)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsAliased, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsExpression, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsKey, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsLong, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.IsUnique, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.NumericPrecision, typeof(short)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.NumericScale, typeof(short)).ReadOnly = true;
            schema.Columns.Add(SchemaTableColumn.ProviderType, typeof(int)).ReadOnly = true;

            schema.Columns.Add(SchemaTableOptionalColumn.BaseCatalogName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.BaseServerName, typeof(string)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsHidden, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsReadOnly, typeof(bool)).ReadOnly = true;
            schema.Columns.Add(SchemaTableOptionalColumn.IsRowVersion, typeof(bool)).ReadOnly = true;

            string[] columnNames;

            if (m_hasHeaders)
            {
                columnNames = m_fieldHeaders;
            }
            else
            {
                columnNames = new string[m_fieldCount];

                for (var i = 0; i < m_fieldCount; i++)
                {
                    columnNames[i] = "Column" + i.ToString(CultureInfo.InvariantCulture);
                }
            }

            // null marks columns that will change for each row
            var schemaRow = new object[] {
                    true,                   // 00- AllowDBNull
                    null,                   // 01- BaseColumnName
                    string.Empty,           // 02- BaseSchemaName
                    string.Empty,           // 03- BaseTableName
                    null,                   // 04- ColumnName
                    null,                   // 05- ColumnOrdinal
                    int.MaxValue,           // 06- ColumnSize
                    typeof(string),         // 07- DataType
                    false,                  // 08- IsAliased
                    false,                  // 09- IsExpression
                    false,                  // 10- IsKey
                    false,                  // 11- IsLong
                    false,                  // 12- IsUnique
                    DBNull.Value,           // 13- NumericPrecision
                    DBNull.Value,           // 14- NumericScale
                    (int) DbType.String,    // 15- ProviderType

                    string.Empty,           // 16- BaseCatalogName
                    string.Empty,           // 17- BaseServerName
                    false,                  // 18- IsAutoIncrement
                    false,                  // 19- IsHidden
                    true,                   // 20- IsReadOnly
                    false                   // 21- IsRowVersion
              };

            for (var i = 0; i < columnNames.Length; i++)
            {
                schemaRow[1] = columnNames[i]; // Base column name
                schemaRow[4] = columnNames[i]; // Column name
                schemaRow[5] = i; // Column ordinal

                schema.Rows.Add(schemaRow);
            }

            return schema;
        }

        #endregion IDataReader Members

        #region IDataRecord Members

        int IDataRecord.GetInt32(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            var value = this[i];

            return Int32.Parse(value ?? string.Empty, CultureInfo.CurrentCulture);
        }

        object IDataRecord.this[string name]
        {
            get
            {
                ValidateDataReader(DataReaderValidations.IsInitialized |
                    DataReaderValidations.IsNotClosed);

                return this[name];
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                ValidateDataReader(DataReaderValidations.IsInitialized |
                    DataReaderValidations.IsNotClosed);

                return this[i];
            }
        }

        object IDataRecord.GetValue(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            if (((IDataRecord)this).IsDBNull(i))
            {
                return DBNull.Value;
            }
            else
            {
                return this[i];
            }
        }

        bool IDataRecord.IsDBNull(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return (string.IsNullOrEmpty(this[i]));
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return CopyFieldToArray(
                i,
                fieldOffset,
                buffer,
                bufferoffset,
                length);
        }

        byte IDataRecord.GetByte(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return Byte.Parse(
                this[i],
                CultureInfo.CurrentCulture);
        }

        Type IDataRecord.GetFieldType(int i)
        {
            EnsureInitialize();

            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            if (i < 0 || i >= m_fieldCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(i),
                    i,
                    String.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, i));
            }

            return typeof(string);
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return Decimal.Parse(this[i], CultureInfo.CurrentCulture);
        }

        int IDataRecord.GetValues(object[] values)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            var record = (IDataRecord) this;

            for (var i = 0; i < m_fieldCount; i++)
            {
                values[i] = record.GetValue(i);
            }

            return m_fieldCount;
        }

        string IDataRecord.GetName(int i)
        {
            EnsureInitialize();

            ValidateDataReader(DataReaderValidations.IsNotClosed);

            if (i < 0 || i >= m_fieldCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(i),
                    i,
                    String.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldIndexOutOfRange, i));
            }

            if (m_hasHeaders)
            {
                return m_fieldHeaders[i];
            }
            else
            {
                return "Column" + i.ToString(CultureInfo.InvariantCulture);
            }
        }

        long IDataRecord.GetInt64(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return Int64.Parse(this[i], CultureInfo.CurrentCulture);
        }

        double IDataRecord.GetDouble(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return Double.Parse(this[i], CultureInfo.CurrentCulture);
        }

        bool IDataRecord.GetBoolean(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            var value = this[i];

            int result;

            if (Int32.TryParse(value, out result))
            {
                return (result != 0);
            }
            else
            {
                return Boolean.Parse(value);
            }
        }

        Guid IDataRecord.GetGuid(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return new Guid(this[i]);
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return DateTime.Parse(this[i], CultureInfo.CurrentCulture);
        }

        int IDataRecord.GetOrdinal(string name)
        {
            EnsureInitialize();

            ValidateDataReader(DataReaderValidations.IsNotClosed);

            int index;

            if (!m_fieldHeaderIndexes.TryGetValue(name, out index))
            {
                throw new ArgumentException(
                    String.Format(CultureInfo.InvariantCulture, ExceptionMessage.FieldHeaderNotFound, name),
                    nameof(name));
            }

            return index;
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return typeof(string).FullName;
        }

        float IDataRecord.GetFloat(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized | DataReaderValidations.IsNotClosed);
            return Single.Parse(this[i], CultureInfo.CurrentCulture);
        }

        IDataReader IDataRecord.GetData(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            if (i == 0)
                return this;
            else
                return null;
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return CopyFieldToArray(
                i,
                fieldoffset,
                buffer,
                bufferoffset,
                length);
        }

        string IDataRecord.GetString(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return this[i];
        }

        char IDataRecord.GetChar(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return Char.Parse(this[i]);
        }

        short IDataRecord.GetInt16(int i)
        {
            ValidateDataReader(DataReaderValidations.IsInitialized |
                DataReaderValidations.IsNotClosed);

            return Int16.Parse(this[i], CultureInfo.CurrentCulture);
        }

        #endregion IDataRecord Members

        #region IEnumerable<string[]> Members

        /// <summary>
        /// Returns an <see cref="CsvReader.RecordEnumerator"/> that can iterate through CSV records.
        /// </summary>
        /// <returns>An <see cref="CsvReader.RecordEnumerator"/> that can iterate through CSV records.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        public RecordEnumerator GetEnumerator()
        {
            return new RecordEnumerator(this);
        }

        /// <summary>
        /// Returns an <see cref="T:System.Collections.Generics.IEnumerator"/>  that can iterate through CSV records.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generics.IEnumerator"/>  that can iterate through CSV records.</returns>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///  The instance has been disposed of.
        /// </exception>
        IEnumerator<string[]> IEnumerable<string[]>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable<string[]> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an <see cref="System.Collections.IEnumerator"/> that can iterate through CSV records.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> that can iterate through CSV records.</returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The instance has been disposed of.
        /// </exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IDisposable members

#if DEBUG
        /// <summary>
        /// Contains the stack when the object was allocated.
        /// </summary>
        private StackTrace m_allocStack;
#endif

        /// <summary>
        /// Contains the disposed status flag.
        /// </summary>
        private bool m_isDisposed = false;

        /// <summary>
        /// Contains the locking object for multi-threading purpose.
        /// </summary>
        private readonly object m_lock = new object();

        /// <summary>
        /// Occurs when the instance is disposed of.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Gets a value indicating whether the instance has been disposed of.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the instance has been disposed of; otherwise, <see langword="false"/>.
        /// </value>
        [Browsable(false)]
        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }

        /// <summary>
        /// Raises the <see cref="M:Disposed"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected virtual void OnDisposed(EventArgs e)
        {
            var handler = Disposed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Checks if the instance has been disposed of, and if it has, throws an <see cref="System.ObjectDisposedException"/>; otherwise, does nothing.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///         The instance has been disposed of.
        /// </exception>
        /// <remarks>
        ///     Derived classes should call this method at the start of all methods and properties that should not be accessed after a call to <see cref="M:Dispose()"/>.
        /// </remarks>
        protected void CheckDisposed()
        {
            if (m_isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Releases all resources used by the instance.
        /// </summary>
        /// <remarks>
        ///     Calls <see cref="M:Dispose(Boolean)"/> with the disposing parameter set to <see langword="true"/> to free unmanaged and managed resources.
        /// </remarks>
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Refer to http://www.bluebytesoftware.com/blog/PermaLink,guid,88e62cdf-5919-4ac7-bc33-20c06ae539ae.aspx
            // Refer to http://www.gotdotnet.com/team/libraries/whitepapers/resourcemanagement/resourcemanagement.aspx

            // No exception should ever be thrown except in critical scenarios.
            // Unhandled exceptions during finalization will tear down the process.
            if (!m_isDisposed)
            {
                try
                {
                    // Dispose-time code should call Dispose() on all owned objects that implement the IDisposable interface. 
                    // "owned" means objects whose lifetime is solely controlled by the container. 
                    // In cases where ownership is not as straightforward, techniques such as HandleCollector can be used.  
                    // Large managed object fields should be nulled out.

                    // Dispose-time code should also set references of all owned objects to null, after disposing them. This will allow the referenced objects to be garbage collected even if not all references to the "parent" are released. It may be a significant memory consumption win if the referenced objects are large, such as big arrays, collections, etc. 
                    if (disposing)
                    {
                        // Acquire a lock on the object while disposing.

                        if (m_reader != null)
                        {
                            lock (m_lock)
                            {
                                if (m_reader != null)
                                {
                                    m_reader.Dispose();

                                    m_reader = null;
                                    m_buffer = null;
                                    m_eof = true;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Ensure that the flag is set
                    m_isDisposed = true;

                    // Catch any issues about firing an event on an already disposed object.
                    try
                    {
                        OnDisposed(EventArgs.Empty);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the instance is reclaimed by garbage collection.
        /// </summary>
        ~CsvReader()
        {
#if DEBUG
            Debug.WriteLine("FinalizableObject was not disposed" + m_allocStack.ToString());
#endif

            Dispose(false);
        }

        #endregion
    }
}