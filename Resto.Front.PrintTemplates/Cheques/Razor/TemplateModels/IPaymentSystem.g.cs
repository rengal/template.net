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

    internal sealed class PaymentSystem : TemplateModelBase, IPaymentSystem
    {
        #region Fields
        private readonly string name;
        private readonly Product replenishProduct;
        private readonly List<Product> activationProducts = new List<Product>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PaymentSystem()
        {}

        private PaymentSystem([NotNull] CopyContext context, [NotNull] IPaymentSystem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            replenishProduct = context.GetConverted(src.ReplenishProduct, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert);
            activationProducts = src.ActivationProducts.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PaymentSystem Convert([NotNull] CopyContext context, [CanBeNull] IPaymentSystem source)
        {
            if (source == null)
                return null;

            return new PaymentSystem(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public IProduct ReplenishProduct
        {
            get { return replenishProduct; }
        }

        public IEnumerable<IProduct> ActivationProducts
        {
            get { return activationProducts; }
        }

        #endregion
    }

}
