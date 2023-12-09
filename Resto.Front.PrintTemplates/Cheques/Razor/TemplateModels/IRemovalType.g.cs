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

    internal sealed class RemovalType : TemplateModelBase, IRemovalType
    {
        #region Fields
        private readonly string name;
        private readonly Account account;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private RemovalType()
        {}

        private RemovalType([NotNull] CopyContext context, [NotNull] IRemovalType src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            account = context.GetConverted(src.Account, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Account.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static RemovalType Convert([NotNull] CopyContext context, [CanBeNull] IRemovalType source)
        {
            if (source == null)
                return null;

            return new RemovalType(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public IAccount Account
        {
            get { return account; }
        }

        #endregion
    }

}
