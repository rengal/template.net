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

    internal sealed class ProductGroup : TemplateModelBase, IProductGroup
    {
        #region Fields
        private readonly string name;
        private readonly string article;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductGroup()
        {}

        private ProductGroup([NotNull] CopyContext context, [NotNull] IProductGroup src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            article = src.Article;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ProductGroup Convert([NotNull] CopyContext context, [CanBeNull] IProductGroup source)
        {
            if (source == null)
                return null;

            return new ProductGroup(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string Article
        {
            get { return GetLocalizedValue(article); }
        }

        #endregion
    }

}
