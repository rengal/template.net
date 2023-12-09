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
    /// Счет-фактура
    /// </summary>
    public interface IVatInvoice : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Информация о кассовом чеке
        /// </summary>
        [NotNull]
        ICashChequeInfo ChequeInfo { get; }

        /// <summary>
        /// Номер счет-фактуры.
        /// </summary>
        [NotNull]
        string Number { get; }

        /// <summary>
        /// Дата/время счет-фактуры.
        /// </summary>
        DateTime DocumentTime { get; }

        /// <summary>
        /// Клиент.
        /// </summary>
        [NotNull]
        ICustomer Customer { get; }

        /// <summary>
        /// Информация об организации.
        /// </summary>
        [NotNull]
        IOrganizationDetailsInfo OrganizationDetails { get; }

        /// <summary>
        /// Общая сумма.
        /// </summary>
        decimal TotalSum { get; }

        /// <summary>
        /// Номер фискального чека.
        /// </summary>
        string FiscalChequeNumber { get; }

        /// <summary>
        /// Номер фискального регистратора.
        /// </summary>
        string CashRegisterNumber { get; }

        /// <summary>
        /// Заказ.
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Список налоговых категорий.
        /// </summary>
        [NotNull]
        IEnumerable<ITaxCategoryInfo> TaxCategories { get; }

        /// <summary>
        /// Позиции заказа для EInvoice.
        /// </summary>
        [NotNull]
        IEnumerable<IDetailedVatInvoiceItem> VatInvoiceItems { get; }

        /// <summary>
        /// Оплаты для EInvoice.
        /// </summary>
        [NotNull]
        IDictionary<IPaymentType, decimal> VatPayments { get; }

    }

    internal sealed class VatInvoice : TemplateModelBase, IVatInvoice
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly CashChequeInfo chequeInfo;
        private readonly string number;
        private readonly DateTime documentTime;
        private readonly Customer customer;
        private readonly OrganizationDetailsInfo organizationDetails;
        private readonly decimal totalSum;
        private readonly string fiscalChequeNumber;
        private readonly string cashRegisterNumber;
        private readonly Order order;
        private readonly List<TaxCategoryInfo> taxCategories = new List<TaxCategoryInfo>();
        private readonly List<DetailedVatInvoiceItem> vatInvoiceItems = new List<DetailedVatInvoiceItem>();
        private readonly Dictionary<PaymentType, decimal> vatPayments = new Dictionary<PaymentType, decimal>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private VatInvoice()
        {}

        internal VatInvoice([NotNull] CopyContext context, [NotNull] IVatInvoice src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            chequeInfo = context.GetConverted(src.ChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CashChequeInfo.Convert);
            number = src.Number;
            documentTime = src.DocumentTime;
            customer = context.GetConverted(src.Customer, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Customer.Convert);
            organizationDetails = context.GetConverted(src.OrganizationDetails, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrganizationDetailsInfo.Convert);
            totalSum = src.TotalSum;
            fiscalChequeNumber = src.FiscalChequeNumber;
            cashRegisterNumber = src.CashRegisterNumber;
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            taxCategories = src.TaxCategories.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.TaxCategoryInfo.Convert)).ToList();
            vatInvoiceItems = src.VatInvoiceItems.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DetailedVatInvoiceItem.Convert)).ToList();
            vatPayments = src.VatPayments.ToDictionary(kvp => context.GetConverted(kvp.Key, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert), kvp => kvp.Value);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public ICashChequeInfo ChequeInfo
        {
            get { return chequeInfo; }
        }

        public string Number
        {
            get { return GetLocalizedValue(number); }
        }

        public DateTime DocumentTime
        {
            get { return documentTime; }
        }

        public ICustomer Customer
        {
            get { return customer; }
        }

        public IOrganizationDetailsInfo OrganizationDetails
        {
            get { return organizationDetails; }
        }

        public decimal TotalSum
        {
            get { return totalSum; }
        }

        public string FiscalChequeNumber
        {
            get { return GetLocalizedValue(fiscalChequeNumber); }
        }

        public string CashRegisterNumber
        {
            get { return GetLocalizedValue(cashRegisterNumber); }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public IEnumerable<ITaxCategoryInfo> TaxCategories
        {
            get { return taxCategories; }
        }

        public IEnumerable<IDetailedVatInvoiceItem> VatInvoiceItems
        {
            get { return vatInvoiceItems; }
        }

        public IDictionary<IPaymentType, decimal> VatPayments
        {
            get { return vatPayments.ToDictionary(kvp => (IPaymentType)kvp.Key, kvp => (decimal)kvp.Value); }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IVatInvoice cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new VatInvoice(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IVatInvoice>(copy, "VatInvoice");
        }
    }
}
