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

    internal sealed class ConfigurablePaymentType : PaymentType, IConfigurablePaymentType
    {
        #region Fields
        private readonly PaymentType basePaymentType;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ConfigurablePaymentType()
        {}

        private ConfigurablePaymentType([NotNull] CopyContext context, [NotNull] IConfigurablePaymentType src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            basePaymentType = context.GetConverted(src.BasePaymentType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ConfigurablePaymentType Convert([NotNull] CopyContext context, [CanBeNull] IConfigurablePaymentType source)
        {
            if (source == null)
                return null;

            return new ConfigurablePaymentType(context, source);
        }
        #endregion

        #region Props
        public IPaymentType BasePaymentType
        {
            get { return basePaymentType; }
        }

        #endregion
    }

}
