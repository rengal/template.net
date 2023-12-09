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
    /// Элемент заказа (модификатор)
    /// </summary>
    public interface IKitchenOrderModifierItem : IKitchenOrderItem
    {
        /// <summary>
        /// Признак модификатора, который готовится отдельно от блюда
        /// </summary>
        bool IsSeparate { get; }

        /// <summary>
        /// Количество модификатора не зависит от количества блюда
        /// </summary>
        bool AmountIndependentOfParentAmount { get; }

        /// <summary>
        /// Признак «скрытого» модификатора
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Общий модификатор (для составных блюд)
        /// </summary>
        [CanBeNull]
        bool IsCommonModifier { get; }

    }

    internal sealed class KitchenOrderModifierItem : KitchenOrderItem, IKitchenOrderModifierItem
    {
        #region Fields
        private readonly bool isSeparate;
        private readonly bool amountIndependentOfParentAmount;
        private readonly bool isHidden;
        private readonly bool isCommonModifier;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private KitchenOrderModifierItem()
        {}

        private KitchenOrderModifierItem([NotNull] CopyContext context, [NotNull] IKitchenOrderModifierItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            isSeparate = src.IsSeparate;
            amountIndependentOfParentAmount = src.AmountIndependentOfParentAmount;
            isHidden = src.IsHidden;
            isCommonModifier = src.IsCommonModifier;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static KitchenOrderModifierItem Convert([NotNull] CopyContext context, [CanBeNull] IKitchenOrderModifierItem source)
        {
            if (source == null)
                return null;

            return new KitchenOrderModifierItem(context, source);
        }
        #endregion

        #region Props
        public bool IsSeparate
        {
            get { return isSeparate; }
        }

        public bool AmountIndependentOfParentAmount
        {
            get { return amountIndependentOfParentAmount; }
        }

        public bool IsHidden
        {
            get { return isHidden; }
        }

        public bool IsCommonModifier
        {
            get { return isCommonModifier; }
        }

        #endregion
    }

}
