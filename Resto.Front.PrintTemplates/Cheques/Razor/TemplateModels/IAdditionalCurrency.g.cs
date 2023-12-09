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

    internal sealed class AdditionalCurrency : TemplateModelBase, IAdditionalCurrency
    {
        #region Fields
        private readonly string isoName;
        private readonly string shortName;
        private readonly string shortNameForGui;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private AdditionalCurrency()
        {}

        private AdditionalCurrency([NotNull] CopyContext context, [NotNull] IAdditionalCurrency src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            isoName = src.IsoName;
            shortName = src.ShortName;
            shortNameForGui = src.ShortNameForGui;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static AdditionalCurrency Convert([NotNull] CopyContext context, [CanBeNull] IAdditionalCurrency source)
        {
            if (source == null)
                return null;

            return new AdditionalCurrency(context, source);
        }
        #endregion

        #region Props
        public string IsoName
        {
            get { return GetLocalizedValue(isoName); }
        }

        public string ShortName
        {
            get { return GetLocalizedValue(shortName); }
        }

        public string ShortNameForGui
        {
            get { return GetLocalizedValue(shortNameForGui); }
        }

        #endregion
    }

}
