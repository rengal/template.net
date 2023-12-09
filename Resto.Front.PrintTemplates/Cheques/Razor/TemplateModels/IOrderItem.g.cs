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
    /// Элемент заказа
    /// </summary>
    public interface IOrderItem : IOrderEntry
    {
        /// <summary>
        /// Величина для сортировки элементов заказа внутри гостя
        /// </summary>
        int OrderRank { get; }

        /// <summary>
        /// Комбо
        /// </summary>
        IOrderCombo Combo { get; }

        /// <summary>
        /// Официант, добавивший или изменивший блюдо
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

    }

    internal abstract class OrderItem : OrderEntry, IOrderItem
    {
        #region Fields
        private readonly int orderRank;
        private readonly OrderCombo combo;
        private readonly User waiter;
        #endregion

        #region Ctor
        protected OrderItem()
        {}

        protected OrderItem([NotNull] CopyContext context, [NotNull] IOrderItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            orderRank = src.OrderRank;
            combo = context.GetConverted(src.Combo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderCombo.Convert);
            waiter = context.GetConverted(src.Waiter, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderItem Convert([NotNull] CopyContext context, [CanBeNull] IOrderItem source)
        {
            if (source == null)
                return null;

            if (source is IProductItem)
                return ProductItem.Convert(context, (IProductItem)source);
            else if (source is ITimePayServiceItem)
                return TimePayServiceItem.Convert(context, (ITimePayServiceItem)source);
            else
                throw new ArgumentException(string.Format("Type {0} not supported", source.GetType()), "source");
        }
        #endregion

        #region Props
        public int OrderRank
        {
            get { return orderRank; }
        }

        public IOrderCombo Combo
        {
            get { return combo; }
        }

        public IUser Waiter
        {
            get { return waiter; }
        }

        #endregion
    }

}
