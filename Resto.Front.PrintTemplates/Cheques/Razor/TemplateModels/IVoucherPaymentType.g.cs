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

    internal sealed class VoucherPaymentType : NonCashPaymentType, IVoucherPaymentType
    {
        #region Fields
        private readonly bool isAmountNominal;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private VoucherPaymentType()
        {}

        private VoucherPaymentType([NotNull] CopyContext context, [NotNull] IVoucherPaymentType src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            isAmountNominal = src.IsAmountNominal;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static VoucherPaymentType Convert([NotNull] CopyContext context, [CanBeNull] IVoucherPaymentType source)
        {
            if (source == null)
                return null;

            return new VoucherPaymentType(context, source);
        }
        #endregion

        #region Props
        public bool IsAmountNominal
        {
            get { return isAmountNominal; }
        }

        #endregion
    }

}
