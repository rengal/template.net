using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Common.Interfaces;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;

namespace Resto.Data
{
    public enum InvoiceEditMode
    {
        SimpleInput,

        RecognizeInput
    }

    public class InvoiceRecord : IRecordProductStoreGetter, IClipboardDataRecord, INotifyPropertyChanged, IDocumentRecordWithProductSize
    {
        #region Constants

        private const decimal DefaultMaxInPrice = 100000000m;

        #endregion

        #region Fields

        private readonly BindingList<InvoiceRecord> collection = new BindingList<InvoiceRecord>();

        private readonly InvoiceEditMode invoiceRecordEditMode;

        private readonly bool isRecordFormed = true;

        private readonly decimal maxInPrice = DefaultMaxInPrice;

        /// <summary>
        /// Счет
        /// </summary>
        private Account account;

        /// <summary>
        /// Плотность
        /// </summary>
        private decimal actualUnitWeight;

        /// <summary>
        /// Данные штрихкода продукта
        /// </summary>
        private BarcodeCompletionData barcode;

        /// <summary>
        /// Фасовка
        /// </summary>
        private Container container;

        /// <summary>
        /// Фактическое количество
        /// </summary>
        private decimal? actualAmount;

        /// <summary>
        /// Количество (в основных ед. измерения). Например, 2 кг
        /// </summary>
        private decimal inCount;

        /// <summary>
        /// Единица измерения (например, кг)
        /// </summary>
        private MeasureUnit inCountM;

        /// <summary>
        /// Стоимость
        /// </summary>
        /// <remarks>
        /// [Стоимость] = [цена] * [количество]
        /// </remarks>
        private decimal inPrice;

        private int nativeItemNumber = -1;

        /// <summary>
        /// Процент НДС
        /// </summary>
        private decimal nds;

        /// <summary>
        /// Сумма НДС
        /// </summary>
        private decimal ndsSumm;

        /// <summary>
        /// Количество (в единицах тары). Например, 2 ящика
        /// </summary>
        private decimal packageAmount;

        /// <summary>
        /// Цена за единицу тары (например: за "кг" или за "мешок 50кг").
        /// </summary>
        private decimal price;

        /// <summary>
        /// Поле для <see cref="PriceWithoutVat"/>.
        /// </summary>
        private decimal priceWithoutVat;

        /// <summary>
        /// Продукт
        /// </summary>
        private Product product;

        /// <summary>
        /// Размер блюда
        /// </summary>
        /// <remarks>
        /// Может задаваться только в документах списания для блюд, списываемых по ингредиентам.
        /// См. серверный класс resto.back.store.AbstractAssemblyWriteoffOutgoingItem.
        /// </remarks>
        private ProductSize productSize;

        /// <summary>
        /// Зафиксированный коэффициент списания блюда (модификатора).
        /// </summary>
        /// <remarks>
        /// Может задаваться только в документах списания для блюд, списываемых по ингредиентам.
        /// См. серверный класс resto.back.store.AbstractAssemblyWriteoffOutgoingItem.
        /// В остальных документах = 1, и не должен отображаться.
        /// </remarks>
        private decimal amountFactor = ProductSizeServerConstants.INSTANCE.DefaultAmountFactor;

        /// <summary>
        /// Разрешается пересчет по ценам поставщика
        /// </summary>
        private bool recalcEnabled = true;

        /// <summary>
        /// Признак фиксации пересчета суммы при изменении количества товара
        /// </summary>
        /// <remarks>
        /// Режим включается при ручном вводе поля 'Сумма'
        /// Режим выключается при вводе поля, пересчитывающего поле 'Сумма' (поле 'Сумма без' и поле 'Цена за ед. тары')
        /// </remarks>
        private bool inFixedPriceForRecalc;

        /// <summary>
        /// Если данные вставляются из буфера обмена.
        /// </summary>
        private bool isDataFromClipboard;

        /// <summary>
        /// Склад
        /// </summary>
        private Store store;

        /// <summary>
        /// Цена без НДС
        /// </summary>
        private decimal summWithoutNds;

        /// <summary>
        /// Продукт поставщика
        /// </summary>
        private Product supplierProduct;

        private User supplier;

        private ProductNumCompletionData productNum;

        /// <summary>
        /// Поле для <see cref="CurrentDocumentType"/>
        /// </summary>
        private DocumentType currentDocumentType;

        #endregion

        #region Constructors

        private InvoiceRecord()
        {
            IsDifferentMeasureUnit = true;
            IsDifferentPrice = true;
            IsDifferentAmount = true;
            CostPrice = EvaluableDecimalValue.ZERO;
            CostPriceSummary = EvaluableDecimalValue.ZERO;
        }

        public InvoiceRecord(AbstractInvoiceItem item, User supplier, BindingList<InvoiceRecord> collection) :
            this(item.Num, item, supplier, DefaultMaxInPrice, InvoiceEditMode.SimpleInput, collection)
        {
        }

