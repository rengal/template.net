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
    /// Скидка для чека
    /// </summary>
    public interface IChequeTaskDiscountItem
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Процент
        /// </summary>
        decimal Percent { get; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Номер скидочной карты
        /// </summary>
        [CanBeNull]
        string CardNumber { get; }

    }

    internal sealed class ChequeTaskDiscountItem : TemplateModelBase, IChequeTaskDiscountItem
    {
        #region Fields
        private readonly string name;
        private readonly decimal percent;
        private readonly decimal sum;
        private readonly string cardNumber;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChequeTaskDiscountItem()
        {}

        private ChequeTaskDiscountItem([NotNull] CopyContext context, [NotNull] IChequeTaskDiscountItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            percent = src.Percent;
            sum = src.Sum;
            cardNumber = src.CardNumber;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChequeTaskDiscountItem Convert([NotNull] CopyContext context, [CanBeNull] IChequeTaskDiscountItem source)
        {
            if (source == null)
                return null;

            return new ChequeTaskDiscountItem(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public decimal Percent
        {
            get { return percent; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public string CardNumber
        {
            get { return GetLocalizedValue(cardNumber); }
        }

        #endregion
    }

}
