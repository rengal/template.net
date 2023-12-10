using System.ComponentModel;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Interfaces;
using Resto.Data;
using Resto.Framework.Common.Currency;
using Container = Resto.Data.Container;

namespace Resto.Common.Models
{
    /// <summary>
    /// Запись, отображаемая в гриде накладных.
    /// </summary>
    public class AbstractInvoiceRecord : IRecordProductStoreGetter, INotifyPropertyChanged, IRecordWithCost<AbstractInvoiceItem>, IDocumentRecordWithProductSize
    {
        #region Fields

        protected decimal amountInContainer;

        protected decimal amountInMeasureUnit;

        protected decimal amountFactor;

        private BarcodeCompletionData barCode;

        protected bool changed;

        protected EvaluableDecimalValue costPrice = new EvaluableDecimalValue();

        protected decimal costPriceSummary;

        protected Product counteragentProduct;

        protected Product invoiceProduct;

        protected ProductSize productSize;

        protected decimal ndsPercent;

        protected decimal ndsSum;

        protected decimal price;

        /// <summary>
        /// Поле для <see cref="PriceWithoutVat"/>
        /// </summary>
        private decimal priceWithoutVat;

        protected Container productContainer = Container.GetEmptyContainer();

        protected decimal recordDiscount;

        protected Store recordStore;

        protected decimal recordSum;

        protected decimal remainsAfter;

        protected decimal remainsBefore;

        protected int rowNumber;

        private bool shouldRaiseOnChange = true;

        protected decimal sumWithoutNDS;

        protected User producer;

        private ProductNumCompletionData invoiceProductNum;

        private decimal incomePricePerUnit;

        private decimal incomePriceSum;

        private DocumentType currentDocumentType;
        [CanBeNull]
        private string customsDeclarationNumber;

        #endregion

        public AbstractInvoiceRecord()
        {
        }

        public AbstractInvoiceRecord(DocumentType currentDocumentType)
            : this()
        {
            this.currentDocumentType = currentDocumentType;
        }

        #region Properties

        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        /// <summary>
        /// Товар у контрагента
        /// </summary>
        public Product CounteragentProduct
        {
            get { return counteragentProduct; }
            set
            {
                counteragentProduct = value;
                RaisePropertyChangedIfNotNull("CounteragentProduct");
            }
        }

        /// <summary>
        /// Артикул товара.
        /// </summary>
        public ProductNumCompletionData InvoiceProductCode
        {
            get { return invoiceProductNum = ProductNumCompletionData.SameOrNew(invoiceProductNum, invoiceProduct); }
            set { InvoiceProduct = value != null ? value.Product : null; }
        }

        /// <summary>
        /// Штрихкод.
        /// </summary>
        public BarcodeCompletionData BarCode
        {
            get { return barCode; }
            set
            {
                if (Equals(barCode, value))
                {
                    return;
                }
                barCode = value;
                if (value != null)
                {
                    InvoiceProduct = value.Product;
                    ProductContainer = Product.GetContainerById(value.BarcodeContainer.GetContainerIdOrDefault());
                }
                else if (InvoiceProduct != null)
                {
                    // если штрихкод не задан, то тара является базовой ед. изм.
                    ProductContainer = Container.GetMeasureUnitContainer(invoiceProduct.MainUnit);
                    if (barCode == null)
                    {
                        UpdateBarcode();
                    }
                }
                RaisePropertyChangedIfNotNull("BarCode");
            }
        }

        /// <summary>
        /// Товар накладной
        /// </summary>
        public Product InvoiceProduct
        {
            get { return invoiceProduct; }
            set
            {
                if (Equals(invoiceProduct, value))
                {
                    return;
                }
                invoiceProduct = value;
                ProductContainer = Container.GetMeasureUnitContainer(invoiceProduct.MainUnit);
                RaisePropertyChangedIfNotNull("InvoiceProduct");
            }
        }

        /// <summary>
        /// Фасовка, в которой приходуется товар
        /// </summary>
        public Container ProductContainer
        {
            get { return productContainer; }
            set
            {
                if (Equals(productContainer, value))
                {
                    return;
                }
                productContainer = value;
                UpdateBarcode();
                RaisePropertyChangedIfNotNull("ProductContainer");
            }
        }

