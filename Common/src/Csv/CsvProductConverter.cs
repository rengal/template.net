using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Common.Properties;

namespace Resto.Common.Csv
{
    #region CsvProductConverter

    /// <summary>
    /// Абстрактный класс, выполняет конвертацию объектов в набор данных для записи в csv файл и наоборот.
    /// </summary>
    public abstract class CsvProductConverter
    {
        #region Data members

        /// <summary>
        /// Название файла куда выгружается номенклатура.
        /// </summary>
        private const string NomenclatureCsvFile = "Номенклатура.csv";

        /// <summary>
        /// Символ разделитель для записи нескольких штрихкодов в одной записи.
        /// </summary>
        public const string BarCodeDelimiter = "|";

        /// <summary>
        /// Символ разделитель данных для одной записи в  csv файле.
        /// </summary>
        protected const string CsvDelimiter = ";";

        /// <summary>
        /// Неизвестный товар. Требуется для возможности сканирования любых штрихкодов, даже которых
        /// нет в файле номенклатуры.
        /// </summary>
        public static CsvDataTerminalItem UnknownProduct = new CsvDataTerminalItem("00000000".AsSequence(), "1", "", Resources.UnknownProduct, "---", "---", "");

        /// <summary>
        /// Список записей для выгрузки на ТСД в файл Номенклатура.csv.
        /// </summary>
        protected IEnumerable<ICsvProductRecord> nomenclatureList;

        /// <summary>
        /// Список записей для выгрузки на ТСД в csv файл документа, если это необходимо.
        /// </summary>
        protected IEnumerable<ICsvProductRecord> documentList;

        /// <summary>
        /// Спиоск политик учета количества. Ключ - идентификатор политик, значение - список идентификаторов едениц измерения.
        /// </summary>
        protected Dictionary<int, List<string>> quantityPolicyInfo = new Dictionary<int, List<string>>();

        protected string fileNameDocument;

        #endregion Data members

        #region Properties

        protected abstract bool PreprocessItems { get; }

        /// <summary>
        /// Имя файла конкретного документа, куда будут выгружаться данные.
        /// Например требуется создавать файл Инвентаризации для выгрузки его на девайс.
        /// </summary>
        public string FileNameDocument
        {
            get { return fileNameDocument; }
        }

        /// <summary>
        /// Штрихкоды, которые не зарегистрированны с системе или идут после товара разделителя.
        /// Содержит информацию после загрузки данных.
        /// </summary>
        public List<CsvDataTerminalItem> NotSupportedItems
        {
            get;
            set;
        }

