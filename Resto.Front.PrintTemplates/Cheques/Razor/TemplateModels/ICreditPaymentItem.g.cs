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
    /// Элемент оплаты заказа в кредит
    /// </summary>
    public interface ICreditPaymentItem : IPaymentItem
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        [CanBeNull]
        IUser Counteragent { get; }

    }

    internal sealed class CreditPaymentItem : PaymentItem, ICreditPaymentItem
    {
        #region Fields
        private readonly User counteragent;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CreditPaymentItem()
        {}

        private CreditPaymentItem([NotNull] CopyContext context, [NotNull] ICreditPaymentItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            counteragent = context.GetConverted(src.Counteragent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CreditPaymentItem Convert([NotNull] CopyContext context, [CanBeNull] ICreditPaymentItem source)
        {
            if (source == null)
                return null;

            return new CreditPaymentItem(context, source);
        }
        #endregion

        #region Props
        public IUser Counteragent
        {
            get { return counteragent; }
        }

        #endregion
    }

}
