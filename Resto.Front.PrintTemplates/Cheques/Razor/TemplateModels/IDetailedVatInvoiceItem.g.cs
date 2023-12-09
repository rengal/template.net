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
    /// Позиция заказа для EInvoice
    /// </summary>
    public interface IDetailedVatInvoiceItem
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        decimal VatRate { get; }

        /// <summary>
        /// Сумма с НДС
        /// </summary>
        decimal SumWithVat { get; }

    }

    internal sealed class DetailedVatInvoiceItem : TemplateModelBase, IDetailedVatInvoiceItem
    {
        #region Fields
        private readonly string name;
        private readonly decimal amount;
        private readonly decimal sum;
        private readonly decimal vatRate;
        private readonly decimal sumWithVat;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DetailedVatInvoiceItem()
        {}

        private DetailedVatInvoiceItem([NotNull] CopyContext context, [NotNull] IDetailedVatInvoiceItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            amount = src.Amount;
            sum = src.Sum;
            vatRate = src.VatRate;
            sumWithVat = src.SumWithVat;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DetailedVatInvoiceItem Convert([NotNull] CopyContext context, [CanBeNull] IDetailedVatInvoiceItem source)
        {
            if (source == null)
                return null;

            return new DetailedVatInvoiceItem(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public decimal VatRate
        {
            get { return vatRate; }
        }

        public decimal SumWithVat
        {
            get { return sumWithVat; }
        }

        #endregion
    }

}
