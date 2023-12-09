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
    /// Транзакция оплаты заказа
    /// </summary>
    public interface IOrderPaymentTransaction
    {
        /// <summary>
        /// Сумма транзакции
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        IPaymentType PaymentType { get; }

        /// <summary>
        /// Флаг транзакции закрытия предоплаты
        /// </summary>
        bool IsPrepayClosedTransaction { get; }

        /// <summary>
        /// Контрагент при оплате в кредит
        /// </summary>
        [CanBeNull]
        IUser CreditCounteragent { get; }

    }

    internal sealed class OrderPaymentTransaction : TemplateModelBase, IOrderPaymentTransaction
    {
        #region Fields
        private readonly decimal sum;
        private readonly PaymentType paymentType;
        private readonly bool isPrepayClosedTransaction;
        private readonly User creditCounteragent;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private OrderPaymentTransaction()
        {}

        private OrderPaymentTransaction([NotNull] CopyContext context, [NotNull] IOrderPaymentTransaction src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sum = src.Sum;
            paymentType = context.GetConverted(src.PaymentType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert);
            isPrepayClosedTransaction = src.IsPrepayClosedTransaction;
            creditCounteragent = context.GetConverted(src.CreditCounteragent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderPaymentTransaction Convert([NotNull] CopyContext context, [CanBeNull] IOrderPaymentTransaction source)
        {
            if (source == null)
                return null;

            return new OrderPaymentTransaction(context, source);
        }
        #endregion

        #region Props
        public decimal Sum
        {
            get { return sum; }
        }

        public IPaymentType PaymentType
        {
            get { return paymentType; }
        }

        public bool IsPrepayClosedTransaction
        {
            get { return isPrepayClosedTransaction; }
        }

        public IUser CreditCounteragent
        {
            get { return creditCounteragent; }
        }

        #endregion
    }

}
