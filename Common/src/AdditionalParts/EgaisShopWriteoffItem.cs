using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class EgaisShopWriteoffItem : IEgaisRecordWithTransactionBRegId, IEgaisRecordWithMarks
    {
        #region Fields

        [Transient]
        private const decimal OneDalVolume = 10m;

        [Transient]
        private decimal internalRemainsSum;

        [Transient]
        private decimal internalWriteoffSum;

        [Transient]
        private bool isShopWriteofFromStore;

        private EgaisTransactionPart transactionBRegId;

        [Transient]
        private bool sumSaleWasManualyChanged;

        [Transient]
        private decimal minimumRetailPrice;

        #endregion

        #region CTOR

        public EgaisShopWriteoffItem(EgaisAbstractInvoiceItem item, bool? packed, [CanBeNull] List<EgaisMark> marks)
            : this(Guid.NewGuid(), item.Num, item.SupplierProduct, item.AmountUnit, item.ContainerId, item.ProductInfo, item.BRegId)
        {
            Product = item.Product;
            Packed = packed;
            Amount = item.ActualAmount ?? item.Amount;
            Marks = marks;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Сумма остатков продукта <see cref="Product"/> по всем складам выбранным для документа списания.
        /// </summary>
        public decimal InternalRemainsSum
        {
            get { return internalRemainsSum; }
            set { internalRemainsSum = value; }
        }

        /// <summary>
        /// Сумма списания товара <see cref="Product"/> на выбранную дату и по всем складам выбранным для документа списания.
        /// </summary>
        public decimal InternalWriteoffSum
        {
            get { return internalWriteoffSum; }
            set { internalWriteoffSum = value; }
        }

        /// <summary>
        /// Признак того что строка списания с торгового зала (регистр 2) сформирована по остаткам на складе (регистр 1).
        /// </summary>
        public bool IsShopWriteofFromStore
        {
            get { return isShopWriteofFromStore; }
            set { isShopWriteofFromStore = value; }
        }

        /// <summary>
        /// Сумма продажи v3
        /// </summary>
        public decimal SaleSum
        {
            get { return SumSale.GetValueOrDefault(); }
            set { SumSale = value; }
        }

        /// <summary>
        /// Количество марок
        /// </summary>
        public int MarkCount
        {
            get { return Marks != null ? Marks.Count : 0; }
        }

        /// <summary>
        /// Справка 2 из расходной накладной, нужна для копирования
        /// </summary>
        public string PrevBRegId { get; set; }

        /// <summary>
        /// Транзакция, к которой привязана справка 2
        /// </summary>
        public EgaisTransactionPart TransactionBRegId
        {
            get { return transactionBRegId; }
            set
            {
                transactionBRegId = value;
                if (transactionBRegId != null)
                {
                    BRegId = value.Key.BRegId;
                }
                else
                {
                    BRegId = string.Empty;
                }
            }
        }

        /// <summary>
        /// Минимальная розничная цена за 0,5л
        /// </summary>
        public decimal MinimumRetailPrice
        {
            get { return minimumRetailPrice; }
            set { minimumRetailPrice = value; }
        }

        /// <summary>
        /// Сумма продажи была изменена пользователем
        /// </summary>
        public bool SumSaleWasManualyChanged
        {
            get { return sumSaleWasManualyChanged; }
            set { sumSaleWasManualyChanged = value; }
        }

        #endregion

        /// <summary>
        /// Высчитывает сумму продажи по минимальной розничной цене
        /// </summary>
        [Pure]
        public decimal CalculateSumSale()
        {
            var capacity = Capacity ?? OneDalVolume;
            // Умножаем литры на 2 - получаем сколько раз у нас по 0,5л.,
            // умножаем это количество на минимальную розничную цену за 0,5л.
            return Amount.GetValueOrDefault() * capacity * 2m * MinimumRetailPrice;
        }
    }
}
