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

    internal sealed class CashPaymentType : PaymentType, ICashPaymentType
    {
        #region Fields
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CashPaymentType()
        {}

        private CashPaymentType([NotNull] CopyContext context, [NotNull] ICashPaymentType src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CashPaymentType Convert([NotNull] CopyContext context, [CanBeNull] ICashPaymentType source)
        {
            if (source == null)
                return null;

            return new CashPaymentType(context, source);
        }
        #endregion

        #region Props
        #endregion
    }

}
