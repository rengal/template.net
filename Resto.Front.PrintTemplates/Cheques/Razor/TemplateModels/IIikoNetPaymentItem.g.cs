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
    /// Элемент оплаты заказа платежной системой Plazius
    /// </summary>
    public interface IIikoNetPaymentItem : IPaymentItem
    {
        /// <summary>
        /// Скидка по маркетинговым акциям
        /// </summary>
        decimal DiscountSum { get; }

    }

    internal sealed class IikoNetPaymentItem : PaymentItem, IIikoNetPaymentItem
    {
        #region Fields
        private readonly decimal discountSum;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private IikoNetPaymentItem()
        {}

        private IikoNetPaymentItem([NotNull] CopyContext context, [NotNull] IIikoNetPaymentItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            discountSum = src.DiscountSum;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static IikoNetPaymentItem Convert([NotNull] CopyContext context, [CanBeNull] IIikoNetPaymentItem source)
        {
            if (source == null)
                return null;

            return new IikoNetPaymentItem(context, source);
        }
        #endregion

        #region Props
        public decimal DiscountSum
        {
            get { return discountSum; }
        }

        #endregion
    }

}
