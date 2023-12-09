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
    /// Дочерний модификатор
    /// </summary>
    public interface IChildModifier
    {
        /// <summary>
        /// Минимальное количество
        /// </summary>
        int MinimumAmount { get; }

        /// <summary>
        /// Максимальное количество
        /// </summary>
        int MaximumAmount { get; }

        /// <summary>
        /// Продукт
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Место приготовления
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// Количество по умолчанию
        /// </summary>
        int DefaultAmount { get; }

        /// <summary>
        /// Количество модификатора, добавленного в заказ, не зависит от количества блюда
        /// </summary>
        bool AmountIndependentOfParentAmount { get; }

        /// <summary>
        /// Общий модификатор (для составных блюд)
        /// </summary>
        bool IsCommonModifier { get; }

        /// <summary>
        /// Скрывать, если количество по умолчанию
        /// </summary>
        bool HideIfDefaultAmount { get; }

    }

    internal sealed class ChildModifier : TemplateModelBase, IChildModifier
    {
        #region Fields
        private readonly int minimumAmount;
        private readonly int maximumAmount;
        private readonly Product product;
        private readonly RestaurantSection cookingPlace;
        private readonly int defaultAmount;
        private readonly bool amountIndependentOfParentAmount;
        private readonly bool isCommonModifier;
        private readonly bool hideIfDefaultAmount;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChildModifier()
        {}

        private ChildModifier([NotNull] CopyContext context, [NotNull] IChildModifier src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            minimumAmount = src.MinimumAmount;
            maximumAmount = src.MaximumAmount;
            product = context.GetConverted(src.Product, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert);
            cookingPlace = context.GetConverted(src.CookingPlace, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
            defaultAmount = src.DefaultAmount;
            amountIndependentOfParentAmount = src.AmountIndependentOfParentAmount;
            isCommonModifier = src.IsCommonModifier;
            hideIfDefaultAmount = src.HideIfDefaultAmount;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChildModifier Convert([NotNull] CopyContext context, [CanBeNull] IChildModifier source)
        {
            if (source == null)
                return null;

            return new ChildModifier(context, source);
        }
        #endregion

        #region Props
        public int MinimumAmount
        {
            get { return minimumAmount; }
        }

        public int MaximumAmount
        {
            get { return maximumAmount; }
        }

        public IProduct Product
        {
            get { return product; }
        }

        public IRestaurantSection CookingPlace
        {
            get { return cookingPlace; }
        }

        public int DefaultAmount
        {
            get { return defaultAmount; }
        }

        public bool AmountIndependentOfParentAmount
        {
            get { return amountIndependentOfParentAmount; }
        }

        public bool IsCommonModifier
        {
            get { return isCommonModifier; }
        }

        public bool HideIfDefaultAmount
        {
            get { return hideIfDefaultAmount; }
        }

        #endregion
    }

}
