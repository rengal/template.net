using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Common.Csv
{
    #region CsvStream

    /// <summary>
    /// Отвечает за создание csv файлов на основе данных и за чтение данных из csv файлов.
    /// </summary>
    public class CsvStream
    {
        #region Data members

        /// <summary>
        /// Зарезервированные символы.
        /// </summary>
        private readonly char[] reservedChars;

        /// <summary>
        /// Разделитель информации в одной записи.
        /// </summary>
        private readonly string delimiter;

        #endregion Data members

        #region Constructor

        public CsvStream(string delimiter, char[] reservedChars)
        {
            this.delimiter = delimiter;
            this.reservedChars = reservedChars;
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Записывает набор данных в csv файл.
        /// Если файла нет, то он создаётся, в противном случае перезаписывается.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="headers"></param>
        /// <param name="records"></param>
        public void WriteToFile(string path, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> records)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                if (headers.Any())
                {
                    sw.WriteLine(headers.ToArray().Join(delimiter));
                }

                if (records.Any())
                {
                    records.ToList().ForEach(r => sw.WriteLine(CreateRecordLine(r)));
                }
            }
        }

        /// <summary>
        /// Читает данные из csv файла.
        /// </summary>
        /// <param name="path">Путь к csv файлу.</param>
        /// <param name="hasHeaders">Файл содержит запись с заголовками.</param>
        public IEnumerable<IEnumerable<string>> ReadFromFile(string path, bool hasHeaders)
        {
            var records = new List<List<string>>();

            if (File.Exists(path))
            {
                var streamReader = new StreamReader(path, Encoding.Default);

                using (IDataReader csv = new CsvReader(streamReader, hasHeaders, delimiter[0]))
                {
                    while (csv.Read())
                    {
                        var record = new List<string>();

                        for (int i = 0; i < csv.FieldCount; i++)
                        {
                            record.Add(csv.GetString(i));
                        }
                        records.Add(record);
                    }
                }
            }
            else
            {                
                throw new RestoException(String.Format(Resources.FileNotFoundOrAccessDeniedException, path));
            }

            return records.ToArray();
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Создаёт запись csv файла.
        /// </summary>
        /// <param name="list">Набор данных.</param>
        private string CreateRecordLine(IEnumerable<string> list)
        {
            return list.Join(delimiter);
        }
        #endregion Private methods
    }
    #endregion CsvStream
}
