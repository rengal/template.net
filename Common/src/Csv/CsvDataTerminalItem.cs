using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common.Csv
{

    #region CsvDataTerminalItem

    /// <summary>
    /// Класс описывает одну позицию csv файла для ТСД в виде конкретной информации.
    /// </summary>
    public class CsvDataTerminalItem : IComparable<CsvDataTerminalItem>
    {
        private static readonly NumberFormatInfo CsvNumberFormatInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = "."
            };
        #region Constructors

        public CsvDataTerminalItem()
        {
            BarCodes = new List<string>();
        }

        /// <summary>
        /// Создаёт объект.
        /// </summary>
        /// <param name="barCodes">Штрихкоды.</param>
        /// <param name="id">Уникальный идентификатор продукта.</param>
        /// <param name="num">Артикул.</param>
        /// <param name="name">Имя.</param>
        /// <param name="measureUnitId">Идентификатор единицы измерения/фасовки</param>
        /// <param name="measureUnitName">Название единицы измерения/фасовки</param>
        /// <param name="quantityPolicyId">Идентификатор политик работы с количеством.
        /// Имеет смысл только для товаров, изверяющихся на вес (бутылок).</param>
        public CsvDataTerminalItem([NotNull] IEnumerable<string> barCodes, string id, [NotNull] string num, [NotNull] string name,
                                   [NotNull] string measureUnitName, string measureUnitId, string quantityPolicyId)
            : this(
                string.Empty, barCodes, id, num, name, Decimal.Zero.ToString(), Decimal.Zero.ToString(), measureUnitName,
                measureUnitId, quantityPolicyId)
        {
        }

        /// <summary>
        /// Создаёт объект.
        /// </summary>
        /// <param name="scanCode">Отсканированный штрихкод.</param>
        /// <param name="codes">Штрихкоды.</param>
        /// <param name="num">Артикул.</param>
        /// <param name="id">Уникальный идентификатор продукта.</param>
        /// <param name="name">Имя.</param>
        /// <param name="plan">Кол-во по плану.</param>
        /// <param name="fact">Кол-во по факту.</param>
        /// <param name="measureUnitId">Идентификатор единицы измерения/фасовки</param>
        /// <param name="measureUnitName">Название единицы измерения/фасовки</param>
        /// <param name="quantityPolicyId">Идентификатор политик работы с количеством.
        /// Имеет смысл только для товаров, изверяющихся на вес (бутылок).</param>
        /// <param name="quantityInContainer">Количество в фасовке</param>
        public CsvDataTerminalItem([NotNull] string scanCode, [NotNull] IEnumerable<string> codes, string id, [NotNull] string num,
                                   [NotNull] string name, [NotNull] string plan, [NotNull] string fact,
                                   [NotNull] string measureUnitName, string measureUnitId, string quantityPolicyId, decimal quantityInContainer = 1)
            : this()
        {
            ScanCode = scanCode;
            BarCodes = codes.Where(c => !string.IsNullOrEmpty(c)).ToList();
            Id = id ?? num;
            Num = num;
            Name = name;
            PlanAmount = plan;
            CurrentAmount = fact;
            MeasureUnitName = measureUnitName;
            MeasureUnitId = measureUnitId;
            QuantityPolicyId = quantityPolicyId;
            QuantityInContainer = quantityInContainer;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Отсканированный штрихкод.
        /// Для DeclaredItems имеет пустое значение.
        /// </summary>
        public string ScanCode { get; set; }

        /// <summary>
        /// Штрихкод(ы) по номенклатуре.
        /// </summary>
        public List<string> BarCodes { get; set; }

        /// <summary>
        /// Уникальный идентификатор продукта.
        /// Для продуктов имеющих несколько штрихкодов и учитываемых по весу,
        /// каждый штрихкод должен иметь уникальный идентифкатор.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Артикул.
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Имя товара.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Колв-о по плану.
        /// </summary>
        public string PlanAmount { get; set; }

        /// <summary>
        /// Кол-во по факту.
        /// </summary>
        public string CurrentAmount { get; set; }

        /// <summary>
        /// Название еденицы измерения.
        /// </summary>
        public string MeasureUnitName { get; set; }

        /// <summary>
        /// Уникальный идентифкатор еденицы измерения.
        /// Сейчас совпадает с названием еденицы измерения.
        /// </summary>
        public string MeasureUnitId { get; set; }

        /// <summary>
        /// Идентифкатор политики учета.
        /// </summary>
        public string QuantityPolicyId { get; set; }
        
        /// <summary>
        /// Количество в фасовке
        /// </summary>
        public decimal QuantityInContainer { get; set; }

        /// <summary>
        /// Возвращает в виде списка строк те данные, которые требуются для создания позиции номенклатуры в файле Номенклатура.csv.
        /// </summary>
        public List<string> ItemAsNomenclatureList
        {
            get
            {
                return new List<string>
                           {
                               BarCodes.Join(CsvProductConverter.BarCodeDelimiter),
                               Id,
                               Num,
                               Name,
                               PlanAmount,
                               MeasureUnitId,
                               MeasureUnitName,
                               QuantityPolicyId,
                               QuantityInContainer.ToString(CsvNumberFormatInfo) ,
                               string.IsNullOrWhiteSpace(Num) ? "1": Num
                           };
            }
        }

        /// <summary>
        /// Возвращает в виде списка строк те данные, которые требуются для создания позиции документа в csv файле.
        /// </summary>
        public List<string> ItemAsDocumentList
        {
            get
            {
                return new List<string>
                           {
                               ScanCode.IsNullOrEmpty() ? BarCodes.Join(CsvProductConverter.BarCodeDelimiter) : ScanCode,
                               Id,
                               Num,
                               Name,
                               "",
                               "",
                               PlanAmount,
                               CurrentAmount,
                               MeasureUnitId,
                               QuantityPolicyId,
                               MeasureUnitName
                           };
            }
        }

        #endregion Properties

        #region IComparable<CsvDataTerminalItem> Members

        public int CompareTo(CsvDataTerminalItem item)
        {
            if (item == null)
            {
                return -1;
            }

            if (ReferenceEquals(this, item))
            {
                return 0;
            }

            int result = Comparer<string>.Default.Compare(Name, item.Name);

            // Если имена одинаковые, сравниваем артикул и штрихкод.
            if (result == 0)
            {
                result = Comparer<string>.Default.Compare(Num, item.Num);

                if (result == 0)
                {
                    result = item.BarCodes.SequenceEqual(BarCodes) ? 0 : -1;
                }
            }

            return result;
        }

        #endregion
    }

    #endregion CsvDataTerminalItem
}