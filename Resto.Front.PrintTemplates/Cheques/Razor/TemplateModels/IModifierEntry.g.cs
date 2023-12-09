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
    /// Позиция заказа (модификатор)
    /// </summary>
    public interface IModifierEntry : IOrderEntry
    {
        /// <summary>
        /// Простой модификатор
        /// </summary>
        [CanBeNull]
        ISimpleModifier SimpleModifier { get; }

        /// <summary>
        /// Дочерний модификатор
        /// </summary>
        [CanBeNull]
        IChildModifier ChildModifier { get; }

        /// <summary>
        /// Тип места приготовления
        /// </summary>
        [NotNull]
        ICookingPlaceType CookingPlaceType { get; }

        /// <summary>
        /// Место приготовления
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// Общий модификатор (для составных блюд)
        /// </summary>
        bool IsCommonModifier { get; }

    }

    internal sealed class ModifierEntry : OrderEntry, IModifierEntry
    {
        #region Fields
        private readonly SimpleModifier simpleModifier;
        private readonly ChildModifier childModifier;
        private readonly CookingPlaceType cookingPlaceType;
        private readonly RestaurantSection cookingPlace;
        private readonly bool isCommonModifier;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ModifierEntry()
        {}

        private ModifierEntry([NotNull] CopyContext context, [NotNull] IModifierEntry src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            simpleModifier = context.GetConverted(src.SimpleModifier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.SimpleModifier.Convert);
            childModifier = context.GetConverted(src.ChildModifier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChildModifier.Convert);
            cookingPlaceType = context.GetConverted(src.CookingPlaceType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CookingPlaceType.Convert);
            cookingPlace = context.GetConverted(src.CookingPlace, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
            isCommonModifier = src.IsCommonModifier;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ModifierEntry Convert([NotNull] CopyContext context, [CanBeNull] IModifierEntry source)
        {
            if (source == null)
                return null;

            return new ModifierEntry(context, source);
        }
        #endregion

        #region Props
        public ISimpleModifier SimpleModifier
        {
            get { return simpleModifier; }
        }

        public IChildModifier ChildModifier
        {
            get { return childModifier; }
        }

        public ICookingPlaceType CookingPlaceType
        {
            get { return cookingPlaceType; }
        }

        public IRestaurantSection CookingPlace
        {
            get { return cookingPlace; }
        }

        public bool IsCommonModifier
        {
            get { return isCommonModifier; }
        }

        #endregion
    }

}
