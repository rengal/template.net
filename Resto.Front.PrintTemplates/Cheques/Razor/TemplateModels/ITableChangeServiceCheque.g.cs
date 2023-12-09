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
    /// Чек смены стола заказа
    /// </summary>
    public interface ITableChangeServiceCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Стол, с которого был перенесён заказ
        /// </summary>
        [NotNull]
        ITable TableFrom { get; }

        /// <summary>
        /// Позиции заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IOrderEntry> Entries { get; }

    }

    internal sealed class TableChangeServiceCheque : ServiceChequeBase, ITableChangeServiceCheque
    {
        #region Fields
        private readonly Table tableFrom;
        private readonly List<OrderEntry> entries = new List<OrderEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private TableChangeServiceCheque()
        {}

        internal TableChangeServiceCheque([NotNull] CopyContext context, [NotNull] ITableChangeServiceCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            tableFrom = context.GetConverted(src.TableFrom, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Table.Convert);
            entries = src.Entries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public ITable TableFrom
        {
            get { return tableFrom; }
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
        public static string Serialize([NotNull] this ITableChangeServiceCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new TableChangeServiceCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<ITableChangeServiceCheque>(copy, "TableChangeServiceCheque");
        }
    }
}