        public InvoiceRecord(int newRowNum, AbstractInvoiceItem item, User supplier, Decimal maxInPrice, InvoiceEditMode invoiceRecordEditMode, BindingList<InvoiceRecord> collection)
            : this(newRowNum, supplier, maxInPrice, invoiceRecordEditMode, collection)
        {
            isRecordFormed = false;
            var incomingInvoiceItem = item as IncomingInvoiceItem;
            if (incomingInvoiceItem != null)
            {
                supplierProduct = incomingInvoiceItem.SupplierProduct;
                actualAmount = incomingInvoiceItem.ActualAmount.GetValueOrFakeDefault();
                CustomsDeclarationNumber = incomingInvoiceItem.CustomsDeclarationNumber;
            }

            if (item.Product != null)
            {
                product = item.Product;
                Container = Product.GetContainerById(item.ContainerId.GetValueOrFakeDefault());
                if (item.ActualUnitWeight.GetValueOrFakeDefault() == 0 && container != null)
                {
                    actualUnitWeight = GetActualUnitWeight();
                }
                else
                {
                    actualUnitWeight = item.ActualUnitWeight.GetValueOrFakeDefault();
                }
            }

            var writeoffOutgoingItem = item as ProductSizeDocumentItem;
            if (writeoffOutgoingItem != null)
            {
                productSize = writeoffOutgoingItem.ProductSize;
                amountFactor = writeoffOutgoingItem.AmountFactor;
            }

            if (item.Store != null)
            {
                store = item.Store;
            }

            inCount = item.Amount;

            if (item.AmountUnit != null)
            {
                inCountM = item.AmountUnit;
            }

            packageAmount = GetActualUnitWeight() != 0m
                ? (inCount / GetActualUnitWeight()).RoundAmountAsServer()
                : 0m;

            inPrice = item.SumWithoutDiscount;
            price = packageAmount == 0m ? item.PriceWithoutDiscount : (item.SumWithoutDiscount / packageAmount).MoneyPrecisionMoneyRound();

            nds = item.NdsPercent.GetValueOrDefault();
            summWithoutNds = item.SumWithoutNds.GetValueOrDefault();
            ndsSumm = item.SumWithoutDiscount - item.SumWithoutNds.GetValueOrDefault();
            priceWithoutVat = packageAmount == 0m ? item.PriceWithoutNds : (summWithoutNds / packageAmount).MoneyPrecisionMoneyRound();

            Discount = item.Discount;

            InvoiceItem = item;
            isRecordFormed = true;

            var outgoingInvoiceItem = item as OutgoingInvoiceItem;
            if (outgoingInvoiceItem != null)
            {
                Comment = outgoingInvoiceItem.Comment;
                actualAmount = outgoingInvoiceItem.ActualAmount;
            }

            Producer = item is IncomingInvoiceItem ? (item as IncomingInvoiceItem).Producer : null;
            SplitVat = item is WithSplitVat ? (item as WithSplitVat).SplitVat : false;
        }

        public InvoiceRecord(int newRowNum, User supplier, BindingList<InvoiceRecord> collection) :
            this(newRowNum, supplier, DefaultMaxInPrice, InvoiceEditMode.SimpleInput, collection)
        {
        }

        public InvoiceRecord(int newRowNum, User supplier, Decimal maxInPrice, InvoiceEditMode invoiceRecordEditMode, BindingList<InvoiceRecord> collection)
            : this()
        {
            Num = newRowNum;
            nativeItemNumber = newRowNum;
            this.supplier = supplier;
            this.maxInPrice = maxInPrice;
            this.invoiceRecordEditMode = invoiceRecordEditMode;
            this.collection = collection;
        }

        public InvoiceRecord(int newRowNum, User supplier, decimal maxInPrice, InvoiceEditMode invoiceRecordEditMode,
            BindingList<InvoiceRecord> collection, DocumentType currentDocumentType)
            : this(newRowNum, supplier, maxInPrice, invoiceRecordEditMode, collection)
        {
            this.currentDocumentType = currentDocumentType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Тип документа, содержащего запись
        /// </summary>
        public DocumentType CurrentDocumentType => InvoiceItem == null ? currentDocumentType : InvoiceItem.Invoice.DocumentType;

        /// <summary>
        /// Связаный элемент для проведенного документа.
        /// </summary>
        public AbstractInvoiceItem InvoiceItem { get; set; }

        /// <summary>
        /// Возвращает id позиции документа (InvoiceItem) с которым связана данная строка.
        /// </summary>
        public Guid? ItemId
        {
            get { return InvoiceItem != null ? InvoiceItem.Id : (Guid?)null; }
        }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal NDSSumm
        {
            get { return ndsSumm; }
            set
            {
                ndsSumm = value;
                RaisePropertyChangedIfNotNull();
                ForwardBackwardCalculate("NDSSumm");
            }
        }

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
                RaisePropertyChangedIfNotNull();
            }
        }

        /// <summary>
        /// Количество (в единицах тары). Например, 2 ящика
        /// </summary>
        public decimal PackageAmount
        {
            get { return packageAmount; }
            set
            {
                packageAmount = value;
                RaisePropertyChangedIfNotNull();
                if (Container != null)
                {
                    inCount = CalculateInCount(packageAmount);
                    ForwardBackwardCalculate("InCount");
                }
            }
        }

        /// <summary>
        /// Фактическое кол-во
        /// </summary>
        public decimal? ActualAmount
        {
            get { return actualAmount; }
            set
            {
                actualAmount = value;
                RaisePropertyChangedIfNotNull();
            }
        }

        /// <summary>
        /// Разница по кол-ву.
        /// </summary>
        public decimal DifferentAmount
        {
            get { return InCount - ActualAmount ?? 0; }
        }

        /// <summary>
        /// Сумма на разницу по кол-ву.
        /// </summary>
        public decimal DifferentSum
        {
            get { return (DifferentAmount * PriceWithNds).MoneyPrecisionMoneyRound(); }
        }

        /// <summary>
        /// Плотность
        /// </summary>
        public decimal ActualUnitWeight
        {
            get { return actualUnitWeight; }
            set
            {
                actualUnitWeight = value;
                RaisePropertyChangedIfNotNull();
                if (Container != null)
                {
                    inCount = (packageAmount * actualUnitWeight).RoundAmountAsServer();
                    ForwardBackwardCalculate("InCount");
                }
            }
        }

        /// <summary>
        /// Цена без НДС
        /// </summary>
        public decimal SummWithoutNDS
        {
            get { return summWithoutNds; }
            set
            {
                summWithoutNds = value;
                RaisePropertyChangedIfNotNull();
                ForwardBackwardCalculate("SummWithoutNDS");
            }
        }

