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
    /// Элемент оплаты для чека
    /// </summary>
    public interface IChequeTaskPaymentItem
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
        [NotNull]
        string Comment { get; }

        /// <summary>
        /// Признак фискальности
        /// </summary>
        bool IsFiscal { get; }

        /// <summary>
        /// Номер чека
        /// </summary>
        int ChequeNumber { get; }

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

    internal sealed class ChequeTaskPaymentItem : TemplateModelBase, IChequeTaskPaymentItem
    {
        #region Fields
        private readonly decimal sum;
        private readonly string paymentTypeName;
        private readonly string comment;
        private readonly bool isFiscal;
        private readonly int chequeNumber;
        private readonly string currencyName;
        private readonly decimal sumInCurrency;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChequeTaskPaymentItem()
        {}

        private ChequeTaskPaymentItem([NotNull] CopyContext context, [NotNull] IChequeTaskPaymentItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sum = src.Sum;
            paymentTypeName = src.PaymentTypeName;
            comment = src.Comment;
            isFiscal = src.IsFiscal;
            chequeNumber = src.ChequeNumber;
            currencyName = src.CurrencyName;
            sumInCurrency = src.SumInCurrency;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChequeTaskPaymentItem Convert([NotNull] CopyContext context, [CanBeNull] IChequeTaskPaymentItem source)
        {
            if (source == null)
                return null;

            return new ChequeTaskPaymentItem(context, source);
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

        public bool IsFiscal
        {
            get { return isFiscal; }
        }

        public int ChequeNumber
        {
            get { return chequeNumber; }
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
