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
    /// Элемент предоплаты для чека
    /// </summary>
    public interface IChequeTaskPrepayItem
    {
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Название типа оплаты
        /// </summary>
        [NotNull]
        string PaymentTypeName { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Стандарное название валюты (ISO). null, если оплата в основной валюте
        /// </summary>
        [CanBeNull]
        string CurrencyName { get; }

        /// <summary>
        /// Сумма в валюте. 0, если оплата в основной валюте
        /// </summary>
        decimal SumInCurrency { get; }

    }

    internal sealed class ChequeTaskPrepayItem : TemplateModelBase, IChequeTaskPrepayItem
    {
        #region Fields
        private readonly decimal sum;
        private readonly string paymentTypeName;
        private readonly string comment;
        private readonly string currencyName;
        private readonly decimal sumInCurrency;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChequeTaskPrepayItem()
        {}

        private ChequeTaskPrepayItem([NotNull] CopyContext context, [NotNull] IChequeTaskPrepayItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sum = src.Sum;
            paymentTypeName = src.PaymentTypeName;
            comment = src.Comment;
            currencyName = src.CurrencyName;
            sumInCurrency = src.SumInCurrency;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChequeTaskPrepayItem Convert([NotNull] CopyContext context, [CanBeNull] IChequeTaskPrepayItem source)
        {
            if (source == null)
                return null;

            return new ChequeTaskPrepayItem(context, source);
        }
        #endregion

        #region Props
        public decimal Sum
        {
            get { return sum; }
        }

        public string PaymentTypeName
        {
            get { return GetLocalizedValue(paymentTypeName); }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public string CurrencyName
        {
            get { return GetLocalizedValue(currencyName); }
        }

        public decimal SumInCurrency
        {
            get { return sumInCurrency; }
        }

        #endregion
    }

}
