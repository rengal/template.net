using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Csv;
using Resto.Common.Extensions;
using Resto.Common.Interfaces;
using Resto.Data;
using Resto.Framework.Common.Currency;
using Container = Resto.Data.Container;

namespace Resto.Common.Models
{
    public sealed class MeasureUnitWrapper
    {
        #region Properties

        [CanBeNull]
        public MeasureUnit MeasureUnit { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return MeasureUnit == null ? "" : MeasureUnit.NameLocal;
        }

        #endregion
    }

    public sealed class ProductGroupWrapper : IComparable
    {
        #region Properties

        public ProductGroup ProductGroup { get; set; }

        #endregion

        #region Methods

        public int CompareTo(object obj)
        {
            return ProductGroup == null
                ? (((ProductGroupWrapper)obj).ProductGroup == null ? 0 : -1)
                : ProductGroup.NameLocal.CompareTo(((ProductGroupWrapper)obj).ProductGroup != null
                    ? ((ProductGroupWrapper)obj).ProductGroup.NameLocal
                    : "");
        }

        public override string ToString()
        {
            return ProductGroup != null ? ProductGroup.NameLocal : "";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as ProductGroupWrapper;
            if (other == null)
            {
                return false;
            }
            return Equals(ProductGroup, other.ProductGroup);
        }

        public override int GetHashCode()
        {
            return ProductGroup != null ? ProductGroup.GetHashCode() : 0;
        }

        #endregion
    }

    /// <summary>
    /// Запись инвентаризации
    /// </summary>
    public sealed class InventoryFormRecord : CsvProductRecord, IRestoGridRow, INotifyPropertyChanged, IDocumentRecordWithProductSize
    {
        #region Constants

        public const int AmountSignificantDigits = 3;

        public const int SumSignificantDigits = 3;

        #endregion

        #region Fields

        private BarcodeCompletionData barcode;

        private decimal? bookCount;

        private Container container;

        private decimal? containerCount;

        private EvaluableDecimalValue costPrice;

        private decimal? currentActualAmount;

        private decimal? currentAmountWithAdditions;

        private decimal? currentAmountWithPackage;

        private decimal? firstStepAdditionCount;

        private EvaluableDecimalValue fullAdditionCostPrice;

        private MeasureUnit measureUnit;

        private Product product;

        private ProductGroupWrapper productGroup;

        private DateTime? recalculationDate;

        private InventoryItemStatus status = InventoryItemStatus.NEW;

        private decimal? turnover;

        private ProductNumCompletionData productNum;

        [CanBeNull]
        private ProductSize productSize;

        #endregion

        #region Constructors

        public InventoryFormRecord()
        {
            Info = string.Empty;
        }

        public InventoryFormRecord(int num)
            : this()
        {
            Num = num;
        }

        public InventoryFormRecord([NotNull] AbstractIncomingInventoryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Unit != null)
            {
                ItemMeasureUnit = new MeasureUnitWrapper { MeasureUnit = item.Unit };
            }
            Product = item.Product;
            ProductSize = item.ProductSize;
            Info = item.Comment;
            Num = item.Num;
            Producer = item.Producer;

            // Ищем фасовку по идентификатору.
            Container = item.Product.Containers.FirstOrDefault(c => c.Id == item.ContainerId.GetValueOrFakeDefault())
                        // Если не нашли, начинаем искать по имени.
                        ?? item.Product.Containers.FirstOrDefault(c => c.Name == item.ContainerName)
                        // Снова не нашли - используем пустой контейнер.
                        ?? Container.GetEmptyContainer();

            ContainerCount = item.ContainerCount.GetValueOrFakeDefault();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Запись можно посчитать или пересчитать.
        /// </summary>
        public bool IsCalculableRecord
        {
            get { return Action == InventoryItemStatus.NEW || Action == InventoryItemStatus.NEW_FOR_RECALC; }
        }

        /// <summary>
        /// Запись сохранена.
        /// </summary>
        public bool IsSaved
        {
            get { return Action == InventoryItemStatus.SAVE; }
        }

        /// <summary>
        /// Запись пересохранена (удалена).
        /// </summary>
        public bool IsRecalculableRecord
        {
            get { return Action == InventoryItemStatus.RECALC; }
        }

        /// <summary>
        /// Оборот/движение продукта.
        /// </summary>
        public decimal? NullableTurnover
        {
            get { return turnover; }
            set { turnover = value; }
        }

        public decimal Turnover
        {
            get { return NullableTurnover.GetValueOrDefault(); }
            set { turnover = value; }
        }

        /// <summary>
        /// Полное кол-во с учётом движения.
        /// </summary>
        public decimal FullAmount
        {
            get { return CurrentAmountWithAdditions - Turnover; }
        }