        /// <summary>
        /// Дополнительные расходы
        /// </summary>
        public decimal AdditionalExpenses { get; set; }

        /// <summary>
        /// Сумма НДС для дополнительных расходов
        /// </summary>
        public decimal AdditionalExpensesNds { get; set; }

        /// <summary>
        /// Запись является доп. расходом
        /// </summary>
        public bool IsAdditionalExpense { get; set; }

        /// <summary>
        /// Запись не пустая и не является доп. расходом.
        /// </summary>
        public bool NotEmptyDefaultInvoiceRecord
        {
            get { return Product != null && !IsAdditionalExpense; }
        }

        /// <summary>
        /// Запись не содержит связанного с ней продукта 
        /// </summary>
        public bool IsEmptyRecord
        {
            get { return product == null && supplierProduct == null; }
        }

        /// <summary>
        /// Фасовка
        /// </summary>
        public Container Container
        {
            get
            {
                Contract.Ensures(IsEmptyRecord || Contract.Result<Container>() != null);

                if (container == null)
                {
                    AssignContainer();
                }
                return container;
            }
            set
            {
                if (Equals(container, value))
                {
                    return;
                }

                if (value != null)
                {
                    container = value;
                }
                else
                {
                    AssignContainer();
                }
                RaisePropertyChangedIfNotNull();
                if (container != null)
                {
                    actualUnitWeight = !container.IsEmptyContainer ? GetActualUnitWeight() : container.Count;
                    InCount = CalculateInCount(PackageAmount);
                }
                UpdateBarcode();
            }
        }

        /// <summary>
        /// Поставщик
        /// </summary>
        public User Supplier
        {
            get { return supplier; }
            set
            {
                User prevValue = supplier;
                supplier = value;
                if (invoiceRecordEditMode != InvoiceEditMode.RecognizeInput && prevValue != null && prevValue != value &&
                    supplierProduct != product)
                {
                    supplierProduct = null;
                }
            }
        }

        /// <summary>
        /// Продукт поставщика
        /// </summary>
        public Product SupplierProduct
        {
            get { return supplierProduct; }
            set { SetSupplierProductAndUpdatePrices(value, false, true); }
        }

        /// <summary>
        /// Порядковый номер записи (вспомогательное поле)
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// Артикул товара.
        /// </summary>
        public ProductNumCompletionData ProductNum
        {
            get { return productNum = ProductNumCompletionData.SameOrNew(productNum, product); }
            set { Product = value != null ? value.Product : null; }
        }

        /// <summary>
        /// Продукт
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

                Product previosProduct = product;
                product = value;
                RaisePropertyChangedIfNotNull();

                if (product == null)
                {
                    inCountM = null;
                    nds = 0;
                    actualUnitWeight = 0;
                }
                else
                {
                    if (invoiceRecordEditMode != InvoiceEditMode.RecognizeInput && recalcEnabled)
                    {
                        nds = value.VatPercent;
                    }

                    inCountM = value.MainUnit;

                    if (!Equals(previosProduct, value))
                    {
                        Container = Container.GetMeasureUnitContainer(value.MainUnit);
                    }

                    if (Supplier != null)
                    {
                        //Предотвращаем ситуацию, когда один и тот-же продукт у поставщика
                        //ссылается на разные продукты у нас
                        if (supplierProduct != null && !IsValidSupplierProduct(supplierProduct, product))
                        {
                            UpdateSupplierProductAndPrice(item => Container.Id == item.ContainerId, false, false, true);
                        }
                    }
                }

