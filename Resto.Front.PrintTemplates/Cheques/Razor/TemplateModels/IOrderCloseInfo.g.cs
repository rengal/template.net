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
    /// Информация о закрытии заказа
    /// </summary>
    public interface IOrderCloseInfo
    {
        /// <summary>
        /// Кассир
        /// </summary>
        [NotNull]
        IUser Cashier { get; }

        /// <summary>
        /// Дата/время закрытия
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// Сдача
        /// </summary>
        decimal Change { get; }

        /// <summary>
        /// Транзакции оплаты
        /// </summary>
        [NotNull]
        IEnumerable<IOrderPaymentTransaction> PaymentTransactions { get; }

        /// <summary>
        /// Скидки для оплат, проведённых как скидки
        /// </summary>
        [NotNull]
        IEnumerable<IDiscountItem> DiscountsForPaymentsAsDiscount { get; }

    }

    internal sealed class OrderCloseInfo : TemplateModelBase, IOrderCloseInfo
    {
        #region Fields
        private readonly User cashier;
        private readonly DateTime time;
        private readonly decimal change;
        private readonly List<OrderPaymentTransaction> paymentTransactions = new List<OrderPaymentTransaction>();
        private readonly List<DiscountItem> discountsForPaymentsAsDiscount = new List<DiscountItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private OrderCloseInfo()
        {}

        private OrderCloseInfo([NotNull] CopyContext context, [NotNull] IOrderCloseInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            cashier = context.GetConverted(src.Cashier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            time = src.Time;
            change = src.Change;
            paymentTransactions = src.PaymentTransactions.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderPaymentTransaction.Convert)).ToList();
            discountsForPaymentsAsDiscount = src.DiscountsForPaymentsAsDiscount.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountItem.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderCloseInfo Convert([NotNull] CopyContext context, [CanBeNull] IOrderCloseInfo source)
        {
            if (source == null)
                return null;

            return new OrderCloseInfo(context, source);
        }
        #endregion

        #region Props
        public IUser Cashier
        {
            get { return cashier; }
        }

        public DateTime Time
        {
            get { return time; }
        }

        public decimal Change
        {
            get { return change; }
        }

        public IEnumerable<IOrderPaymentTransaction> PaymentTransactions
        {
            get { return paymentTransactions; }
        }

        public IEnumerable<IDiscountItem> DiscountsForPaymentsAsDiscount
        {
            get { return discountsForPaymentsAsDiscount; }
        }

        #endregion
    }

}