        /// <summary>
        /// Время учёта движения.
        /// </summary>
        public DateTime? RecalculationDate
        {
            get { return recalculationDate; }
            set { recalculationDate = value; }
        }

        /// <summary>
        /// Сигнализиурет об изменении состояния элемента: удалён, изменён, добавлен
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product
        {
            get { return product; }
            set
            {
                if (Equals(product, value))
                {
                    return;
                }
                product = value;
                this.SetDefaultProductSize();

                if (product == null)
                {
                    productGroup = null;
                    ItemMeasureUnit = null;
                    firstStepAdditionCount = 0;
                }
                else
                {
                    productGroup = new ProductGroupWrapper { ProductGroup = value.Parent };
                    if (ItemMeasureUnit == null)
                    {
                        ItemMeasureUnit = new MeasureUnitWrapper { MeasureUnit = value.MainUnit };
                    }
                }
                UpdateBarcode();
                RaisePropertyChangedIfNotNull("Product");
            }
        }

        /// <summary>
        /// Размер продукта
        /// </summary>
        [CanBeNull]
        public ProductSize ProductSize
        {
            get => productSize;
            set
            {
                if (Equals(productSize, value))
                {
                    return;
                }

                productSize = value;
                RaisePropertyChangedIfNotNull(nameof(ProductSize));
            }
        }

        #region IDocumentRecordWithProductSize

        public decimal Amount { get; }

        public decimal AmountFactor { get; set; }

        #endregion IDocumentRecordWithProductSize

        /// <summary>
        /// Артикул товара.
        /// </summary>
        public ProductNumCompletionData ProductCode
        {
            get { return productNum = ProductNumCompletionData.SameOrNew(productNum, product); }
            set { Product = value != null ? value.Product : null; }
        }

        /// <summary>
        /// Штрихкод
        /// </summary>
        /// <remarks><c>UsedImplicitly</c>Используется для привязки столбца таблицы</remarks>
        [UsedImplicitly]
        public BarcodeCompletionData Barcode
        {
            get { return barcode; }
            set
            {
                barcode = value;
                if (value != null)
                {
                    Product = value.Product;
                    Container = Product.GetContainerById(value.BarcodeContainer.GetContainerIdOrDefault());
                }
            }
        }

        /// <summary>
        /// Фасовка.
        /// </summary>
        public Container Container
        {
            get { return container ?? (container = Container.GetEmptyContainer()); }
            set
            {
                if (Equals(container, value))
                {
                    return;
                }
                container = value ?? Container.GetEmptyContainer();
                if (container.IsEmptyContainer)
                {
                    NullableCurrentAmountWithPackage = null;
                    NullableContainerCount = null;
                }
                UpdateBarcode();
            }
        }

        public decimal CurrentAmountWithPackage
        {
            get { return NullableCurrentAmountWithPackage ?? new decimal(); }
            set { NullableCurrentAmountWithPackage = value; }
        }

        /// <summary>
        /// Фактическое количество с тарой (вес с тарой).
        /// </summary>
        public decimal? NullableCurrentAmountWithPackage
        {
            get { return currentAmountWithPackage; }
            set
            {
                if (currentAmountWithPackage == value)
                {
                    return;
                }

                currentAmountWithPackage = value;
                if (currentAmountWithPackage == null)
                {
                    return;
                }

                // Можем указать только что-то одно - либо вес с тарой, либо количество тары.
                NullableContainerCount = null;
                // Производим нормализацию общего веса (не может быть меньше веса фасовки)
                if (CurrentAmountWithPackage < Container.ContainerWeight)
                {
                    CurrentAmountWithPackage = Container.ContainerWeight;
                }

                if (Product != null && Product.Containers.Count > 0)
                {
                    if (Container == null || Container.Id == Guid.Empty)
                    {
                        Container = Product.Containers.FirstOrDefault(productContainer => !productContainer.Deleted)
                            ?? Container.GetEmptyContainer();
                    }
                }

                if (!Container.IsEmptyContainer)
                {
                    CurrentActualAmount = Container.GetValueByWeigth(CurrentAmountWithPackage);
                    CurrentAmountWithAdditions = FirstStepAdditionCount + CurrentActualAmount;
                }
                else
                {
                    CurrentActualAmount = 0;
                    CurrentAmountWithAdditions = FirstStepAdditionCount;
                }
            }
        }

        public bool Merged { get; set; }

        public int Num { get; set; }

        public int RecalculationNumber { get; set; }

        public int CountState { get; set; }

        public bool IsDisassembled { get; set; }

        [UsedImplicitly]
        public ProductGroupWrapper ProductGroup
        {
            get { return productGroup; }
            set { productGroup = value; }
        }

        public decimal FirstStepAdditionCount
        {
            get { return NullableFirstStepAdditionCount ?? new decimal(); }
            set { firstStepAdditionCount = value; }
        }

