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
    /// Элемент оплаты заказа без выручки
    /// </summary>
    public interface IWriteoffPaymentItem : IPaymentItem
    {
        /// <summary>
        /// Сотрудник/гость
        /// </summary>
        [CanBeNull]
        IUser Employee { get; }

    }

    internal sealed class WriteoffPaymentItem : PaymentItem, IWriteoffPaymentItem
    {
        #region Fields
        private readonly User employee;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private WriteoffPaymentItem()
        {}

        private WriteoffPaymentItem([NotNull] CopyContext context, [NotNull] IWriteoffPaymentItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            employee = context.GetConverted(src.Employee, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static WriteoffPaymentItem Convert([NotNull] CopyContext context, [CanBeNull] IWriteoffPaymentItem source)
        {
            if (source == null)
                return null;

            return new WriteoffPaymentItem(context, source);
        }
        #endregion

        #region Props
        public IUser Employee
        {
            get { return employee; }
        }

        #endregion
    }

}
