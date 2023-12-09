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
    /// Позиция чека
    /// </summary>
    public interface IChequeTaskSale
    {
        /// <summary>
        /// Название позиции
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Артикул
        /// </summary>
        [NotNull]
        string Article { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Сумма позиции чека (с учетом всех скидок, надбавок и НДС)
        /// </summary>
        decimal ResultSum { get; }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        decimal NdsPercent { get; }

        /// <summary>
        /// Процент надбавки
        /// </summary>
        decimal IncreasePercent { get; }

        /// <summary>
        /// Сумма надбавки
        /// </summary>
        decimal IncreaseSum { get; }

        /// <summary>
        /// Процент скидки
        /// </summary>
        decimal DiscountPercent { get; }

        /// <summary>
        /// Сумма скидки
        /// </summary>
        decimal DiscountSum { get; }

        /// <summary>
        /// Флаг отдельный НДС(Клиент сам оплачивает НДС)
        /// </summary>
        bool IsSplitVat { get; }

    }

    internal sealed class ChequeTaskSale : TemplateModelBase, IChequeTaskSale
    {
        #region Fields
        private readonly string name;
        private readonly string article;
        private readonly decimal amount;
        private readonly decimal price;
        private readonly decimal resultSum;
        private readonly decimal ndsPercent;
        private readonly decimal increasePercent;
        private readonly decimal increaseSum;
        private readonly decimal discountPercent;
        private readonly decimal discountSum;
        private readonly bool isSplitVat;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChequeTaskSale()
        {}

        private ChequeTaskSale([NotNull] CopyContext context, [NotNull] IChequeTaskSale src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            article = src.Article;
            amount = src.Amount;
            price = src.Price;
            resultSum = src.ResultSum;
            ndsPercent = src.NdsPercent;
            increasePercent = src.IncreasePercent;
            increaseSum = src.IncreaseSum;
            discountPercent = src.DiscountPercent;
            discountSum = src.DiscountSum;
            isSplitVat = src.IsSplitVat;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChequeTaskSale Convert([NotNull] CopyContext context, [CanBeNull] IChequeTaskSale source)
        {
            if (source == null)
                return null;

            return new ChequeTaskSale(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string Article
        {
            get { return GetLocalizedValue(article); }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Price
        {
            get { return price; }
        }

        public decimal ResultSum
        {
            get { return resultSum; }
        }

        public decimal NdsPercent
        {
            get { return ndsPercent; }
        }

        public decimal IncreasePercent
        {
            get { return increasePercent; }
        }

        public decimal IncreaseSum
        {
            get { return increaseSum; }
        }

        public decimal DiscountPercent
        {
            get { return discountPercent; }
        }

        public decimal DiscountSum
        {
            get { return discountSum; }
        }

        public bool IsSplitVat
        {
            get { return isSplitVat; }
        }

        #endregion
    }

}