        public decimal? NullableFirstStepAdditionCount
        {
            get { return RoundNullableDecimal(firstStepAdditionCount, AmountSignificantDigits); }
            set { firstStepAdditionCount = value; }
        }

        public string Info { get; set; }

        public EvaluableDecimalValue CostPrice
        {
            get
            {
                // Если установлена сумма коррекции из проводок
                if (fullAdditionCostPrice != null && fullAdditionCostPrice.Value.GetValueOrFakeDefault() != 0 && FullAdditionCount != 0m)
                {
                    return fullAdditionCostPrice.Divide(FullAdditionCount, SumSignificantDigits);
                }

                // если сумма не равна null, но равна нулю, себестоимость (costPrice) не равна нулю 
                // и округлённое произведение FullAdditionCount и costPrice равно нулю
                // ex: FullAdditionCount = 0.001; costPrice = 3.33; округление сумм транзакций 
                // до 2-го знака на сервере даёт fullAdditionCostPrice = Round(0.001 * 3.33, 2) = 0
                if (fullAdditionCostPrice != null && fullAdditionCostPrice.Value.GetValueOrFakeDefault() == 0 &&
                    costPrice != null && costPrice.Value.GetValueOrFakeDefault() != 0 &&
                    (FullAdditionCount * costPrice.Value.GetValueOrFakeDefault()).MoneyPrecisionMoneyRound() == 0)
                {
                    return costPrice;
                }

                return costPrice ?? EvaluableDecimalValue.EVAL_ZERO;
            }
            set { costPrice = value; }
        }

        /// <summary>
        /// Кол-во введённое пользователем.
        /// </summary>
        public decimal CurrentActualAmount
        {
            get { return NullableCurrentActualAmount ?? new decimal(); }
            set
            {
                currentActualAmount = value;
                CountState = 1;
                if (currentActualAmount > 0 && ContainerCount == 0)
                {
                    NullableContainerCount = null;
                }
            }
        }

        [UsedImplicitly]
        public decimal? NullableCurrentActualAmount
        {
            get { return RoundNullableDecimal(currentActualAmount, AmountSignificantDigits); }
            set
            {
                currentActualAmount = value;
                CountState = 1;
                RaisePropertyChangedIfNotNull("NullableCurrentActualAmount");
            }
        }

        /// <summary>
        /// Количество тары.
        /// </summary>
        public decimal ContainerCount
        {
            get { return NullableContainerCount ?? new decimal(); }
            set { NullableContainerCount = value; }
        }

        public decimal? NullableContainerCount
        {
            get { return containerCount; }
            set { containerCount = value; }
        }

        public MeasureUnitWrapper ItemMeasureUnit { get; set; }

        /// <summary>
        /// Фактическое кол-во с учётом блюд и заготовок.
        /// </summary>
        public decimal CurrentAmountWithAdditions
        {
            get { return NullableCurrentAmountWithAdditions ?? new decimal(); }
            set { currentAmountWithAdditions = value; }
        }

        [UsedImplicitly]
        public decimal? NullableCurrentAmountWithAdditions
        {
            get { return RoundNullableDecimal(currentAmountWithAdditions, AmountSignificantDigits); }
            set { currentAmountWithAdditions = value; }
        }

        /// <summary>
        /// Книжное количество
        /// </summary>
        public decimal BookCount
        {
            get { return NullableBookCount ?? new decimal(); }
            set { bookCount = value; }
        }

        public decimal? NullableBookCount
        {
            get { return bookCount; }
        }

        /// <summary>
        /// Разница, количество.
        /// </summary>
        public decimal FullAdditionCount
        {
            get { return FullAmount - BookCount; }
        }

        /// <summary>
        /// Себестоимость разницы между фактическими и книжными остатками.
        /// </summary>
        /// <remarks>
        /// Для проведенного и неизмененного документа по информации сервера устанавливается значение fullAdditionCostPrice.
        /// В остальных случах это поле равно null и значение рассчитывается на основе произведения разницы и себестоимости.
        /// </remarks>
        public EvaluableDecimalValue FullAdditionCostPrice
        {
            get
            {
                // RMS-9280 Если разница меньше 1 грамма, не учитываем ее.
                // На сервере, при подготовке к выгрузке инвентаризации в 1С: resto.transfer.documents.ExternalInventory#initializeBy()
                if (FullAdditionCount.RoundAmountAsServer() == 0m)
                {
                    return EvaluableDecimalValue.ZERO;
                }

                return fullAdditionCostPrice
                       ?? CostPrice.Multiply(FullAdditionCount, SumSignificantDigits);
            }
            set { fullAdditionCostPrice = value; }
        }

        /// <summary>
        /// 0-новая
        /// 1-сохранённая
        /// 2-пересчитанная
        /// 3-новая для пересчета
        /// </summary>
        public InventoryItemStatus Action
        {
            get { return status; }
            set { status = value; }
        }

