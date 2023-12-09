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

    internal class NonCashPaymentType : PaymentType, INonCashPaymentType
    {
        #region Fields
        private readonly DiscountType replaceDiscount;
        #endregion

        #region Ctor
        protected NonCashPaymentType()
        {}

        protected NonCashPaymentType([NotNull] CopyContext context, [NotNull] INonCashPaymentType src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            replaceDiscount = context.GetConverted(src.ReplaceDiscount, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountType.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static NonCashPaymentType Convert([NotNull] CopyContext context, [CanBeNull] INonCashPaymentType source)
        {
            if (source == null)
                return null;

            return new NonCashPaymentType(context, source);
        }
        #endregion

        #region Props
        public IDiscountType ReplaceDiscount
        {
            get { return replaceDiscount; }
        }

        #endregion
    }

}
