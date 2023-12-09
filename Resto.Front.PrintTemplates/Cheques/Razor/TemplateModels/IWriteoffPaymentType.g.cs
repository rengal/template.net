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

    internal sealed class WriteoffPaymentType : PaymentType, IWriteoffPaymentType
    {
        #region Fields
        private readonly Account account;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private WriteoffPaymentType()
        {}

        private WriteoffPaymentType([NotNull] CopyContext context, [NotNull] IWriteoffPaymentType src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            account = context.GetConverted(src.Account, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Account.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static WriteoffPaymentType Convert([NotNull] CopyContext context, [CanBeNull] IWriteoffPaymentType source)
        {
            if (source == null)
                return null;

            return new WriteoffPaymentType(context, source);
        }
        #endregion

        #region Props
        public IAccount Account
        {
            get { return account; }
        }

        #endregion
    }

}
