using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Interfaces;
using Resto.Data;
using Container = Resto.Data.Container;

namespace Resto.Common.Models
{
    /// <summary>
    /// Запись документа для UI-представления в Grid
    /// </summary>
    public sealed class IncomingDocumentRecord : INotifyPropertyChanged, IRecordWithCost<IncomingDocumentItem>, IRestoGridRow, IDocumentRecordWithProductSize
    {
        #region Fields

        private readonly IncomingDocumentItem item;

        private Decimal amount;

        private Decimal balance;

        private BarcodeCompletionData barcode;

        private Container container;

        private Decimal containerAmount;

        private Decimal costForOne;

        private Decimal costWhole;

        private Boolean deleted;

        private decimal mainProductAmountPercent;

        private int number;

        private Decimal price;

        private Product product;

        private ProductSize productSize;

        private decimal amountFactor = ProductSizeServerConstants.INSTANCE.DefaultAmountFactor;

        private decimal restInStoreFromBefore;

        private decimal restInStoreToBefore;

        private MeasureUnit unit;

        private ProductNumCompletionData productNum;

        #endregion

        #region Constructors

        public IncomingDocumentRecord()
        {
        }

        public IncomingDocumentRecord(IncomingDocumentItem item)
        {
            this.item = item;
            if (this.item != null)
            {
                Fill();
            }
            else
            {
                number = 1;
            }
        }

        public IncomingDocumentRecord(IncomingDocumentItem item, int number)
            : this(item)
        {
            if (item != null)
            {
                item.Number = number;
            }
            else
            {
                this.number = number > 0 ? number : 1;
            }
        }

        #endregion

        #region Properties

        public IncomingDocumentItem Item
        {
            get { return item; }
        }

        /// <summary>
        /// Процент себестоимости
        /// </summary>
        public decimal MainProductAmountPercent
        {
            get { return mainProductAmountPercent; }
            set
            {
                mainProductAmountPercent = value;
                RaisePropertyChangedIfNotNull("MainProductAmountPercent");
            }
        }

        /// <summary>
        /// Количество в фасовке
        /// </summary>
        public Decimal ContainerAmount
        {
            get { return containerAmount; }
            set
            {
                containerAmount = value;
                RaisePropertyChangedIfNotNull("ContainerAmount");
            }
        }

        /// <summary>
        /// Фасовка
        /// </summary>
        public Container Container
        {
            get
            {
                if (container == null && product != null)
                {
                    container = Container.GetMeasureUnitContainer(product.MainUnit);
                }
                else if (container == null)
                {
                    container = Container.GetEmptyContainer();
                }
                return container;
            }
            set
            {
                if (Equals(container, value))
                {
                    return;
                }
                container = value;
                UpdateBarcode();
                RaisePropertyChangedIfNotNull("Container");
            }
        }

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
                if (product == value)
                {
                    return;
                }
                product = value;
                UpdateBarcode();
                RaisePropertyChangedIfNotNull("Product");
            }
        }

        /// <summary>
        /// Размер блюда
        /// </summary>
        /// <remarks>
        /// Может использоваться только в документах списания для блюд, списываемых по ингредиентам.
        /// </remarks>
        public ProductSize ProductSize
        {
            get { return productSize; }
            set
            {
                if (productSize == value)
                {
                    return;
                }
                productSize = value;
                RaisePropertyChangedIfNotNull("ProductSize");
            }
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
                if (amountFactor == value)
                {
                    return;
                }
                amountFactor = value;
                RaisePropertyChangedIfNotNull("AmountFactor");
            }
        }

        [UsedImplicitly] // используется для привязки столбца таблицы.
        public BarcodeCompletionData Barcode
        {
            get { return barcode; }
            set
            {
                if (barcode == value)
                {
                    return;
                }
                barcode = value;
                if (value != null)
                {
                    Product = value.Product;
                    Container = Product.GetContainerById(value.BarcodeContainer.GetContainerIdOrDefault());
                }
                RaisePropertyChangedIfNotNull("Barcode");
            }
        }

        /// <summary>
        /// Количество в основной ед.изм продукта
        /// </summary>
        public Decimal Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                RaisePropertyChangedIfNotNull("Amount");
            }
        }

        /// <summary>
        /// Основная ед.изм продукта
        /// </summary>
        public MeasureUnit Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                RaisePropertyChangedIfNotNull("Unit");
            }
        }

        /// <summary>
        /// Цена
        /// </summary>
        public Decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChangedIfNotNull("Price");
            }
        }

        /// <summary>
        /// Себестоимость за единицу продукта
        /// </summary>
        public Decimal CostForOne
        {
            get { return costForOne; }
            set
            {
                costForOne = value;
                RaisePropertyChangedIfNotNull("CostForOne");
            }
        }

        /// <summary>
        /// Себестоимость за весь продукт
        /// </summary>
        public Decimal CostWhole
        {
            get { return costWhole; } //return (costForOne * amount).RoundSumAsServer(); }
            set { costWhole = value; }
        }

        /// <summary>
        /// Остатки продукта
        /// </summary>
        public Decimal Balance
        {
            get { return balance; }
            set
            {
                balance = value;
                RaisePropertyChangedIfNotNull("Balance");
            }
        }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        /// <summary>
        /// Признак удаления записи
        /// </summary>
        public Boolean Deleted
        {
            get { return deleted; }
            set
            {
                deleted = value;
                RaisePropertyChangedIfNotNull("Deleted");
            }
        }

        /// <summary>
        /// Остатки на исходном складе до проведения
        /// </summary>
        public decimal RestInStoreFromBefore
        {
            get { return restInStoreFromBefore; }
            set
            {
                restInStoreFromBefore = value;
                RaisePropertyChangedIfNotNull("RestInStoreFromBefore");
            }
        }

        /// <summary>
        /// Остатки на складе-назначении до проведения
        /// </summary>
        public decimal RestInStoreToBefore
        {
            get { return restInStoreToBefore; }
            set
            {
                restInStoreToBefore = value;
                RaisePropertyChangedIfNotNull("RestInStoreToBefore");
            }
        }

        public bool IsEmptyRecord
        {
            get { return product == null; }
        }

        public Store Store
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Производитель/Поставщик
        /// </summary>
        public User Producer { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void Fill()
        {
            number = item.Number;
            deleted = item.Deleted;
            product = item.Product;
            productSize = item.ProductSize;
            amountFactor = item.AmountFactor;
            amount = item.Amount;
            unit = item.Unit;
            price = item.Product != null ? item.Product.SalePrice : 0;
            costForOne = 0;
            costWhole = 0;
            balance = 0;
            mainProductAmountPercent = item.MainProductAmountPercent;
            Container = item.Container;
            if (container.Count != 0)
            {
                containerAmount = amount / container.Count;
            }
            Producer = item.Producer;
        }

        [NotNull]
        public IncomingDocumentItem CreateDocumentItem()
        {
            Contract.Ensures(Contract.Result<IncomingDocumentItem>() != null);

            return new IncomingDocumentItem(
                item != null ? item.Id : Guid.NewGuid(),
                Number,
                Product,
                ProductSize,
                AmountFactor,
                Amount,
                (Product ?? new Product()).MainUnit,
                Container ?? Container.GetEmptyContainer())
                {
                    MainProductAmountPercent = mainProductAmountPercent,
                };
        }

        private void RaisePropertyChangedIfNotNull(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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

        #endregion
    }
}