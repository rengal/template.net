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

    internal sealed class ProductItemSize : TemplateModelBase, IProductItemSize
    {
        #region Fields
        private readonly string name;
        private readonly string kitchenName;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductItemSize()
        {}

        private ProductItemSize([NotNull] CopyContext context, [NotNull] IProductItemSize src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            kitchenName = src.KitchenName;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ProductItemSize Convert([NotNull] CopyContext context, [CanBeNull] IProductItemSize source)
        {
            if (source == null)
                return null;

            return new ProductItemSize(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string KitchenName
        {
            get { return GetLocalizedValue(kitchenName); }
        }

        #endregion
    }

}
