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
    /// Элемент заказа, закрытого в прошлые кассовые смены
    /// </summary>
    public interface IPastOrderItem
    {
        /// <summary>
        /// Блюдо/продукт/услуга
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Размер
        /// </summary>
        [CanBeNull]
        IProductItemSize ProductSize { get; }

        /// <summary>
        /// Стоимость с учётом скидок/надбавок
        /// </summary>
        decimal SumWithDiscounts { get; }

        /// <summary>
        /// Стоимость без учёта скидок/надбавок
        /// </summary>
        decimal SumWithoutDiscounts { get; }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        decimal VatRate { get; }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        decimal VatSum { get; }

    }

    internal sealed class PastOrderItem : TemplateModelBase, IPastOrderItem
    {
        #region Fields
        private readonly Product product;
        private readonly decimal amount;
        private readonly decimal price;
        private readonly ProductItemSize productSize;
        private readonly decimal sumWithDiscounts;
        private readonly decimal sumWithoutDiscounts;
        private readonly decimal vatRate;
        private readonly decimal vatSum;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PastOrderItem()
        {}

        private PastOrderItem([NotNull] CopyContext context, [NotNull] IPastOrderItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            product = context.GetConverted(src.Product, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert);
            amount = src.Amount;
            price = src.Price;
            productSize = context.GetConverted(src.ProductSize, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductItemSize.Convert);
            sumWithDiscounts = src.SumWithDiscounts;
            sumWithoutDiscounts = src.SumWithoutDiscounts;
            vatRate = src.VatRate;
            vatSum = src.VatSum;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PastOrderItem Convert([NotNull] CopyContext context, [CanBeNull] IPastOrderItem source)
        {
            if (source == null)
                return null;

            return new PastOrderItem(context, source);
        }
        #endregion

        #region Props
        public IProduct Product
        {
            get { return product; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Price
        {
            get { return price; }
        }

        public IProductItemSize ProductSize
        {
            get { return productSize; }
        }

        public decimal SumWithDiscounts
        {
            get { return sumWithDiscounts; }
        }

        public decimal SumWithoutDiscounts
        {
            get { return sumWithoutDiscounts; }
        }

        public decimal VatRate
        {
            get { return vatRate; }
        }

        public decimal VatSum
        {
            get { return vatSum; }
        }

        #endregion
    }

}
