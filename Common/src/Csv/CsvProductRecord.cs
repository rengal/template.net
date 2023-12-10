using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Data;
using Resto.Framework.Common;

namespace Resto.Common.Csv
{
    #region ICsvProductRecord

    /// <summary>
    /// Описывает csv данные в объектах модели.
    /// </summary>
    public interface ICsvProductRecord
    {
        /// <summary>
        /// Продукт.
        /// </summary>
        Product CsvProduct { set; get; }

        /// <summary>
        /// Фасовка.
        /// </summary>
        Container CsvContainer { set; get; }

        /// <summary>
        /// Кол-во по плану.
        /// Содержит кол-во элементов фасовки или базовой единицы измерения в зависимости от контейнера.
        /// </summary>
        decimal CsvPlanAmount { set; get; }

        /// <summary>
        /// Кол-во по факту.
        /// Содержит кол-во элементов фасовки или базовой единицы измерения в зависимости от контейнера.
        /// </summary>
        decimal CsvCurrentAmount { set; get; }

        /// <summary>
        /// Если true - неизвестный товар.
        /// Элемент нераспознался на ТСД.
        /// </summary>
        bool IsUnknown { set; get; }

        bool IsBalanceProduct { get; }

        /// <summary>
        /// Попытка инициализировать данными CsvProductRecord объект.
        /// </summary>
        /// <returns>Возвращает true, если объект удачно инициализирован.</returns>
        bool TryInitCsvRecord(CsvDataTerminalItem csvDataTerminalItem);
    }

    #endregion ICsvProductRecord

    #region CsvProductRecord

    /// <summary>
    /// Описывает csv данные в объектах модели.
    /// Преобразует csv данные в объект модели предназначенный для работы с документами.
    /// </summary>
    public class CsvProductRecord : ICsvProductRecord
    {
        #region Properties

        /// <summary>
        /// Продукт.
        /// </summary>
        public Product CsvProduct { get; set; }

        /// <summary>
        /// Фасовка.
        /// </summary>
        public Container CsvContainer { get; set; }

        /// <summary>
        /// Кол-во по плану.
        /// </summary>
        public decimal CsvPlanAmount { get; set; }

        /// <summary>
        /// Кол-во по факту.
        /// </summary>
        public decimal CsvCurrentAmount { get; set; }

        /// <summary>
        /// Если true - неизвестный товар.
        /// Элемент не распознался на ТСД, но есть в iiko.
        /// </summary>
        public bool IsUnknown { get; set; }

        /// <summary>
        /// Продукты, учитываемые по весу.
        /// </summary>
        public bool IsBalanceProduct { get; set; }

        #endregion Properties

        #region Public Methods

        public virtual bool TryInitCsvRecord(CsvDataTerminalItem csvDataTerminalItem)
        {
            Product prod = null;
            Container cont = null;

            IsUnknown = csvDataTerminalItem.CompareTo(CsvProductConverter.UnknownProduct) == 0;

            // поиск товара по сканированному штрихкоду
            if (!string.IsNullOrEmpty(csvDataTerminalItem.ScanCode))
            {
                var pair = GetProductContainer(new List<string> { csvDataTerminalItem.ScanCode });

                if (pair.HasValue)
                {
                    prod = pair.Value.First;
                    cont = pair.Value.Second;
                    if (prod.UseBalanceForInventory
                        && csvDataTerminalItem.MeasureUnitId == MeasureUnit.DefaultKgUnit.NameLocal)
                    {
                        IsBalanceProduct = true;
                    }
                }
            }

            // поиск товара по штрихкодам
            if (prod == null && csvDataTerminalItem.BarCodes.Any())
            {
                var pair = GetProductContainer(csvDataTerminalItem.BarCodes);

                if (pair.HasValue)
                {
                    prod = pair.Value.First;
                    cont = pair.Value.Second;
                }
            }

            // поиск товара по артикулу
            if (prod == null)
            {
                prod = Product.GetProductByNum(csvDataTerminalItem.Num);
            }

            if (prod != null /*&& prod.Num == pin*/)
            {
                CsvProduct = prod;
                if (cont == null || cont.IsEmptyContainer)
                {
                    CsvContainer = Container.GetEmptyContainer();
                }
                else
                {
                    CsvContainer = cont;
                }

                decimal current;
                CsvCurrentAmount = Decimal.TryParse(csvDataTerminalItem.CurrentAmount, out current) ? current : 0;

                decimal plan;
                CsvPlanAmount = Decimal.TryParse(csvDataTerminalItem.PlanAmount, out plan) ? plan : 0;
            }

            return prod != null;
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Возвращает пару продукт-контейнер по штрихкодам.
        /// </summary>
        private static Pair<Product, Container>? GetProductContainer(IEnumerable<string> barCodes)
        {
            var list = barCodes.Select(Product.GetProductContainerByBarcode).Where(p => p != null).ToList();

            //разные штрих коды должны ссылаться на одинаковый контейнер/фасовку
            return list.Select(i => i.Value.Second).Distinct().Count() == 1 ? list[0] : null;
        }

        #endregion Private Methods
    }

    #endregion CsvProductRecord
}
