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

    internal sealed class PayInOutType : TemplateModelBase, IPayInOutType
    {
        #region Fields
        private readonly string name;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PayInOutType()
        {}

        private PayInOutType([NotNull] CopyContext context, [NotNull] IPayInOutType src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PayInOutType Convert([NotNull] CopyContext context, [CanBeNull] IPayInOutType source)
        {
            if (source == null)
                return null;

            return new PayInOutType(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        #endregion
    }

}
