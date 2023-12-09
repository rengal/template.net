// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Чек для печати на ФРе
    /// </summary>
    public interface IChequeTask
    {
        /// <summary>
        /// Имя кассира
        /// </summary>
        [NotNull]
        string CashierName { get; }

        /// <summary>
        /// Признак детализированного чека (содержащего сведения о нефискальных оплатах)
        /// </summary>
        bool Detailed { get; }

        /// <summary>
        /// Сумма оплаты наличными
        /// </summary>
        decimal CashPayment { get; }

        /// <summary>
        /// Сумма чека
        /// </summary>
        decimal ResultSum { get; }

        /// <summary>
        /// Флаг сторнирования
        /// </summary>
        bool IsStorno { get; }

        /// <summary>
        /// Флаг покупки
        /// </summary>
        bool IsBuy { get; }

        /// <summary>
        /// Флаг печати НДС
        /// </summary>
        bool PrintNds { get; }

        /// <summary>
        /// Идентификатор гостя в заказе, для которого печататется чек.
        /// </summary>
        Guid? GuestId { get; }

        /// <summary>
        /// Позиции
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskSale> Sales { get; }

        /// <summary>
        /// Надбавки
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskDiscountItem> Increases { get; }

        /// <summary>
        /// Скидки
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskDiscountItem> Discounts { get; }

        /// <summary>
        /// Предоплаты
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskPrepayItem> Prepayments { get; }

        /// <summary>
        /// Оплаты наличными
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskPaymentItem> CashPayments { get; }

        /// <summary>
        /// Оплаты по карте
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskPaymentItem> CardPayments { get; }

        /// <summary>
        /// Дополнительные оплаты
        /// </summary>
        [NotNull]
        IEnumerable<IChequeTaskPaymentItem> AdditionalPayments { get; }

    }

    internal sealed class ChequeTask : TemplateModelBase, IChequeTask
    {
        #region Fields
        private readonly string cashierName;
        private readonly bool detailed;
        private readonly decimal cashPayment;
        private readonly decimal resultSum;
        private readonly bool isStorno;
        private readonly bool isBuy;
        private readonly bool printNds;
        private readonly Guid? guestId;
        private readonly List<ChequeTaskSale> sales = new List<ChequeTaskSale>();
        private readonly List<ChequeTaskDiscountItem> increases = new List<ChequeTaskDiscountItem>();
        private readonly List<ChequeTaskDiscountItem> discounts = new List<ChequeTaskDiscountItem>();
        private readonly List<ChequeTaskPrepayItem> prepayments = new List<ChequeTaskPrepayItem>();
        private readonly List<ChequeTaskPaymentItem> cashPayments = new List<ChequeTaskPaymentItem>();
        private readonly List<ChequeTaskPaymentItem> cardPayments = new List<ChequeTaskPaymentItem>();
        private readonly List<ChequeTaskPaymentItem> additionalPayments = new List<ChequeTaskPaymentItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChequeTask()
        {}

        private ChequeTask([NotNull] CopyContext context, [NotNull] IChequeTask src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            cashierName = src.CashierName;
            detailed = src.Detailed;
            cashPayment = src.CashPayment;
            resultSum = src.ResultSum;
            isStorno = src.IsStorno;
            isBuy = src.IsBuy;
            printNds = src.PrintNds;
            guestId = src.GuestId;
            sales = src.Sales.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskSale.Convert)).ToList();
            increases = src.Increases.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskDiscountItem.Convert)).ToList();
            discounts = src.Discounts.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskDiscountItem.Convert)).ToList();
            prepayments = src.Prepayments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskPrepayItem.Convert)).ToList();
            cashPayments = src.CashPayments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskPaymentItem.Convert)).ToList();
            cardPayments = src.CardPayments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskPaymentItem.Convert)).ToList();
            additionalPayments = src.AdditionalPayments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTaskPaymentItem.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChequeTask Convert([NotNull] CopyContext context, [CanBeNull] IChequeTask source)
        {
            if (source == null)
                return null;

            return new ChequeTask(context, source);
        }
        #endregion

        #region Props
        public string CashierName
        {
            get { return GetLocalizedValue(cashierName); }
        }

        public bool Detailed
        {
            get { return detailed; }
        }

        public decimal CashPayment
        {
            get { return cashPayment; }
        }

        public decimal ResultSum
        {
            get { return resultSum; }
        }

        public bool IsStorno
        {
            get { return isStorno; }
        }

        public bool IsBuy
        {
            get { return isBuy; }
        }

        public bool PrintNds
        {
            get { return printNds; }
        }

        public Guid? GuestId
        {
            get { return guestId; }
        }

        public IEnumerable<IChequeTaskSale> Sales
        {
            get { return sales; }
        }

        public IEnumerable<IChequeTaskDiscountItem> Increases
        {
            get { return increases; }
        }

        public IEnumerable<IChequeTaskDiscountItem> Discounts
        {
            get { return discounts; }
        }

        public IEnumerable<IChequeTaskPrepayItem> Prepayments
        {
            get { return prepayments; }
        }

        public IEnumerable<IChequeTaskPaymentItem> CashPayments
        {
            get { return cashPayments; }
        }

        public IEnumerable<IChequeTaskPaymentItem> CardPayments
        {
            get { return cardPayments; }
        }

        public IEnumerable<IChequeTaskPaymentItem> AdditionalPayments
        {
            get { return additionalPayments; }
        }

        #endregion
    }

}
