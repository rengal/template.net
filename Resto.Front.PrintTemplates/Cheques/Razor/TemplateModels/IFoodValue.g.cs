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

    internal sealed class FoodValue : TemplateModelBase, IFoodValue
    {
        #region Fields
        private readonly decimal fat;
        private readonly decimal protein;
        private readonly decimal carbohydrate;
        private readonly decimal caloricity;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private FoodValue()
        {}

        private FoodValue([NotNull] CopyContext context, [NotNull] IFoodValue src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            fat = src.Fat;
            protein = src.Protein;
            carbohydrate = src.Carbohydrate;
            caloricity = src.Caloricity;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static FoodValue Convert([NotNull] CopyContext context, [CanBeNull] IFoodValue source)
        {
            if (source == null)
                return null;

            return new FoodValue(context, source);
        }
        #endregion

        #region Props
        public decimal Fat
        {
            get { return fat; }
        }

        public decimal Protein
        {
            get { return protein; }
        }

        public decimal Carbohydrate
        {
            get { return carbohydrate; }
        }

        public decimal Caloricity
        {
            get { return caloricity; }
        }

        #endregion
    }

}
