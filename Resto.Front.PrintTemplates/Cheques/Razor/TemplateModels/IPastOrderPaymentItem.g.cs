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
    /// Элемент оплаты заказа, закрытого в прошлые кассовые смены
    /// </summary>
    public interface IPastOrderPaymentItem
    {
        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        IPaymentType Type { get; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Флаг предоплаты
        /// </summary>
        bool IsPrepay { get; }

        /// <summary>
        /// Флаг, является ли оплата чаевыми
        /// </summary>
        bool IsDonation { get; }

    }

    internal sealed class PastOrderPaymentItem : TemplateModelBase, IPastOrderPaymentItem
    {
        #region Fields
        private readonly PaymentType type;
        private readonly decimal sum;
        private readonly bool isPrepay;
        private readonly bool isDonation;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PastOrderPaymentItem()
        {}

        private PastOrderPaymentItem([NotNull] CopyContext context, [NotNull] IPastOrderPaymentItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            type = context.GetConverted(src.Type, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert);
            sum = src.Sum;
            isPrepay = src.IsPrepay;
            isDonation = src.IsDonation;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PastOrderPaymentItem Convert([NotNull] CopyContext context, [CanBeNull] IPastOrderPaymentItem source)
        {
            if (source == null)
                return null;

            return new PastOrderPaymentItem(context, source);
        }
        #endregion

        #region Props
        public IPaymentType Type
        {
            get { return type; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public bool IsPrepay
        {
            get { return isPrepay; }
        }

        public bool IsDonation
        {
            get { return isDonation; }
        }

        #endregion
    }

}
