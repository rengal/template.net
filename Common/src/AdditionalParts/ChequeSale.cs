using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ChequeSale
    {
        public const int DefaultFiscalSection = 1;
        public const int MinFiscalSection = 1;
        public const int MaxFiscalSection = 16;
        [Transient]
        private bool hasComment;
        private bool isComment;
        private string comment;

        /// <summary>
        /// Служебный флаг для фильтрации при объединении позиций в чеке
        /// </summary>
        public bool HasComment
        {
            get => hasComment;
            set => hasComment = value;
        }

        public bool IsComment
        {
            get => isComment;
            set => isComment = value;
        }

        public string Comment
        {
            get => comment;
            set => comment = value;
        }

        [Transient]
        private Guid productId;
        /// <summary>
        /// ID продукта. В ФР передавать не нужно, нужен для выгрузки. 
        /// </summary>
        public Guid ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        // ReSharper disable ConvertToAutoProperty
        private string guestName;
        /// <summary>
        /// Имя гостя, которому соответствует позиция в чеке
        /// </summary>
        public string GuestName
        {
            get { return guestName; }
            set { guestName = value; }
        }

        [Transient, CanBeNull]
        private ChequeSaleNameInfo nameInfo;
        /// <summary>
        /// Составное название позиции в чеке, когда сумма позиции вычисляется в iiko,
        /// а в ФР передается единичное количество с дополнительной информацией о рельном количестве в названии,
        /// а в качестве цены передается полная сумма позиции.
        /// null, если сумму позиции вычисляет сам ФР: <see cref="Framework.Common.Currency.CurrencyHelper.CalculateItemSumOnCashRegister"/>
        /// </summary>
        [CanBeNull]
        public ChequeSaleNameInfo NameInfo
        {
            get { return nameInfo; }
            set { nameInfo = value; }
        }

        private string itemCategory;
        /// <summary>
        /// Признак предмета расчета (код)
        /// </summary>
        public string ItemCategory
        {
            get { return itemCategory; }
            set { itemCategory = value; }
        }

        private ChequeTransferType transferType = ChequeTransferType.FullPayment;
        /// <summary>
        /// Признак способа расчета
        /// </summary>
        public ChequeTransferType TransferType
        {
            get { return transferType; }
            set { transferType = value; }
        }

        public enum ChequeTransferType
        {
            FullPrepayment = 1,
            PartialPrepayment = 2,
            Advance = 3,
            FullPayment = 4,
            PartialCredit = 5,
            /// <summary>
            /// Передача в кредит
            /// </summary>
            Credit = 6,
            /// <summary>
            /// Оплата кредита
            /// </summary>
            RepaymentOfCredit = 7
        }

        // ReSharper disable ConvertToAutoProperty
        private string commodityCode;
        /// <summary>
        /// Код товарной номенклатуры (код маркировки, тег 1162)
        /// </summary>
        public string CommodityCode
        {
            get { return commodityCode; }
            set { commodityCode = value; }
        }

        private string commodityMark;
        /// <summary>
        /// Данные, прочитанные со штрихкода
        /// </summary>
        public string CommodityMark
        {
            get => commodityMark;
            set => commodityMark = value;
        }

        private string gtinCode;
        /// <summary>
        /// 14-тизначный "Global Trade Item Number" для позиции чека. Может быть пустым.
        /// </summary>
        public string GtinCode
        {
            get { return gtinCode; }
            set { gtinCode = value; }
        }
        // ReSharper restore ConvertToAutoProperty

        private Contractor contractor;
        /// <summary>
        /// Комитент
        /// </summary>
        public Contractor Contractor
        {
            get { return contractor; }
            set { contractor = value; }
        }
        
        private string taricCode;
        /// <summary>
        /// Код товарной номенклатуры для внешнеэкономической деятельности по евростандарту классификации TARIC
        /// </summary>
        public string TaricCode
        {
            get { return taricCode; }
            set { taricCode = value; }
        }

        private Ffd12ChequeSaleData ffd12;
        /// <summary>
        /// Код маркироваки в соответствии с протоколом ФФД 1.2
        /// </summary>
        public Ffd12ChequeSaleData Ffd12
        {
            get { return ffd12; }
            set { ffd12 = value; }
        }

        private KzChequeSaleData kzChequeSaleData;
        /// <summary>
        /// Дополнительные данные о товаре в соответствии с законодательством Казахстана
        /// </summary>
        public KzChequeSaleData KzChequeSaleData
        {
            get { return kzChequeSaleData; }
            set { kzChequeSaleData = value; }
        }

        private UkraineChequeSaleData ukraineChequeSaleData;
        /// <summary>
        /// Дополнительные данные о товаре в соответствии с законодательством Украины
        /// </summary>
        public UkraineChequeSaleData UkraineChequeSaleData
        {
            get { return ukraineChequeSaleData; }
            set { ukraineChequeSaleData = value; }
        }

        private UzChequeSaleData uzChequeSaleData;
        /// <summary>
        ///Дополнительные Фискальные данные для Узбекистана
        /// </summary>
        public UzChequeSaleData UzChequeSaleData
        {
            get { return uzChequeSaleData; }
            set { uzChequeSaleData = value; }
        }

        private bool isSplitVat;
        /// <summary>
        /// Отдельный НДС который оплачивается клиентом
        /// </summary>
        public bool IsSplitVat
        {
            get { return isSplitVat; }
            set { isSplitVat = value; }
        }
    }

    public sealed class ChequeSaleNameInfo : IEquatable<ChequeSaleNameInfo>
    {
        private readonly string productName;
        private readonly decimal realAmount;
        private readonly string measuringUnitName;

        public ChequeSaleNameInfo([NotNull] string productName, decimal realAmount, [NotNull] string measuringUnitName)
        {
            if (productName == null)
                throw new ArgumentNullException(nameof(productName));
            if (measuringUnitName == null)
                throw new ArgumentNullException(nameof(measuringUnitName));

            this.productName = productName;
            this.realAmount = realAmount;
            this.measuringUnitName = measuringUnitName;
        }

        public string ProductName
        {
            get { return productName; }
        }

        public decimal RealAmount
        {
            get { return realAmount; }
        }

        public string MeasuringUnitName
        {
            get { return measuringUnitName; }
        }

        #region Equality
        public bool Equals(ChequeSaleNameInfo other)
        {
            return productName.Equals(other.ProductName) && measuringUnitName.Equals(other.MeasuringUnitName);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other is ChequeSaleNameInfo && Equals((ChequeSaleNameInfo)other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (productName.GetHashCode() * 397) ^ measuringUnitName.GetHashCode();
            }
        }
        #endregion
    }
}