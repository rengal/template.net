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
    /// <summary>
    /// Чек подачи блюд курса
    /// </summary>
    public interface IProductsServeCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Позиции заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IOrderEntry> Entries { get; }

    }

    internal sealed class ProductsServeCheque : ServiceChequeBase, IProductsServeCheque
    {
        #region Fields
        private readonly List<OrderEntry> entries = new List<OrderEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductsServeCheque()
        {}

        internal ProductsServeCheque([NotNull] CopyContext context, [NotNull] IProductsServeCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            entries = src.Entries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public IEnumerable<IOrderEntry> Entries
        {
            get { return entries; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IProductsServeCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new ProductsServeCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IProductsServeCheque>(copy, "ProductsServeCheque");
        }
    }
}
