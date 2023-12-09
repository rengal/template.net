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

    internal sealed class Card : TemplateModelBase, ICard
    {
        #region Fields
        private readonly string slipText;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Card()
        {}

        private Card([NotNull] CopyContext context, [NotNull] ICard src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            slipText = src.SlipText;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Card Convert([NotNull] CopyContext context, [CanBeNull] ICard source)
        {
            if (source == null)
                return null;

            return new Card(context, source);
        }
        #endregion

        #region Props
        public string SlipText
        {
            get { return GetLocalizedValue(slipText); }
        }

        #endregion
    }

}
