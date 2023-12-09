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

    internal sealed class ProductTag : TemplateModelBase, IProductTag
    {
        #region Fields
        private readonly string value;
        private readonly ProductTagGroup group;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductTag()
        {}

        private ProductTag([NotNull] CopyContext context, [NotNull] IProductTag src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            value = src.Value;
            group = context.GetConverted(src.Group, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductTagGroup.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ProductTag Convert([NotNull] CopyContext context, [CanBeNull] IProductTag source)
        {
            if (source == null)
                return null;

            return new ProductTag(context, source);
        }
        #endregion

        #region Props
        public string Value
        {
            get { return GetLocalizedValue(value); }
        }

        public IProductTagGroup Group
        {
            get { return group; }
        }

        #endregion
    }

}
