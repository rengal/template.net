using System;
using System.Collections.Generic;
using Resto.Common.Extensions;
using Resto.Common.Interfaces;
using Resto.Common.Localization;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class AbstractDocumentListRecord : IGroupProcessableDocument
    {
        public virtual Account RevenueAccountData
        {
            get { return null; }
        }

        public virtual int CashDisplayNumber
        {
            get { return -1; }
        }

        public virtual int SessionDisplayNumber
        {
            get { return -1; }
        }

        public virtual EditableDocumentType EditableDocumentType
        {
            get { return EditableDocumentType.DEFAULT_DOCUMENT; }
        }

        public virtual string StoreFromAsString
        {
            get { return string.Empty; }
        }

        public virtual string StoreToAsString
        {
            get { return string.Empty; }
        }

        public virtual Store SalesStore
        {
            get { return null; }
        }

        public virtual string DepartmentToAsString
        {
            get { return string.Empty; }
        }

        public virtual string ExecutorToAsString
        {
            get { return string.Empty; }
        }

        public virtual string Name
        {
            get { return string.Empty; }
        }

        public virtual string StoreInfoField
        {
            get { return string.Empty; }
        }

        public virtual bool IsAutomatic
        {
            get { return false; }
        }

        public virtual decimal SumToShow
        {
            get { return new decimal(); }
            set { }
        }

        public virtual decimal? SumWithoutNdsToShow
        {
            get { return new decimal(); }
            set { }
        }

        public virtual decimal? NDSSum
        {
            get { return new decimal(); }
            set { }
        }

        /// <summary>
        /// Разница по весу.
        /// </summary>
        public virtual decimal WeightDifferenceToShow
        {
            get { return new decimal(); }
            set { }
        }

        /// <summary>
        /// Среднний вес.
        /// </summary>
        public virtual decimal AverageWeightToShow
        {
            get { return new decimal(); }
            set { }
        }

        /// <summary>
        /// Отклонение по среднему весу
        /// </summary>
        public virtual decimal DeviationWeightToShow
        {
            get { return new decimal(); }
            set { }
        }

        public virtual decimal WeightInToShow
        {
            get { return new decimal(); }
            set { }
        }

        public virtual decimal WeightOutToShow
        {
            get { return new decimal(); }
            set
            {
            }
        }

        public virtual string InvoiceType
        {
            get
            {
                if (Type.Equals(DocumentType.INCOMING_INVOICE) || Type.Equals(DocumentType.INCOMING_SERVICE))
                    return Resources.AbstractDocumentListRecordIncoming;
                if (Type.Equals(DocumentType.OUTGOING_INVOICE) || Type.Equals(DocumentType.OUTGOING_SERVICE))
                    return Resources.AbstractDocumentListRecordOutgoing;
                return string.Empty;
            }
        }

        public virtual Account AccountSurplusData
        {
            get { return null; }
        }

        public virtual Account AccountShortageData
        {
            get { return null; }
        }

        public virtual decimal SurplusSummData
        {
            get { return new Decimal(); }
        }

        public virtual decimal ShortageSummData
        {
            get { return new Decimal(); }
        }

        public virtual string IncomingInvoiceCaptionString
        {
            get { return string.Empty; }
        }

        public virtual string OutgoingInvoiceCaptionString
        {
            get { return string.Empty; }
        }

        public string IncomingNumber
        {
            get { return string.Empty; }
        }


        public virtual string DocumentShortCaption
        {
            get { return Number != null ? string.Format(Resources.AbstractDocumentListRecordNumberFrom, Number, Date.GetValueOrFakeDefault().ToShortDateString()) : string.Empty; }
        }

        public virtual string DocumentFullCaption
        {
            get
            {
                return Type != null && Number != null
                           ? string.Format(Resources.AbstractDocumentListRecordNumberFrom1, Type.GetLocalName(), Number, Date.GetValueOrFakeDefault().ToShortDateString())
                           : string.Empty;
            }
        }

        /// <summary>
        /// Информация о док-те в формате - "Тип документа от Дата документе: Список товаров по документу"
        /// пример: "Акт приготовления от 21.17.2011: Салат "Крабовый", Винегрет"
        /// </summary>
        public virtual string DocumentSummaryCaption
        {
            get
            {
                return Type != null ? string.Format(Resources.AbstractDocumentListRecordFrom, Type.GetLocalName(), Date.GetValueOrFakeDefault().ToShortDateString(), DocumentSummary) : string.Empty;
            }
        }

        public virtual string AccountToAsString
        {
            get { return string.Empty; }
        }

        public virtual Account ExpenseAccountData
        {
            get { return null; }
        }

        public virtual decimal? DiscountSumValue
        {
            get { return new decimal(); }
            set { }
        }

        public virtual decimal? SelfPriceSumValue
        {
            get { return new decimal(); }
            set { }
        }

        public virtual decimal? TotalCostData
        {
            get { return null; }
        }

        public virtual List<Store> StoresList
        {
            get { return new List<Store>(); }
        }

        public override string ToString()
        {
            return string.Format(Resources.AbstractDocumentListRecordFrom, Type.GetLocalName(), Date.GetValueOrFakeDefault().ToShortDateString(), Comment);
        }

        #region MenuChangeDocumentListRecord


        public virtual string DepartmentsString
        {
            get { return string.Empty; }
        }

        public virtual DateTime? DateToValue
        {
            get { return null; }
        }

        public virtual string ScheduleNameValue
        {
            get { return null; }
        }

        #endregion

        #region FuelAcceptanceListRecord

        /// <summary>
        /// Список ёмкостей
        /// </summary>
        public virtual string VesselsAsString
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Список топливов
        /// </summary>
        public virtual string ProductsAsString
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Список накладных
        /// </summary>
        public virtual string InvoicesAsString
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Время отъезда бензовоза
        /// </summary>
        public virtual DateTime? FuelTankOutcomingDate
        {
            get { return null; }
        }

        public virtual string EmployeePassToAccountString
        {
            get { return string.Empty; }
        }

        public virtual string DueDateString
        {
            get { return string.Empty; }
        }

        public virtual string PaymentDateString
        {
            get { return string.Empty; }
        }

        public virtual string IsDeliveryOnTimeString
        {
            get { return string.Empty; }
        }

        public virtual string IsMatchesToTheOrderString
        {
            get { return string.Empty; }
        }

        public virtual string DocNumberString
        {
            get { return string.Empty; }
        }

        public virtual string DocDateString
        {
            get { return string.Empty; }
        }

        public virtual string SupplierString
        {
            get { return string.Empty; }
        }

        public virtual string SupplierTypeSting
        {
            get { return string.Empty; }
        }

        public virtual string FromDateString
        {
            get { return string.Empty; }
        }

        #endregion

        #region FuelAcceptanceListRecord
        /// <summary>
        /// Тип замера (ручной/аппаратный)
        /// </summary>
        public virtual string MeasureTypeAsString
        {
            get { return string.Empty; }
        }
        #endregion

        #region EdiOrderDocumentRecord

        public virtual string DeliveryDateAsString
        {
            get { return string.Empty; }
        }

        public virtual string SellerAsString
        {
            get { return string.Empty; }
        }

        public virtual string PayerAsString
        {
            get { return string.Empty; }
        }

        public virtual string StatusAsString
        {
            get { return string.Empty; }
        }

        #endregion
    }
}