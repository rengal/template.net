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
    /// Блюда для печати в чеке об окончании приготовления блюда
    /// </summary>
    public interface ICookingCompleteChequeItem
    {
        /// <summary>
        /// Элемент заказа, для которого печатается чек
        /// </summary>
        [NotNull]
        IKitchenOrderItem Item { get; }

        /// <summary>
        /// Правая половинка разделенной пиццы, для которой печатается чек
        /// </summary>
        [CanBeNull]
        IKitchenOrderProductItem CompoundItemSecondaryComponent { get; }

    }

    internal sealed class CookingCompleteChequeItem : TemplateModelBase, ICookingCompleteChequeItem
    {
        #region Fields
        private readonly KitchenOrderItem item;
        private readonly KitchenOrderProductItem compoundItemSecondaryComponent;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CookingCompleteChequeItem()
        {}

        private CookingCompleteChequeItem([NotNull] CopyContext context, [NotNull] ICookingCompleteChequeItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            item = context.GetConverted(src.Item, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderItem.Convert);
            compoundItemSecondaryComponent = context.GetConverted(src.CompoundItemSecondaryComponent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderProductItem.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CookingCompleteChequeItem Convert([NotNull] CopyContext context, [CanBeNull] ICookingCompleteChequeItem source)
        {
            if (source == null)
                return null;

            return new CookingCompleteChequeItem(context, source);
        }
        #endregion

        #region Props
        public IKitchenOrderItem Item
        {
            get { return item; }
        }

        public IKitchenOrderProductItem CompoundItemSecondaryComponent
        {
            get { return compoundItemSecondaryComponent; }
        }

        #endregion
    }

}
