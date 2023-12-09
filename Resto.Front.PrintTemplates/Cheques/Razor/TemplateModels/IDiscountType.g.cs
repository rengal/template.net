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

    internal sealed class DiscountType : TemplateModelBase, IDiscountType
    {
        #region Fields
        private readonly string name;
        private readonly string printableName;
        private readonly bool printProductItemInPrecheque;
        private readonly bool printDetailedInPrecheque;
        private readonly bool discountBySum;
        private readonly bool isIikoCard51DiscountType;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DiscountType()
        {}

        private DiscountType([NotNull] CopyContext context, [NotNull] IDiscountType src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            printableName = src.PrintableName;
            printProductItemInPrecheque = src.PrintProductItemInPrecheque;
            printDetailedInPrecheque = src.PrintDetailedInPrecheque;
            discountBySum = src.DiscountBySum;
            isIikoCard51DiscountType = src.IsIikoCard51DiscountType;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DiscountType Convert([NotNull] CopyContext context, [CanBeNull] IDiscountType source)
        {
            if (source == null)
                return null;

            return new DiscountType(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string PrintableName
        {
            get { return GetLocalizedValue(printableName); }
        }

        public bool PrintProductItemInPrecheque
        {
            get { return printProductItemInPrecheque; }
        }

        public bool PrintDetailedInPrecheque
        {
            get { return printDetailedInPrecheque; }
        }

        public bool DiscountBySum
        {
            get { return discountBySum; }
        }

        public bool IsIikoCard51DiscountType
        {
            get { return isIikoCard51DiscountType; }
        }

        #endregion
    }

}