        /// <summary>
        /// Кол-во товара в его основной единице измерения
        /// </summary>
        public decimal AmountInMeasureUnit
        {
            get { return amountInMeasureUnit; }
            set
            {
                amountInMeasureUnit = value;
                RaisePropertyChangedIfNotNull("AmountInMeasureUnit");
            }
        }

        /// <summary>
        /// Кол-во товара в выбранной фасовке
        /// </summary>
        public decimal AmountInContainer
        {
            get { return amountInContainer; }
            set
            {
                amountInContainer = value;
                RaisePropertyChangedIfNotNull("AmountInContainer");
            }
        }

        /// <summary>
        /// Цена, по которой осуществляется приход или расход товара.
        /// Включает НДС.
        /// </summary>
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                RaisePropertyChangedIfNotNull("Price");
            }
        }

        /// <summary>
        /// Цена, по которой осуществляется приход или расход товара.
        /// Без НДС.
        /// </summary>
        public decimal PriceWithoutVat
        {
            get => priceWithoutVat;
            set
            {
                priceWithoutVat = value;
                RaisePropertyChangedIfNotNull("PriceWithoutVat");
            }
        }

        /// <summary>
        /// Сумма прихода или расхода товара (кол-во * цену)
        /// </summary>
        public decimal RecordSum
        {
            get => recordSum;
            set
            {
                recordSum = value;
                RaisePropertyChangedIfNotNull("RecordSum");
            }
        }

        /// <summary>
        /// Скидка
        /// </summary>
        public decimal RecordDiscount
        {
            get { return recordDiscount; }
            set
            {
                recordDiscount = value;
                RaisePropertyChangedIfNotNull("RecordDiscount");
            }
        }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        public decimal NDSPercent
        {
            get { return ndsPercent; }
            set
            {
                ndsPercent = value;
                RaisePropertyChangedIfNotNull("NDSPercent");
            }
        }

        /// <summary>
        /// Признак того, что гость платит НДС самостоятельно.
        /// По умолчанию false, т.е. НДС платит заведение общепита.
        /// </summary>
        public bool SplitVat { get; set; }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal NDSSum
        {
            get { return ndsSum; }
            set
            {
                ndsSum = value;
                RaisePropertyChangedIfNotNull("NDSSum");
            }
        }

        /// <summary>
        /// Сумма без НДС
        /// </summary>
        public decimal SumWithoutNDS
        {
            get { return sumWithoutNDS; }
            set
            {
                sumWithoutNDS = value;
                RaisePropertyChangedIfNotNull("SumWithoutNDS");
            }
        }

        /// <summary>
        /// Склад на который будет осуществляться приход/расход товара
        /// </summary>
        public Store RecordStore
        {
            get { return recordStore; }
            set
            {
                recordStore = value;
                RaisePropertyChangedIfNotNull("RecordStore");
            }
        }

        /// <summary>
        /// Остатки ДО прихода/расхода
        /// </summary>
        public decimal RemainsBefore
        {
            get { return remainsBefore; }
            set
            {
                remainsBefore = value;
                RaisePropertyChangedIfNotNull("RemainsBefore");
            }
        }

        /// <summary>
        /// Остатки после прихода/расхода
        /// </summary>
        public decimal RemainsAfter
        {
            get { return remainsAfter; }
            set
            {
                remainsAfter = value;
                RaisePropertyChangedIfNotNull("RemainsAfter");
            }
        }

        /// <summary>
        /// Строка формата {Наименование валюты}/{Ед.изм продукта}
        /// </summary>
        public string MeasureUnitCurrencyString
        {
            get
            {
                return InvoiceProduct == null
                    ? string.Empty
                    : string.Format("{0}/{1}",
                        CurrencyHelper.GuiCurrencyName,
                        Container.GetName(ProductContainer, InvoiceProduct));
            }
        }

        /// <summary>
        /// Себестоимость товара
        /// </summary>
        public EvaluableDecimalValue CostPrice
        {
            get { return costPrice; }
            set
            {
                costPrice = value;
                RaisePropertyChangedIfNotNull("CostPrice");
            }
        }

        /// <summary>
        /// Сумма себестоимости товара (кол-во * себестоимость)
        /// </summary>
        public decimal CostPriceSummary
        {
            get { return costPriceSummary; }
            set
            {
                costPriceSummary = value;
                RaisePropertyChangedIfNotNull("CostPriceSummary");
            }
        }

        /// <summary>
        /// Признак того, что запись была изменена
        /// </summary>
        public bool Changed
        {
            get { return changed; }
            set { changed = value; }
        }

        /// <summary>
        /// Признак того, что строка является "пустой",
        /// т.е без продукта у нас и без продукта контрагента
        /// </summary>
        public bool IsEmptyRecord
        {
            get { return counteragentProduct == null && invoiceProduct == null; }
        }

        /// <summary>
        /// Пара Склад-Продукт записи, используется для 
        /// расчета себестоимости
        /// </summary>
        public StoreProductPair StoreProductPair
        {
            get
            {
                return InvoiceProduct != null && RecordStore != null
                    ? new StoreProductPair(RecordStore, InvoiceProduct)
                    : null;
            }
        }

        /// <summary>
        /// Производитель/Импортер
        /// </summary>
        [CanBeNull]
        public User Producer
        {
            get { return producer; }
            set
            {
                producer = value;
                RaisePropertyChangedIfNotNull("Producer");
            }
        }

        /// <summary>
        /// Цена прихода за ед. для возвратов
        /// </summary>
        public decimal IncomePricePerUnit
        {
            get { return incomePricePerUnit; }
            set
            {
                incomePricePerUnit = value;
                RaisePropertyChangedIfNotNull("IncomePricePerUnit");
            }
        }

        /// <summary>
        /// Сумма прихода для возвратов (кол-во * цену)
        /// </summary>
        public decimal IncomePriceSum
        {
            get { return incomePriceSum; }
            set
            {
                incomePriceSum = value;
                RaisePropertyChangedIfNotNull("IncomePriceSum");
            }
        }

        /// <summary>
        /// Номер таможенной декларации
        /// </summary>
        [CanBeNull]
        public string CustomsDeclarationNumber
        {
            get { return customsDeclarationNumber; }
            set
            {
                customsDeclarationNumber = value;
                RaisePropertyChangedIfNotNull(nameof(CustomsDeclarationNumber));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Обновляет штрихкод с учётом выбранного продукта и типа тары.
        /// </summary>
        private void UpdateBarcode()
        {
            if (invoiceProduct == null || invoiceProduct.Barcodes == null || productContainer == null)
            {
                BarCode = null;
                return;
            }

            var barcodeContainer = invoiceProduct.Barcodes
                .Select(barcodes => barcodes.Value)
                .FirstOrDefault(x => x.GetContainerIdOrDefault() == ProductContainer.Id);
            if (barcodeContainer == null)
            {
                BarCode = null;
                return;
            }

            BarCode = new BarcodeCompletionData
            {
                Product = invoiceProduct,
                BarcodeContainer = barcodeContainer
            };
        }

        #endregion Private Methods

        #region Implementation of IRecordWithCost<AbstractInvoiceItem>

        public Product Product
        {
            get { return invoiceProduct; }
        }

        public decimal Amount
        {
            get { return amountInMeasureUnit; }
        }

        public DocumentType CurrentDocumentType => Item == null ? currentDocumentType : Item.Invoice.DocumentType;

        public AbstractInvoiceItem Item { get; set; }


        public Store Store
        {
            get { return recordStore; }
        }

        #endregion

        # region Implementation of IDocumentRecordWithProductSize

        /// <summary>
        /// Размер блюда
        /// </summary>
        public ProductSize ProductSize
        {
            get { return productSize; }
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

        /// <summary>
        /// Коэффициент списания
        /// </summary>
        public decimal AmountFactor
        {
            get { return amountFactor; }
            set
            {
                amountFactor = value;
                RaisePropertyChangedIfNotNull(nameof(AmountFactor));
            }
        }

        #endregion

        #region IRecordProductStoreGetter Members
        /// <summary>
        /// Возвращает склад(используется для печати списка складов)
        /// </summary>
        public Store GetStore()
        {
            return RecordStore;
        }

        /// <summary>
        /// Возвращает товар (используется для печати списка складов)
        /// </summary>
        public Product GetProduct()
        {
            return InvoiceProduct;
        }

        public void CancelRaising()
        {
            shouldRaiseOnChange = false;
        }

        public void RestoreRaising()
        {
            shouldRaiseOnChange = true;
        }

        private void RaisePropertyChangedIfNotNull(string propertyName)
        {
            if (shouldRaiseOnChange && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}