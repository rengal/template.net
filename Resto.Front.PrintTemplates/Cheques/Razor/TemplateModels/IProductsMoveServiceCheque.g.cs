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
    /// Чек переноса позиций заказа
    /// </summary>
    public interface IProductsMoveServiceCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Заказ, откуда перенесли позиции
        /// </summary>
        [NotNull]
        IOrder OrderFrom { get; }

        /// <summary>
        /// Позиции заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IOrderEntry> Entries { get; }

    }

    internal sealed class ProductsMoveServiceCheque : ServiceChequeBase, IProductsMoveServiceCheque
    {
        #region Fields
        private readonly Order orderFrom;
        private readonly List<OrderEntry> entries = new List<OrderEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductsMoveServiceCheque()
        {}

        internal ProductsMoveServiceCheque([NotNull] CopyContext context, [NotNull] IProductsMoveServiceCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            orderFrom = context.GetConverted(src.OrderFrom, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            entries = src.Entries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public IOrder OrderFrom
        {
            get { return orderFrom; }
        }

        public IEnumerable<IOrderEntry> Entries
        {
            get { return entries; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IProductsMoveServiceCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new ProductsMoveServiceCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IProductsMoveServiceCheque>(copy, "ProductsMoveServiceCheque");
        }
    }
}
