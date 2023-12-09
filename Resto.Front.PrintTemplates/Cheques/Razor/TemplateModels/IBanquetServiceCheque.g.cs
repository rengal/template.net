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
    /// Сервисный чек банкетного заказа
    /// </summary>
    public interface IBanquetServiceCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Флаг повторной печати
        /// </summary>
        bool IsRepeatPrint { get; }

        /// <summary>
        /// Позиции заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IOrderEntry> Entries { get; }

    }

    internal sealed class BanquetServiceCheque : ServiceChequeBase, IBanquetServiceCheque
    {
        #region Fields
        private readonly bool isRepeatPrint;
        private readonly List<OrderEntry> entries = new List<OrderEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private BanquetServiceCheque()
        {}

        internal BanquetServiceCheque([NotNull] CopyContext context, [NotNull] IBanquetServiceCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            isRepeatPrint = src.IsRepeatPrint;
            entries = src.Entries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public bool IsRepeatPrint
        {
            get { return isRepeatPrint; }
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
        public static string Serialize([NotNull] this IBanquetServiceCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new BanquetServiceCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IBanquetServiceCheque>(copy, "BanquetServiceCheque");
        }
    }
}