                UpdateBarcode();
            }
        }

        /// <summary>
        /// Размер блюда
        /// </summary>
        /// <remarks>
        /// Может задаваться только в документах списания для блюд, списываемых по ингредиентам.
        /// См. серверный класс resto.back.store.AbstractAssemblyWriteoffOutgoingItem.
        /// </remarks>
        public ProductSize ProductSize
        {
            get { return productSize; }
            set
            {
                productSize = value;
                RaisePropertyChangedIfNotNull();
            }
        }

        decimal IDocumentRecordWithProductSize.Amount
        {
            get { return InCount; }
        }

        /// <summary>
        /// Зафиксированный коэффициент списания блюда (модификатора).
        /// </summary>
        /// <remarks>
        /// Может задаваться только в документах списания для блюд, списываемых по ингредиентам.
        /// См. серверный класс resto.back.store.AbstractAssemblyWriteoffOutgoingItem.
        /// В остальных документах = 1, и не должен отображаться.
        /// </remarks>
        public decimal AmountFactor
        {
            get { return amountFactor; }
            set
            {
                amountFactor = value;
                RaisePropertyChangedIfNotNull();
            }
        }

        /// <summary>
        /// Склад
        /// </summary>
        public Store Store
        {
            get { return store; }
            set
            {
                store = value;
                RaisePropertyChangedIfNotNull();
            }
        }

        /// <summary>
        /// Счет
        /// </summary>
        public Account Account
        {
            get { return account; }
            set
            {
                account = value;
                RaisePropertyChangedIfNotNull();
            }
        }

        /// <summary>
        /// Учет НДС в цене за единицу тары.
        /// </summary>
        private VatAccounting VatAccounting => CompanySetup.Corporation.VatAccounting;

        /// <summary>
        /// Цена за единицу тары (например: за "кг" или за "мешок 50кг").
        /// Включает НДС.
        /// </summary>
        public decimal Price
        {
            get => price;
            set
            {
                price = Math.Abs(value);
                RaisePropertyChangedIfNotNull();
                ForwardBackwardCalculate("Price");
            }
        }

        /// <summary>
        /// Цена за единицу тары (например: за "кг" или за "мешок 50кг").
        /// Не включает НДС.
        /// </summary>
        public decimal PriceWithoutVat
        {
            get => priceWithoutVat;
            set
            {
                priceWithoutVat = Math.Abs(value);
                RaisePropertyChangedIfNotNull();
                ForwardBackwardCalculate("PriceWithoutVat");
            }
        }

        /// <summary>
        /// Получает цену c НДС за базовую единицу измерения.
        /// </summary>
        /// <remarks>Расчитывается как [стоимость C НДС] / [количество].</remarks>
        public decimal PriceWithNds
        {
            get { return InCount != 0 ? (InPrice / InCount).MoneyPrecisionMoneyRound() : 0; }
        }

        /// <summary>
        /// Получает цену без НДС за базовую единицу измерения.
        /// </summary>
        /// <remarks>Расчитывается как [стоимость без НДС] / [количество].</remarks>
        public decimal PriceWithoutNds
        {
            get { return InCount != 0 ? (SummWithoutNDS / InCount).MoneyPrecisionMoneyRound() : Price; }
        }

        /// <summary>
        /// Текстовое представление цена/ед.изм. Например: "руб/кг"
        /// </summary>
        public string PriceM
        {
            get
            {
                return String.Format("{0}/{1}",
                                     CurrencyHelper.GuiCurrencyName,
                                     // Если Container == null, то пустая строка...
                                     Convert.ToString(Container));
            }
        }

        public decimal ChangeBefore { get; set; }

        /// <summary>
        /// Количество (в основных ед. измерения). Например, 2 кг
        /// </summary>
        public decimal InCount
        {
            get { return inCount; }
            set
            {
                inCount = value;
                RaisePropertyChangedIfNotNull();
                if (Container != null && GetActualUnitWeight() != 0)
                {
                    packageAmount = (inCount / GetActualUnitWeight()).RoundAmountAsServer();
                }
                ForwardBackwardCalculate("InCount");
            }
        }

        /// <summary>
        /// Единица измерения (например, кг)
        /// </summary>
        public MeasureUnit InCountM
        {
            get { return inCountM ?? (Product != null ? Product.MainUnit : null); }
            set
            {
                inCountM = value;
                if (supplierProduct != null)
                {
                    supplierProduct.MainUnit = value;
                }
                ForwardBackwardCalculate("InCountM");
            }
        }

        /// <summary>
        /// Стоимость
        /// </summary>
        /// <remarks>
        /// [Стоимость] = [цена] * [количество]
        /// </remarks>
        public decimal InPrice
        {
            get { return inPrice; }
            set
            {
                inPrice = Math.Abs(value.MoneyPrecisionMoneyRound());
                RaisePropertyChangedIfNotNull();
                ForwardBackwardCalculate("InPrice");
            }
        }

        /// <summary>
        /// Количество товара (в осн. ед. измерения) после выполнения операции
        /// </summary>
        public decimal ChangeAfter { get; set; }

        /// <summary>
        /// Процент НДС
        /// </summary>
        public decimal Nds
        {
            get { return nds; }
            set
            {
                nds = value;
                RaisePropertyChangedIfNotNull();
                ForwardBackwardCalculate("Nds");
            }
        }

        /// <summary>
        /// Признак того, что гость платит НДС самостоятельно.
        /// По умолчанию false, т.е.НДС платит заведение общепита.
        /// </summary>
        public bool SplitVat { get; set; }

        /// <summary>
        /// Себестоимость за единицу товара.
        /// </summary>
        /// <value>Себестоимость за единицу товара.</value>
        /// <remarks>
        /// <para>Себестоимость за единицу товара и суммарная себестоимость связаны 
        /// между собой формулой Себестоимость(сум) = Себестоимость(ед) * Количество. 
        /// При присвоении одному из свойств, не происходит автоматического пересчета 
        /// других, поэтому надо устанавливать себестоимость за единицу и суммарную 
        /// совместно.</para>
        /// </remarks>
        public EvaluableDecimalValue CostPrice { get; set; }

        /// <summary>
        /// Суммарная себестоимость.
        /// </summary>
        /// <value>Суммарная себестоимость.</value>
        /// <remarks>
        /// <para>Себестоимость за единицу товара и суммарная себестоимость связаны 
        /// между собой формулой Себестоимость(сум) = Себестоимость(ед) * Количество. 
        /// При присвоении одному из свойств, не происходит автоматического пересчета 
        /// других, поэтому надо устанавливать себестоимость за единицу и суммарную 
        /// совместно.</para>
        /// </remarks>
        public EvaluableDecimalValue CostPriceSummary { get; set; }

        /// <summary>
        /// Процент скидки
        /// </summary>
        public decimal Discount { get; set; }

        public int NativeItemNumber
        {
            get { return nativeItemNumber; }
            set { nativeItemNumber = value; }
        }

        public StoreProductPair StoreProductPair
        {
            get { return new StoreProductPair(store, product); }
        }

        public ProductSizeStoreKey ProductKey
        {
            get { return new ProductSizeStoreKey(store, product, productSize, amountFactor); }
        }

        /// <summary>
        /// Номер таможенной декларации.
        /// </summary>
        public string CustomsDeclarationNumber { get; set; }

        /// <summary>
        /// <para>Комментарий</para>
        /// <para>(пока только для расходной накладной)</para>
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Активный прайс.
        /// </summary>
        public SupplierPriceList SupplierPrice { get; set; }

        /// <summary>
        /// Активный внутренний прайс.
        /// </summary>
        [CanBeNull]
        public IndependentPriceList IndependentPrice { get; set; }

        /// <summary>
        /// Связки поставщика соответсвующие товару айко.
        /// </summary>
        [NotNull]
        public IEnumerable<SupplierPriceListItem> SupplierPriceItems
        {
            get
            {
                return Product != null && Supplier != null && SupplierPrice != null
                    ? SupplierPrice.GetItemsByNativeProduct(Product)
                    : Enumerable.Empty<SupplierPriceListItem>();
            }
        }

        /// <summary>
        /// Товары из внутреннего прайс-листа
        /// </summary>
        [NotNull]
        public IEnumerable<IndependentPriceListItem> IndependentPriceItems
        {
            get
            {
                return Product != null && IndependentPrice != null
                    ? IndependentPrice.GetItemsByNativeProduct(Product)
                    : Enumerable.Empty<IndependentPriceListItem>();
            }
        }

        /// <summary>
        /// Связки поставщика соответсвующие товару поставщика.
        /// </summary>
        public IEnumerable<SupplierPriceListItem> SupplierPriceItemsForSupplierProduct
        {
            get
            {
                return SupplierProduct != null && Supplier != null && SupplierPrice != null
                    ? SupplierPrice.GetItemsBySupplierProduct(SupplierProduct)
                    : Enumerable.Empty<SupplierPriceListItem>();
            }
        }

        /// <summary>
        /// Связка товар поставщика - айко товар с учётом фасовки.
        /// Если отображаются товары поставщика то связка может быть на любой такой товар, иначе только на айко товар.
        /// </summary>
        [CanBeNull]
        public SupplierPriceListItem SupplierPriceItem
        {
            get
            {
                return CommonConfig.Instance.UseSupplierProducts || invoiceRecordEditMode == InvoiceEditMode.RecognizeInput
                           ? SupplierPriceItems.FirstOrDefault(pi => pi.SupplierProduct.Equals(supplierProduct) && pi.ContainerId == Container.Id)
                           : SupplierPriceItems.FirstOrDefault(pi => pi.SupplierProduct.Equals(Product) && pi.ContainerId == Container.Id);
            }
        }

        /// <summary>
        /// Товар из внутреннего прайс-листа с учетом фасовки
        /// </summary>
        [CanBeNull]
        public IndependentPriceListItem IndependentPriceItem
        {
            get { return IndependentPriceItems.FirstOrDefault(item => item.NativeProduct.Equals(Product) && item.ContainerId == Container.Id); }
        }

        /// <summary>
        /// Возвращает true, если у заданного продукта имеется связка с товаром поставщика.
        /// </summary>
        public bool HasSupplierPriceItem => SupplierPriceItems.Any();

        /// <summary>
        /// Возвращает <c>true</c>, если есть цена в свободном прайс-листе
        /// </summary>
        public bool HasIndependentPriceListItem => IndependentPriceItems.Any();

        /// <summary>
        /// Номер позиции в заказе. В отличии от <see cref="Num"/> не меняется после удаления позиций.
        /// Используется для однозначного сопоставления позиций во внешних системах
        /// </summary>
        public int? OrderNum { get; set; }

        /// <summary>
        /// Производитель/Импортер
        /// </summary>
        [CanBeNull]
        public User Producer { get; set; }

        /// <summary>
        /// Код для АД
        /// </summary>
        public string AlcoholClassCode
        {
            get
            {
                return product == null || product.AlcoholClass == null ? string.Empty : product.AlcoholClass.Code;
            }
        }

        /// <summary>
        /// Заказанное количество отличается от подтвержденного/отгруженного
        /// Свойство актуально только для EDI накладной
        /// </summary>
        public bool IsDifferentAmount { get; set; }

        /// <summary>
        /// Заказанная стоимость отличается от подтвержденной/отгруженной
        /// Свойство актуально только для EDI накладной
        /// </summary>
        public bool IsDifferentPrice { get; set; }

        /// <summary>
        /// Заказанная фасовка отличается от подтвержденной/отгруженной
        /// Свойство актуально только для EDI накладной
        /// </summary>
        public bool IsDifferentMeasureUnit { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает цену с НДС или без НДС в зависимости от настроек учета НДС у корпорации.
        /// </summary>
        public decimal GetPriceCurrentPrice()
        {
            if (VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE))
            {
                return Price;
            }

            if (VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
            {
                return PriceWithoutVat;
            }

            throw new UnsupportedEnumValueException<VatAccounting>(VatAccounting);
        }

        public SupplierInfo GetSupplierInfo()
        {
            if (Supplier == null || Container == null || Product == null)
            {
                return null;
            }
            SupplierInfo supplierInfo = SupplierInfo.GetSupplierInfosBySupplier(Supplier).FirstOrDefault(supInfo => supInfo.ContainerId == Container.Id &&
                                                                                                                    supInfo.NativeProduct == Product);
            return supplierInfo;
        }

        /// <summary>
        /// Включение пересчета зависимых полей
        /// </summary>
        public void EnableRecalc()
        {
            recalcEnabled = true;
        }

        /// <summary>
        /// Выключение пересчета зависимых полей
        /// </summary>
        public void DisableRecalc()
        {
            recalcEnabled = false;
        }

        /// <summary>
        /// Расчет плотности
        /// </summary>
        private decimal GetActualUnitWeight()
        {
            if (product != null && product.UseRangeForInvoices && actualUnitWeight != 0)
            {
                return actualUnitWeight;
            }
            return Container.Count;
        }

        /// <summary>
        /// Расчет количество (в основных ед. измерения) на основании количества в единицах тары
        /// </summary>
        /// <remarks>
        /// [Количество (кг)] = [плотность] * [количество (ящик)]
        /// </remarks>
        public decimal CalculateInCount(decimal packAmount)
        {
            decimal unitWeight = GetActualUnitWeight();
            return (unitWeight * packAmount).RoundAmountAsServer();
        }

        /// <summary>
        /// Сброс режима "Ввод данных с фиксированной ценой"
        /// </summary>
        public void ResetFixedPrice()
        {
            inFixedPriceForRecalc = false;
        }

        private void AssignContainer()
        {
            if (container == null && product != null)
            {
                container = Container.GetMeasureUnitContainer(product.MainUnit);
            }
            else if (container == null && supplierProduct != null)
            {
                container = Container.GetMeasureUnitContainer(supplierProduct.MainUnit);
            }
        }

        public void ProcessSupplierProduct(Product prevSupplierProduct, bool updateNds)
        {
            if (Supplier == null || supplierProduct == null)
            {
                return;
            }

            SupplierPriceListItem item;
            if (supplierProduct.IsOuter || container == null)
            {
                item = SupplierPriceItemsForSupplierProduct.FirstOrDefault();
            }
            else
            {
                item = SupplierPriceItemsForSupplierProduct.SingleOrDefault(pli => pli.Container.Equals(container));
            }

            if (item == null || Equals(prevSupplierProduct, supplierProduct))
            {
                if (SupplierPriceItem == null && !supplierProduct.IsOuter)
                {
                    product = supplierProduct;
                    if (updateNds)
                    {
                        nds = product.VatPercent;
                    }
                }

                return;
            }

            if (product == null || IsValidSupplierProduct(supplierProduct, product))
            {
                product = item.NativeProduct;
                inCountM = product.MainUnit;
                if (updateNds)
                {
                    nds = product.VatPercent;
                }

                container = product.Containers.GetContainer(item.ContainerId);
            }
            else
            {
                supplierProduct = prevSupplierProduct;
            }
        }

        /// <summary>
        /// Прямой пересчет 1: В зависимости от <see cref="VatAccounting"/>
        /// Стоимость = цена * количество
        /// или
        /// СуммаБезНДС = цена с НДС * количество
        /// </summary>
        private void ForwardCalculatePrice(bool withoutVatSetting)
        {
            if (Product == null)
            {
                return;
            }

            if (VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE) || withoutVatSetting)
            {
                inPrice = CalculateSumSafe(price);
            }
            else if (VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
            {
                summWithoutNds = CalculateSumSafe(priceWithoutVat);
            }
            else
            {
                throw new UnsupportedEnumValueException<VatAccounting>(VatAccounting);
            }
        }

        /// <summary>
        /// Возвращает <paramref name="newPrice"/> * <see cref="PackageAmount"/>
        /// </summary>
        /// <param name="newPrice">Цена</param>
        private decimal CalculateSumSafe(decimal newPrice)
        {
            var result = (newPrice * packageAmount).MoneyPrecisionMoneyRound();
            if (result <= maxInPrice)
            {
                return result;
            }

            if (newPrice != 0)
            {
                packageAmount = (maxInPrice / newPrice).RoundAmountAsServer();
            }

            return (newPrice * packageAmount).MoneyPrecisionMoneyRound();
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

        private bool IsValidSupplierProduct(Product outer, Product native)
        {
            return !collection.Except(this.AsSequence()).Any(r => Equals(r.SupplierProduct, outer) && !Equals(r.Product, native)) || (SupplierPrice != null &&
                !SupplierPrice.AllNotDeletedItems().Any(si => si.SupplierProduct.Equals(outer) && !si.NativeProduct.Equals(native)));
        }

        public Store GetStore()
        {
            return Store;
        }

        public Product GetProduct()
        {
            return Product;
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChangedIfNotNull([NotNull, CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ClipboardStartPasteRecord()
        {
            DisableRecalc();
        }

        public void ClipboardEndPasteRecord()
        {
            EnableRecalc();
        }

        /// <summary>
        /// Возвращает список возможных контейнеров (фасовок) для заданного продукта.
        /// </summary>        
        public List<Container> GetContainers()
        {
            var containers = new List<Container> { Container.GetMeasureUnitContainer(Product.MainUnit) };

            containers.AddRange(Product.NonDeletedContainers);

            return containers;
        }

        /// <summary>
        /// Обновляет продукт поставщика и пересчитывает цену.
        /// </summary>
        /// <param name="condition">Условие выбора продукта</param>
        /// <param name="onlyZeroPrices">Пересчитать только нулевые цены</param>
        /// <param name="isAfterClipboard">Обновление продукта поставщика происходит после вставки из буфера обмена</param>
        /// <param name="updateNds">Обновить НДС записи</param>
        public void UpdateSupplierProductAndPrice(Func<SupplierPriceListItem, bool> condition, bool onlyZeroPrices, bool isAfterClipboard, bool updateNds)
        {
            UpdateSupplierProduct(condition, isAfterClipboard, updateNds);
            UpdatePriceByPriceList(onlyZeroPrices);
        }

        /// <summary>
        /// Обновляет продукт поставщика
        /// </summary>
        /// <param name="condition">Условие выбора продукта</param>
        /// <param name="isAfterClipboard">Обновление продукта поставщика происходит после вставки из буфера обмена</param>
        /// <param name="updateNds">Обновить НДС продукта</param>
        public void UpdateSupplierProduct(Func<SupplierPriceListItem, bool> condition, bool isAfterClipboard, bool updateNds)
        {
            SupplierPriceListItem item;

            if (CommonConfig.Instance.UseSupplierProducts)
            {
                // RMS-44016: По возможности стараемся не менять продукт постащика. Если текущий продукт поставщика
                // есть среди подходящих в прайс-листе, то оставляем его, иначе берём любой (первый по алфавиту) из подходящих.
                var applicablePriceItems = SupplierPriceItems.Where(condition).ToArray();
                item = applicablePriceItems.FirstOrDefault(pli => Equals(pli.SupplierProduct, supplierProduct))
                       ?? applicablePriceItems.OrderBy(pli => pli.SupplierProduct.NameLocal).FirstOrDefault();
            }
            else
            {
                item = SupplierPriceItem;
            }

            isDataFromClipboard = isAfterClipboard;
            SetSupplierProduct(item != null ? item.SupplierProduct : null, updateNds);
        }

        /// <summary>
        /// Установить продукт поставщика и обновить цену
        /// </summary>
        /// <param name="value">Продукт</param>
        /// <param name="onlyZeroPrices">Обновить только нулевые цены</param>
        /// <param name="updateNds">Обновить НДС записи</param>
        private void SetSupplierProductAndUpdatePrices(Product value, bool onlyZeroPrices, bool updateNds)
        {
            SetSupplierProduct(value, updateNds);
            UpdatePriceByPriceList(onlyZeroPrices);
        }

        /// <summary>
        /// Установить продукт поставщика не обновляя цену
        /// </summary>
        /// <param name="value">Новый продукт поставщика</param>
        /// <param name="updateNds">Обновить НДС записи</param>
        private void SetSupplierProduct(Product value, bool updateNds)
        {
            Product prevProduct = supplierProduct;
            supplierProduct = value;
            RaisePropertyChangedIfNotNull();
            ProcessSupplierProduct(prevProduct, updateNds);
        }

        /// <summary>
        /// Устанавливает для позиции цену за фасовку из данного словаря с ценами за базовую единицу измерения, 
        /// если в нем есть цена для продукта данной записи.
        /// </summary>
        /// <param name="newPricesToApply">Словарь с ценами продуктов за базовую единицу измерения.</param>
        public void ApplyPrice(Dictionary<ProductSizeKey, decimal> newPricesToApply)
        {
            decimal newPrice;
            if (newPricesToApply.TryGetValue(new ProductSizeKey(product, productSize), out newPrice))
            {
                Price = newPrice * Container.Count;
            }
        }

        /// <summary>
        /// Обновляет цены.
        /// </summary>
        /// <param name="olnyZeroPrice">Если true, то обновляем только нулевые цены, если false то все.</param>
        /// <remarks>
        /// Приоритет обновления:
        /// 1. Прайс-лист поставшика
        /// 2. Внутренний прайс-лист
        /// 3. Цена по последнему приходу
        /// </remarks>
        public void UpdatePriceByPriceList(bool olnyZeroPrice)
        {
            // При фиксированной цене (был выполнен ввод пользователем вручную), запрещается
            // пересчет себестоимости в автоматическом режиме, т.к. он приводит к сбросу режима "фиксированной цены"
            if (inFixedPriceForRecalc || isDataFromClipboard)
            {
                isDataFromClipboard = false;
                return;
            }

            // если поставщика нет: нет продукта поставщика - не обновляем
            // по спеке для внутреннего прайс-листа тоже должен быть поставщик
            if (Supplier == null)
            {
                Price = 0m;
                return;
            }

            decimal? newPrice = null;

            if (SupplierProduct != null && SupplierPriceItem != null)
            {
                newPrice = SupplierPriceItem.CostPrice;
            }
            else if (IndependentPriceItem != null)
            {
                newPrice = IndependentPriceItem.CostPrice;
            }

            if (Container != null && !newPrice.HasValue &&
                CommonConfig.Instance.NeedSubstituteSupplierPrice &&
                SupplierProduct == null)
            {
                var si = SupplierInfo.GetSupplierInfoBy(Supplier, Product, Container.Id);
                if (si != null)
                {
                    newPrice = si.CostPrice.GetValueOrDefault();
                }
            }

            if (invoiceRecordEditMode != InvoiceEditMode.RecognizeInput)
            {
                if (newPrice.HasValue && recalcEnabled && (price == 0 || !olnyZeroPrice))
                {
                    if (VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE))
                    {
                        Price = newPrice.Value;
                    }
                    else if (VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
                    {
                        PriceWithoutVat = newPrice.Value;
                    }
                    else
                    {
                        throw new UnsupportedEnumValueException<VatAccounting>(VatAccounting);
                    }
                }
                else if (!olnyZeroPrice)
                {
                    Price = 0m;
                }
            }
        }

        /// <summary>
        /// Перерасчет записимых полей при изменении одного из них
        /// </summary>
        /// <param name="fName">Имя изменившегося поля</param>
        public void ForwardBackwardCalculate(string fName)
        {
            if (!recalcEnabled || Product == null)
            {
                return;
            }

            // По задаче: RMS-34146
            // После ручного ввода пользователем поля "Сумма" (InPrice) это поле переходит в фиксированное состояние
            // и теперь автоматически не пересчитывается при изменении поля "Количиство" (InCount). 
            // Вместо этого автоматические начинается пересчитываться поле "Цена за ед. тары" (Price)
            // Выход из этого режима: автоматический пересчет поля "Сумма" (InPrice) другими полями
            // RMS-45106
            // В случае, если НДС в стоимости не учитывается, для поля "Сумма без НДС" выполняются действия, описанные выше, а поле "Сумма"
            // не фиксируется.
            // Выход из этого режима: автоматический пересчет поля "Сумма без НДС" (SummWithoutNDS) другими полями.
            var isInPriceChanging = VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE) && fName == "InPrice";
            var isSumWithoutVatChanging = VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE) && fName == "SummWithoutNDS";
            if (isInPriceChanging || isSumWithoutVatChanging)
            {
                inFixedPriceForRecalc = true;
            }
            else if (fName.In("Price", "PriceWithoutVat", "SummWithoutNDS", "InPrice"))
            {
                inFixedPriceForRecalc = false;
            }

            RecalculatePriceIfNeed(fName);

            // RMS-45106
            // если НДС не учитывается в стоимости, поле Price редактировать из UI РН и ПН нельзя,
            // но это поле может быть изменено при выполнении расчета цены по прейскуранту или из Акта реализации
            // в этом случае прямой и обратный пересчет выполняем с учетом НДС в стоимости
            var withoutVatSetting = Equals(CurrentDocumentType, DocumentType.SALES_DOCUMENT) || fName == "Price";

            // Задаем логику 3-х прямых и 3-х обратных пересчетов

            // Поля, после изменения которых применяется прямой пересчет 1,2,3
            var isForwardConverting1 = fName.In("InCount", "Price", "PriceWithoutVat", "InCountM", "PriceM");
            var isForwardConverting2 = isForwardConverting1 || fName.In("Nds", "InPrice");
            var isForwardConverting3 = isForwardConverting2 || fName == "NDSSumm";

            // Поля, после изменения которых применяется обратный пересчет 1,2,3
            var isBackwardConverting1 = fName == "SummWithoutNDS";
            var isBackwardConverting2 = isBackwardConverting1;
            var isBackwardConverting3 = isBackwardConverting2 || fName == "InPrice" || fName == "NDSSumm" ||
                                        (fName == "InCount" && inFixedPriceForRecalc);

            // Прямой пересчет 1 - (Стоимость = цена * кол-во)
            if (isForwardConverting1)
            {
                if (inFixedPriceForRecalc == false)
                {
                    // Пересчет "Стоимость" (за исключением случая, когда "Стоимость" фиксирована)
                    ForwardCalculatePrice(withoutVatSetting);
                }

                packageAmount = Container.GetContainerCountByWeigthForIncomingInvoice(inCount, GetActualUnitWeight()).RoundAmountAsServer();
            }

            if (fName == "InPrice" || withoutVatSetting)
            {
                if (isForwardConverting2)
                {
                    ndsSumm = (inPrice * (nds / (100m + nds))).MoneyPrecisionMoneyRound();
                }

                if (isForwardConverting3)
                {
                    summWithoutNds = inPrice - ndsSumm;
                }
            }
            else
            {
                if (isForwardConverting2)
                {
                    ForwardCalculateVatSum();
                }

                if (isForwardConverting3)
                {
                    ForwardCalculateSum();
                }
            }

            // Обратный пересчет 1 - (суммаНДС = суммаБезНДС * ставкаНДС / 100)
            if (isBackwardConverting1)
            {
                ndsSumm = (summWithoutNds * (nds / 100m)).MoneyPrecisionMoneyRound();
            }

            // Обратный пересчет 2 - (стоимость = суммаБезНДС + суммаНДС)
            if (isBackwardConverting2)
            {
                inPrice = summWithoutNds + ndsSumm;
                inFixedPriceForRecalc = false;
            }

            // Обратный пересчет 3:
            // цена с НДС = стоимость / кол-во
            // цена без НДС = сумма без НДС / кол-во
            if (isBackwardConverting3 && packageAmount != 0m)
            {
                price = (inPrice / packageAmount).MoneyPrecisionMoneyRound();
                priceWithoutVat = (summWithoutNds / packageAmount).MoneyPrecisionMoneyRound();
            }
        }

        /// <summary>
        /// Прямой пересчет <see cref="SummWithoutNDS"/> или <see cref="InPrice"/> в зависимости от <see cref="VatAccounting"/>
        /// </summary>
        /// <remarks>
        /// суммаБезНДС = Стоимость - суммаНДС
        /// или
        /// Стоимость = суммаБезНДС + суммаНДС
        /// </remarks>
        private void ForwardCalculateSum()
        {
            if (VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE))
            {
                summWithoutNds = inPrice - ndsSumm;
            }
            else if (VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
            {
                inPrice = summWithoutNds + ndsSumm;
            }
            else
            {
                throw new UnsupportedEnumValueException<VatAccounting>(VatAccounting);
            }
        }

        /// <summary>
        /// Прямой пересчет <see cref="NDSSumm"/> в зависимости от <see cref="VatAccounting"/>
        /// </summary>
        /// <remarks>
        /// суммаНДС = Стоимость * (ставкаНДС / (100 + ставкаНДС)))
        /// или
        /// суммаНДС = СуммаБезНДС * (ставкаНДС / 100)
        /// </remarks>
        private void ForwardCalculateVatSum()
        {
            if (VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE))
            {
                ndsSumm = (inPrice * (nds / (100m + nds))).MoneyPrecisionMoneyRound();
            }
            else if (VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
            {
                ndsSumm = (summWithoutNds * (nds / 100m)).MoneyPrecisionMoneyRound();
            }
            else
            {
                throw new UnsupportedEnumValueException<VatAccounting>(VatAccounting);
            }
        }

        /// <summary>
        /// Пересчитывает <see cref="Price"/> или <see cref="PriceWithoutVat"/>
        /// относительно <see cref="Nds"/>
        /// </summary>
        /// <param name="fieldName">Измененное свойство.</param>
        /// <remarks>
        /// Пересчет производится в случае, если меняется цена с НДС, цена без НДС или процент НДС.
        /// </remarks>
        private void RecalculatePriceIfNeed(string fieldName)
        {
            var vatCoefficient = 1m + nds / 100m;
            switch (fieldName)
            {
                case "Price":
                    priceWithoutVat = (price / vatCoefficient).MoneyPrecisionMoneyRound();
                    break;
                case "PriceWithoutVat":
                    price = priceWithoutVat * vatCoefficient;
                    break;
                case "Nds":
                    var withoutVatSetting = Equals(CurrentDocumentType, DocumentType.SALES_DOCUMENT) || fieldName == "Price";
                    if (VatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE) || withoutVatSetting)
                    {
                        priceWithoutVat = (price / vatCoefficient).MoneyPrecisionMoneyRound();
                    }
                    else if (VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
                    {
                        price = priceWithoutVat * vatCoefficient;
                    }
                    else
                    {
                        throw new UnsupportedEnumValueException<VatAccounting>(VatAccounting);
                    }

                    break;
            }
        }

        #endregion
    }
}