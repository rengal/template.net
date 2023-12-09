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
    /// Чек удаления блюд
    /// </summary>
    public interface IDeleteProductsServiceCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Причина удаления
        /// </summary>
        [NotNull]
        string DeleteReason { get; }

        /// <summary>
        /// Позиции заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IOrderEntry> Entries { get; }

    }

    internal sealed class DeleteProductsServiceCheque : ServiceChequeBase, IDeleteProductsServiceCheque
    {
        #region Fields
        private readonly string deleteReason;
        private readonly List<OrderEntry> entries = new List<OrderEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DeleteProductsServiceCheque()
        {}

        internal DeleteProductsServiceCheque([NotNull] CopyContext context, [NotNull] IDeleteProductsServiceCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            deleteReason = src.DeleteReason;
            entries = src.Entries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public string DeleteReason
        {
            get { return GetLocalizedValue(deleteReason); }
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
        public static string Serialize([NotNull] this IDeleteProductsServiceCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new DeleteProductsServiceCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IDeleteProductsServiceCheque>(copy, "DeleteProductsServiceCheque");
        }
    }
}
