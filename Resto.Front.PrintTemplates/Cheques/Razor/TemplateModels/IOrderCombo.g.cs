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
    /// Комбо
    /// </summary>
    public interface IOrderCombo
    {
        /// <summary>
        /// Имя
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Величина для сортировки комбо в заказе
        /// </summary>
        int OrderRank { get; }

        /// <summary>
        /// Цена комбо
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Кол-во комбо
        /// </summary>
        int Amount { get; }

    }

    internal sealed class OrderCombo : TemplateModelBase, IOrderCombo
    {
        #region Fields
        private readonly string name;
        private readonly int orderRank;
        private readonly decimal price;
        private readonly int amount;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private OrderCombo()
        {}

        private OrderCombo([NotNull] CopyContext context, [NotNull] IOrderCombo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            orderRank = src.OrderRank;
            price = src.Price;
            amount = src.Amount;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderCombo Convert([NotNull] CopyContext context, [CanBeNull] IOrderCombo source)
        {
            if (source == null)
                return null;

            return new OrderCombo(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public int OrderRank
        {
            get { return orderRank; }
        }

        public decimal Price
        {
            get { return price; }
        }

        public int Amount
        {
            get { return amount; }
        }

        #endregion
    }

}