        [UsedImplicitly]
        public MeasureUnit MeasureUnit
        {
            get
            {
                if (ItemMeasureUnit != null && measureUnit == null)
                {
                    measureUnit = ItemMeasureUnit.MeasureUnit;
                }
                return measureUnit;
            }
            set
            {
                if (value != null)
                {
                    ItemMeasureUnit = new MeasureUnitWrapper { MeasureUnit = value };
                }
                measureUnit = value;
            }
        }

        /// <summary>
        /// Возвращает признак, содержит ли запись данные о коррекции из соответсвующей проводки.
        /// </summary>
        public bool HasCorrectionDataFromTransaction
        {
            get { return fullAdditionCostPrice != null; }
        }

        public EvaluableDecimalValue SumBefore { get; set; }

        public EvaluableDecimalValue SumAfter { get; set; }

        public bool IsEmptyRecord
        {
            get { return product == null; }
        }

        [CanBeNull]
        public User Producer { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public override bool TryInitCsvRecord(CsvDataTerminalItem item)
        {
            bool res = base.TryInitCsvRecord(item);
            if (res)
            {
                Product = CsvProduct;
                Container = CsvContainer;

                if (Container.IsEmptyContainer)
                {
                    currentActualAmount = CsvCurrentAmount;
                }
                else if (IsBalanceProduct)
                {
                    currentAmountWithPackage = CsvCurrentAmount;
                }
                else
                {
                    containerCount = CsvCurrentAmount;
                }
                status = InventoryItemStatus.NEW;
                CountState = 1;
            }
            return res;
        }

        [NotNull]
        public InventoryFormRecord MakeCopy()
        {
            return new InventoryFormRecord(Num)
            {
                Product = Product,
                CostPrice = CostPrice,
                NullableCurrentActualAmount = NullableCurrentActualAmount,
                NullableCurrentAmountWithAdditions = NullableCurrentAmountWithAdditions,
                ItemMeasureUnit =
                    ItemMeasureUnit != null
                        ? new MeasureUnitWrapper { MeasureUnit = ItemMeasureUnit.MeasureUnit }
                        : null,
                RecalculationNumber = RecalculationNumber,
                bookCount = NullableBookCount,
                status = Action,
                Info = Info,
                firstStepAdditionCount = NullableFirstStepAdditionCount,
                container = Container,
                containerCount = NullableContainerCount,
                currentAmountWithPackage = NullableCurrentAmountWithPackage,
                fullAdditionCostPrice = fullAdditionCostPrice,
                IsDisassembled = IsDisassembled,
                Merged = Merged,
                CountState = CountState,
                turnover = Turnover,
                recalculationDate = RecalculationDate,
                SumBefore = SumBefore,
                SumAfter = SumAfter
            };
        }

        public static decimal? RoundNullableDecimal(decimal? value, int fractionDigits)
        {
            return value.HasValue
                ? (decimal?)Math.Round(value.Value, fractionDigits, MidpointRounding.AwayFromZero)
                : null;
        }

        [Pure]
        public static bool CanMerge([NotNull] InventoryFormRecord record1, [NotNull] InventoryFormRecord record2)
        {
            return record1.Product != null &&
                   Equals(record1.Product, record2.Product) &&
                   record1.Action == record2.Action &&
                   record1.RecalculationNumber == record2.RecalculationNumber;
        }

        /// <summary>
        /// Очищает информацию о данных, которые нужны при учёте движения.
        /// </summary>
        public void ClearTurnoverData()
        {
            turnover = null;
            recalculationDate = null;
        }

        public bool HasDifference(Func<bool> predicate)
        {
            if (predicate != null && predicate())
            {
                return HasCorrectionDataFromTransaction;
            }
            return FullAdditionCount.RoundAmountAsServer() != 0;
        }

        private void UpdateBarcode()
        {
            if (product == null || product.Barcodes == null || container == null)
            {
                Barcode = null;
                return;
            }

            var barcodeContainer = product.Barcodes
                .Select(barcodes => barcodes.Value)
                .FirstOrDefault(x => x.GetContainerIdOrDefault() == container.Id);
            if (barcodeContainer == null)
            {
                Barcode = null;
                return;
            }

            Barcode = new BarcodeCompletionData
            {
                Product = product,
                BarcodeContainer = barcodeContainer
            };
        }

        public override string ToString()
        {
            return String.Format("{0}   {1:0.000}{2:+0.000;-0.000}  {3:+0.00;-0.00}",
                Convert.ToString(Product), CurrentActualAmount, FullAdditionCount,
                Convert.ToDecimal(EvaluableDecimalValue.GetNullableDecimal(FullAdditionCostPrice))
                );
        }

        private void RaisePropertyChangedIfNotNull(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}