        /// <summary>
        /// Словарь возвращает данные по контейнарм, для которых не заданы штрихкоды.
        /// Содержит информацию после выгрузки данных.
        /// </summary>
        public Dictionary<Product, List<Container>> WrongContainers
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        protected CsvProductConverter()
        {
            // в файл Номенклатура.csv выгружается вся номенклатура, даже удалённых товаров
            nomenclatureList = Product.GetAllNativeProducts().Select(
                r => (ICsvProductRecord)new CsvProductRecord
                {
                    CsvProduct = r,
                    CsvContainer = Container.GetMeasureUnitContainer(r.MainUnit),
                    CsvPlanAmount = 0
                });
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Удаляет файл документа.
        /// </summary>
        /// <param name="path">Путь к папке где лежит файл документа.</param>
        public virtual void RemoveDocumentFile(string path)
        {
            var fullPath = Path.Combine(path, fileNameDocument);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// Преобразует список объектов для записи в файл номенклатуры.
        /// </summary>
        /// <param name="path">Путь к папке где лежит файл документа.</param>
        public int ToNomenclature(string path)
        {
            WrongContainers = new Dictionary<Product, List<Container>>();
            var csvList = new List<CsvDataTerminalItem> { UnknownProduct };
            List<Product> products = GetNomenclatureInfo(nomenclatureList).ToList();

            int quantityPolicyId = 0;
            int productId = 2;

            foreach (var p in products)
            {
                if (PreprocessItems && p.Deleted)
                    continue;
                // только для базовой еденицы измерения, возможно создавать элемент для выгрузки, без штрихкода
                Container mainUnitContainer = Container.GetMeasureUnitContainer(p.MainUnit);
                var item = CreateCsvItem(p, (++productId).ToString(), mainUnitContainer, true);
                if (item != null)
                {
                    if (PreprocessItems)
                    {
                        if (p.UseBalanceForSell)
                        {
                            item.BarCodes = (item.BarCodes ?? new List<string>());
                            item.BarCodes.Add(p.Num);
                        }
                    }
                    csvList.Add(item);
                }

                if (PreprocessItems && p.UseBalanceForSell)
                    continue;
                foreach (var c in p.Containers.Where(cont => !cont.Deleted))
                {
                    item = CreateCsvItem(p, (++productId).ToString(), c, false);
                    if (item != null)
                    {
                        item.QuantityInContainer = c.Count;
                        csvList.Add(item);
                        //для объектов, имеющих фасовки и выставленный флаг "Учитывать товар по весу" для каждой фасовки создается 
                        //вторая строка для учета в килогарммах. Работает только для инвентаризации.

                        CreateAdditionalMeasureUnitRecords(p, item, csvList, c, ref quantityPolicyId);
                    }
                    else
                    {
                        if (!WrongContainers.ContainsKey(p))
                        {
                            WrongContainers.Add(p, new List<Container>());
                        }
                        WrongContainers[p].Add(c);
                    }
                }
            }

            var helper = new CsvDataTerminalStream(CsvDelimiter, BarCodeDelimiter);
            helper.WriteToNomenclature(Path.Combine(path, NomenclatureCsvFile), csvList.Select(i => i.ItemAsNomenclatureList).ToArray());

            WriteQuantityPolicyFile(path);

            // возвращает кол-во выгруженных позиций, без учёта позиции по "неизвестному товару"
            return csvList.Count - 1;
        }

        /// <summary>
        /// Метод создает дополнительную запись для товаров, учитываемых по весу.
        /// </summary>
        /// <param name="p">Продукт</param>
        /// <param name="item">Базовая запись</param>
        /// <param name="csvList">Список запись</param>
        /// <param name="c">Контейнер, для которого создается запись</param>
        /// <param name="quantityPolicyId">Идентификатор политики учета для записи</param>
        private void CreateAdditionalMeasureUnitRecords(Product p, CsvDataTerminalItem item,
                                                                   List<CsvDataTerminalItem> csvList, Container c,
                                                                   ref int quantityPolicyId)
        {
            if (p.UseBalanceForInventory)
            {
                quantityPolicyInfo[++quantityPolicyId] = new List<string>
                                                             {
                                                                 c.Name,
                                                                 MeasureUnit.DefaultKgUnit.NameLocal
                                                             };
                item.QuantityPolicyId = quantityPolicyId.ToString();
                item = CreateCsvItem(p, item.Id, p.GetBarCodesByContainer(c), false, MeasureUnit.DefaultKgUnit.NameLocal, quantityPolicyId);
                if (item != null)
                {
                    csvList.Add(item);
                }
            }
        }

        /// <summary>
        /// Запись в файл списка политик учета фасовок.
        /// </summary>
        /// <param name="path">Путь для записи файлов</param>
        private void WriteQuantityPolicyFile(string path)
        {
            using (var xmlWriter = new XmlTextWriter(Path.Combine(path, "qty_pol.xml"), Encoding.UTF8))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("QuantityPolicyCollection");
                foreach (var item in quantityPolicyInfo)
                {
                    xmlWriter.WriteStartElement("QuantityPolicy");
                    xmlWriter.WriteAttributeString("id", item.Key.ToString());
                    xmlWriter.WriteAttributeString("multiline", "True");
                    xmlWriter.WriteStartElement("PackingIds");
                    foreach (var id in item.Value)
                    {
                        xmlWriter.WriteElementString("String", id);
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }
        /// <summary>
        /// Создаёт элемент для выгрузки в csv файл.
        /// </summary>
        /// <param name="p">Продукт.</param>
        /// <param name="container">Контейнер.</param>
        /// <param name="useNum">Если true, есть возможность создавать элемент на основе артикула, без учёта штрих кода.</param>
        private static CsvDataTerminalItem CreateCsvItem(Product p, string productId, Container container, bool useNum)
        {
            List<string> codes = p.GetBarCodesByContainer(container);
            return CreateCsvItem(p, productId, codes, useNum, container.Name, 0);
        }

        /// <summary>
        /// Создаёт элемент для выгрузки в csv файл.
        /// </summary>
        /// <param name="p">Продукт.</param>
        /// <param name="codes">Штрихкоды</param>
        /// <param name="useNum">Если true, есть возможность создавать элемент на основе артикула, без учёта штрих кода.</param>
        /// <param name="unitName">Название/идентифкатор единицы измерения/фасовки</param>
        /// <param name="quantityPolicy">Идентифкатор политики фасовки</param>
        protected static CsvDataTerminalItem CreateCsvItem(Product p, string productId, IEnumerable<string> codes, bool useNum, string unitName, int quantityPolicy)
        {
            CsvDataTerminalItem csvItem = null;

            string quantityPolicyId = quantityPolicy != 0 ? quantityPolicy.ToString() : "";
            if (codes.Any())
            {
                // создается позиция со штрихкодами
                csvItem = new CsvDataTerminalItem(codes, productId, p.Num, p.NameLocal, unitName, unitName, quantityPolicyId);
            }
            else if (!string.IsNullOrEmpty(p.Num) && useNum)
            {
                // создаётся позиция с пустым штрихкодом, в качестве основного элемента выступает артикул
                csvItem = new CsvDataTerminalItem(string.Empty.AsSequence(), productId, p.Num, p.NameLocal, unitName, unitName, quantityPolicyId);
            }
            return csvItem;
        }

        /// <summary>
        /// Преобразует список объектов для записи в файл.
        /// </summary>
        /// <param name="path">Путь к папке где лежит файл документа.</param>
        /// <param name="blank">Если true, создавать пустой файл, только с хедерами</param>
        public virtual void ToCsv(string path, bool blank)
        {
            // Использовался для тестирования, чтобы не создавать csv файлы руками.
            // Может пригодиться в будущем если им потребуется создавать не пустой файл документа в котором можно посмотреть кол-во товара по плану.

            var records = new List<CsvDataTerminalItem>();

            if (!blank)
            {
                records = GetDocumentInfo(documentList.ToList());
            }
            var helper = new CsvDataTerminalStream(CsvDelimiter, BarCodeDelimiter);
            helper.WriteToDocumentFile(Path.Combine(path, fileNameDocument), records.Select(i => i.ItemAsDocumentList).ToArray(), true);
        }

        /// <summary>
        /// Преобразует данные из файла в набор объектов.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Первый список пары - набор преобразованных объектов, 
        /// второй список - данные которые не удалось преобразовать.</returns>
        public virtual IEnumerable<ICsvProductRecord> FromCsv(string path)
        {
            NotSupportedItems = new List<CsvDataTerminalItem>();
            var helper = new CsvDataTerminalStream(CsvDelimiter, BarCodeDelimiter);
            var list = new List<ICsvProductRecord>();

            foreach (var item in helper.ReadFromFile(path, true))
            {
                ICsvProductRecord record = CreateRecord();

                bool res = record.TryInitCsvRecord(item);

                if (res)
                {
                    // добавляем в список, если объект был удачно инициализирова (было найдено соответствие по 
                    // штрихкоду или артикулу)
                    list.Add(record);
                }
                else
                {
                    // элементы нераспознанные в iiko 
                    NotSupportedItems.Add(item);
                }
            }

            if (NotSupportedItems.Any())
            {
                // формирование файла, c данными, которые не удалось преобразовать
                var fullPath = string.Format("{0}\\${1}", Path.GetDirectoryName(path), Path.GetFileName(path));
                helper.WriteToDocumentFile(fullPath, NotSupportedItems.Select(i => i.ItemAsDocumentList).ToArray(), false);
            }

            return list;
        }

        /// <summary>
        /// Получает из списка объектов данные о документе.
        /// </summary>
        /// <param name="list">Список объектов.</param>
        private static List<CsvDataTerminalItem> GetDocumentInfo(IEnumerable<ICsvProductRecord> list)
        {
            var records = new List<CsvDataTerminalItem>();

            foreach (var record in list.OrderBy(r => r.CsvProduct))
            {
                List<string> barCodes = record.CsvProduct.GetBarCodesByContainer(record.CsvContainer).Where(s => !s.IsNullOrEmpty()).ToList();
                string amount = record.CsvPlanAmount.ToString();

                var rec = new CsvDataTerminalItem(string.Empty, barCodes, "", record.CsvProduct.Num, record.CsvProduct.NameLocal, amount, Decimal.Zero.ToString(), string.Empty, "", "");
                records.Add(rec);
            }

            return records;
        }

        /// <summary>
        /// Получает из списка объектов данные о номенклатуре.
        /// </summary>
        /// <param name="list">Список объектов.</param>
        private static IEnumerable<Product> GetNomenclatureInfo(IEnumerable<ICsvProductRecord> list)
        {
            return list.Cast<CsvProductRecord>().Select(r => r.CsvProduct).Distinct();
        }

        /// <summary>
        /// Возвращает конкретный объект, который будет наполняться данными.
        /// </summary>
        protected abstract ICsvProductRecord CreateRecord();

        #endregion Methods
    }

    #endregion CsvProductConverter
}
