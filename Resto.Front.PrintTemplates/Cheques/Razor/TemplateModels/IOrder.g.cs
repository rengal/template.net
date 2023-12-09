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
    /// Заказ
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid OrderId { get; }

        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Название таба
        /// </summary>
        [CanBeNull]
        string TabName { get; }

        /// <summary>
        /// Название источника заказа
        /// </summary>
        [CanBeNull]
        string OriginName { get; }

        /// <summary>
        /// Дата/время открытия
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Тип заказа
        /// </summary>
        [CanBeNull]
        IOrderType Type { get; }

        /// <summary>
        /// Стол
        /// </summary>
        [NotNull]
        ITable Table { get; }

        /// <summary>
        /// Официант
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Номинальное количество гостей в заказе
        /// </summary>
        int InitialGuestsCount { get; }

        /// <summary>
        /// Ценовая категория (назначенная вручную или по карте)
        /// </summary>
        [CanBeNull]
        IPriceCategory PriceCategory { get; }

        /// <summary>
        /// Ценовая категория отделения по умолчанию
        /// </summary>
        [CanBeNull]
        IPriceCategory DefaultPriceCategory { get; }

        /// <summary>
        /// Информация о закрытии заказа
        /// </summary>
        [CanBeNull]
        IOrderCloseInfo CloseInfo { get; }

        /// <summary>
        /// Резерв/банкет
        /// </summary>
        [CanBeNull]
        IReserve Reserve { get; }

        /// <summary>
        /// Доставка
        /// </summary>
        [CanBeNull]
        IDelivery Delivery { get; }

        /// <summary>
        /// Привязка заказа к карте клиента («Карта на входе»)
        /// </summary>
        [CanBeNull]
        IClientBinding ClientBinding { get; }

        /// <summary>
        /// Время последнего изменения
        /// </summary>
        DateTime LastModifiedTime { get; }

        /// <summary>
        /// Номер данного заказа во внешней системе (на сайте агрегатора и т.п.)
        /// </summary>
        [CanBeNull]
        string ExternalNumber { get; }

        /// <summary>
        /// Гости
        /// </summary>
        [NotNull]
        IEnumerable<IGuest> Guests { get; }

        /// <summary>
        /// Оплаты
        /// </summary>
        [NotNull]
        IEnumerable<IPaymentItem> Payments { get; }

        /// <summary>
        /// Предоплаты
        /// </summary>
        [NotNull]
        IEnumerable<IPaymentItem> PrePayments { get; }

        /// <summary>
        /// Чаевые
        /// </summary>
        [NotNull]
        IEnumerable<IPaymentItem> Donations { get; }

        /// <summary>
        /// Скидки, применённые к заказу
        /// </summary>
        [NotNull]
        IEnumerable<IDiscountItem> DiscountItems { get; }

        /// <summary>
        /// Список клиентов
        /// </summary>
        [NotNull]
        IEnumerable<ICustomer> Customers { get; }

        /// <summary>
        /// Зафиксированные курсы валют
        /// </summary>
        [NotNull]
        IDictionary<IAdditionalCurrency, decimal> FixedCurrencyRates { get; }

        /// <summary>
        /// Общедоступные внешние данные, хранимые API-плагинами в заказе.
        /// </summary>
        [NotNull]
        IDictionary<string, string> ExternalData { get; }

    }

    internal sealed class Order : TemplateModelBase, IOrder
    {
        #region Fields
        private readonly Guid orderId;
        private readonly int number;
        private readonly string tabName;
        private readonly string originName;
        private readonly DateTime openTime;
        private readonly OrderType type;
        private readonly Table table;
        private readonly User waiter;
        private readonly int initialGuestsCount;
        private readonly PriceCategory priceCategory;
        private readonly PriceCategory defaultPriceCategory;
        private readonly OrderCloseInfo closeInfo;
        private readonly Reserve reserve;
        private readonly Delivery delivery;
        private readonly ClientBinding clientBinding;
        private readonly DateTime lastModifiedTime;
        private readonly string externalNumber;
        private readonly List<Guest> guests = new List<Guest>();
        private readonly List<PaymentItem> payments = new List<PaymentItem>();
        private readonly List<PaymentItem> prePayments = new List<PaymentItem>();
        private readonly List<PaymentItem> donations = new List<PaymentItem>();
        private readonly List<DiscountItem> discountItems = new List<DiscountItem>();
        private readonly List<Customer> customers = new List<Customer>();
        private readonly Dictionary<AdditionalCurrency, decimal> fixedCurrencyRates = new Dictionary<AdditionalCurrency, decimal>();
        private readonly Dictionary<string, string> externalData = new Dictionary<string, string>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Order()
        {}

        private Order([NotNull] CopyContext context, [NotNull] IOrder src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            orderId = src.OrderId;
            number = src.Number;
            tabName = src.TabName;
            originName = src.OriginName;
            openTime = src.OpenTime;
            type = context.GetConverted(src.Type, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderType.Convert);
            table = context.GetConverted(src.Table, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Table.Convert);
            waiter = context.GetConverted(src.Waiter, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            initialGuestsCount = src.InitialGuestsCount;
            priceCategory = context.GetConverted(src.PriceCategory, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PriceCategory.Convert);
            defaultPriceCategory = context.GetConverted(src.DefaultPriceCategory, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PriceCategory.Convert);
            closeInfo = context.GetConverted(src.CloseInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderCloseInfo.Convert);
            reserve = context.GetConverted(src.Reserve, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Reserve.Convert);
            delivery = context.GetConverted(src.Delivery, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Delivery.Convert);
            clientBinding = context.GetConverted(src.ClientBinding, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ClientBinding.Convert);
            lastModifiedTime = src.LastModifiedTime;
            externalNumber = src.ExternalNumber;
            guests = src.Guests.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Guest.Convert)).ToList();
            payments = src.Payments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentItem.Convert)).ToList();
            prePayments = src.PrePayments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentItem.Convert)).ToList();
            donations = src.Donations.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentItem.Convert)).ToList();
            discountItems = src.DiscountItems.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountItem.Convert)).ToList();
            customers = src.Customers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Customer.Convert)).ToList();
            fixedCurrencyRates = src.FixedCurrencyRates.ToDictionary(kvp => context.GetConverted(kvp.Key, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.AdditionalCurrency.Convert), kvp => kvp.Value);
            externalData = new Dictionary<string, string>(src.ExternalData);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Order Convert([NotNull] CopyContext context, [CanBeNull] IOrder source)
        {
            if (source == null)
                return null;

            return new Order(context, source);
        }
        #endregion

        #region Props
        public Guid OrderId
        {
            get { return orderId; }
        }

        public int Number
        {
            get { return number; }
        }

        public string TabName
        {
            get { return GetLocalizedValue(tabName); }
        }

        public string OriginName
        {
            get { return GetLocalizedValue(originName); }
        }

        public DateTime OpenTime
        {
            get { return openTime; }
        }

        public IOrderType Type
        {
            get { return type; }
        }

        public ITable Table
        {
            get { return table; }
        }

        public IUser Waiter
        {
            get { return waiter; }
        }

        public int InitialGuestsCount
        {
            get { return initialGuestsCount; }
        }

        public IPriceCategory PriceCategory
        {
            get { return priceCategory; }
        }

        public IPriceCategory DefaultPriceCategory
        {
            get { return defaultPriceCategory; }
        }

        public IOrderCloseInfo CloseInfo
        {
            get { return closeInfo; }
        }

        public IReserve Reserve
        {
            get { return reserve; }
        }

        public IDelivery Delivery
        {
            get { return delivery; }
        }

        public IClientBinding ClientBinding
        {
            get { return clientBinding; }
        }

        public DateTime LastModifiedTime
        {
            get { return lastModifiedTime; }
        }

        public string ExternalNumber
        {
            get { return GetLocalizedValue(externalNumber); }
        }

        public IEnumerable<IGuest> Guests
        {
            get { return guests; }
        }

        public IEnumerable<IPaymentItem> Payments
        {
            get { return payments; }
        }

        public IEnumerable<IPaymentItem> PrePayments
        {
            get { return prePayments; }
        }

        public IEnumerable<IPaymentItem> Donations
        {
            get { return donations; }
        }

        public IEnumerable<IDiscountItem> DiscountItems
        {
            get { return discountItems; }
        }

        public IEnumerable<ICustomer> Customers
        {
            get { return customers; }
        }

        public IDictionary<IAdditionalCurrency, decimal> FixedCurrencyRates
        {
            get { return fixedCurrencyRates.ToDictionary(kvp => (IAdditionalCurrency)kvp.Key, kvp => (decimal)kvp.Value); }
        }

        public IDictionary<string, string> ExternalData
        {
            get { return externalData; }
        }

        #endregion
    }

}
