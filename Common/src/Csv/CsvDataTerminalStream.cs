using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Common;
using Resto.Common.Properties;

namespace Resto.Common.Csv
{    
    #region CsvDataTerminalStream

    /// <summary>
    /// Отвечает за создание csv файлов на основе данных и за чтение данных из csv файлов, для ТСД.
    /// </summary>
    public class CsvDataTerminalStream
    {

        #region Data Members

        /// <summary>
        /// Список хедеров набора данных номенклатуры.
        /// </summary>
        private readonly List<string> nomenclatureHeaders = new List<string>
                                                                {
                                                                    Resources.Barcodes,
                                                                    Resources.Id,
                                                                    Resources.ProductNumber,
                                                                    Resources.ProductName,
                                                                    Resources.ProductAmount,
                                                                    Resources.PackageId,
                                                                    Resources.PackageName,
                                                                    Resources.QuantityPolicyId,
                                                                    Resources.QuantityInContainer,
                                                                    Resources.Identifier
                                                                };

        /// <summary>
        /// Список хедеров набора данных для документов.
        /// Конфигурация с ячейками.
        /// Должен удовлетворять шаблону заданному в соответсвующем файле для конкретного документа в
        /// папках SyncUtil\Templates\Download и SyncUtil\Templates\Upload.
        /// "Штрихкоды";"ид";"Артикул";"Наименование";"Место хранения";"Паллета";"План";"Факт";"ИдУпаковки"
        /// </summary>
        protected readonly List<string> headersWithCellPallet = new List<string>
                                                                    {
                                                                        Resources.Barcodes,
                                                                        Resources.Id,
                                                                        Resources.ProductNumber,
                                                                        Resources.ProductName,
                                                                        Resources.CellName,
                                                                        Resources.Pallet,
                                                                        Resources.PlanAmount,
                                                                        Resources.FactAmount,
                                                                        Resources.PackageId
                                                                    };
        
        /// <summary>
        /// Список хедеров набора данных для документов.
        /// Конфигурация с ячейками.
        /// Должен удовлетворять шаблону заданному в соответсвующем файле для конкретного документа в
        /// папках SyncUtil\Templates\Download и SyncUtil\Templates\Upload.
        /// "Code";"Штрихкоды";"ид";"Артикул";"Наименование";"Место хранения";"Паллета";"План";"Факт";"ИдУпаковки"
        /// </summary>
        protected readonly List<string> headersWithCodeCellPallet = new List<string>
                                                                        {
                                                                            "Code",
                                                                            Resources.Barcodes,
                                                                            Resources.Id,
                                                                            Resources.ProductNumber,
                                                                            Resources.ProductName,
                                                                            Resources.CellName,
                                                                            Resources.Pallet,
                                                                            Resources.PlanAmount,
                                                                            Resources.FactAmount,
                                                                            Resources.PackageId
                                                                        };

        /// <summary>
        /// Для скрытия зарезервированных символов.
        /// </summary>
        private const string quote = "\"";

        /// <summary>
        /// Пустой штрихкод.
        /// </summary>
        protected const string blankBarcode = "0";

        /// <summary>
        /// Разделитель штрихкодов в одной записи.
        /// </summary>
        private readonly string barCodeDelimiter;

        private readonly CsvStream stream;

        private readonly char[] reservedChars = { ',', ';', '"' };

        #endregion Data Members

        #region Constructor

        public CsvDataTerminalStream(string recordDelimiter, string barCodeDelimiter)
        {
            stream = new CsvStream(recordDelimiter, reservedChars);
            this.barCodeDelimiter = barCodeDelimiter;
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Читает данные с csv файла, который сгенерировал ТСД.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="hasHeaders">True, если в файле есть строка с заголовками элементов.</param>
        public IEnumerable<CsvDataTerminalItem> ReadFromFile(string path, bool hasHeaders)
        {
            foreach (List<string> r in stream.ReadFromFile(path, hasHeaders))
            {
                //создание позиции инвентуры на основе csv записи, с учётом шаблона csv файла
                if (r.Count == headersWithCodeCellPallet.Count)
                {
                    //парсинг списка штрихкодов из 1-го элемента записи "Штрихкоды;Ид;Артикул;Название;Заявлено;Принято;НазваниеЕдИзм;ИдЕдИзм;ИдУпаковки"
                    //4094874564535;PP;Полотенце «Пушистое»;0;10; - задает три штрихкода 4094874564542, 77345345, 11CF
                    //;TP;Зубной порошок;0;2; - штрихкод не указан, товар будет распознаваться по артикулу
                    yield return new CsvDataTerminalItem(r[0], r[1].Split(barCodeDelimiter.ToCharArray()),r[2], r[3], r[4], r[7], r[8], r[9], r[9], "");
                }
                else if (r.Count == headersWithCellPallet.Count)
                {
                    yield return new CsvDataTerminalItem("", r[0].Split(barCodeDelimiter.ToCharArray()), r[1], r[2], r[5], r[6], r[7], r[7], r[8], "");
                }
                else
                {
                    throw new RestoException(string.Format(Resources.WrongCsvInventoryFormatException,
                        headersWithCodeCellPallet.Join(", "), headersWithCellPallet.Join(", ")));                    
                }
            }
        }

        /// <summary>
        /// Записывает данные в специальный csv файл (инвентаризация, приходная накладная и т.п.), 
        /// для работы с ним на ТСД.
        /// </summary>
        /// <param name="path">Путь для создания файла.</param>
        /// <param name="records">Данные для выгрузки в файл.</param>
        /// <param name="useHeaders">True, тогда используется стандартный шаблон хедеров, иначе хедеры не добавляются.</param>
        public void WriteToDocumentFile(string path, IEnumerable<IEnumerable<string>> records, bool useHeaders)
        {
            stream.WriteToFile(path, useHeaders ? headersWithCellPallet : new List<string>(), UpdateRecords(records));
        }

        /// <summary>
        /// Записывает данные в специальный csv файл номенклатуры, для работы с ним на ТСД.
        /// </summary>
        /// <param name="path">Путь для создания файла.</param>
        /// <param name="records"></param>
        public void WriteToNomenclature(string path, IEnumerable<IEnumerable<string>> records)
        {
            stream.WriteToFile(path, nomenclatureHeaders, UpdateRecords(records));
        }

        #endregion Public methods

        #region Private methods
        /// <summary>
        /// Обновляет данные для записи на ТСД, с учётом специфики создания csv данных.
        /// </summary>
        private IEnumerable<IEnumerable<string>> UpdateRecords(IEnumerable<IEnumerable<string>> records)
        {
            var updatedRecords = new List<List<string>>();

            foreach (var list in records)
            {
                var tempList = new List<string>();

                foreach (var s in list)
                {
                    // символ " (двойные ковычки) преобразуется в ' (одинарную)
                    // поскольку ТСД формирует не верные данные для товаров, в именах которых встречаются двойные ковычки
                    // например ТСД выгружает - "РО-РО" жидкость для розжига, а нужно """РО-РО"" жидкость для розжига"
                    string str = s.Replace(quote, "\'");

                    if (str.ToCharArray().Any(c => reservedChars.Contains(c)))
                    {
                        // данные содержат зарезервированные символы.
                        tempList.Add(quote + str + quote);
                    }
                    else
                    {
                        tempList.Add(str);
                    }
                }
                updatedRecords.Add(tempList);
            }
            return updatedRecords.ToArray();
        }
        #endregion Private methods
    }

    #endregion CsvDataTerminalStream
}