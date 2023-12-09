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
    /// Сервисный чек
    /// </summary>
    public interface IServiceCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Флаг повторной печати
        /// </summary>
        bool IsRepeatPrint { get; }

        /// <summary>
        /// Штрихкод
        /// </summary>
        [CanBeNull]
        string Barcode { get; }

        /// <summary>
        /// Позиции заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IOrderEntry> Entries { get; }

    }

    internal sealed class ServiceCheque : ServiceChequeBase, IServiceCheque
    {
        #region Fields
        private readonly bool isRepeatPrint;
        private readonly string barcode;
        private readonly List<OrderEntry> entries = new List<OrderEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ServiceCheque()
        {}

        internal ServiceCheque([NotNull] CopyContext context, [NotNull] IServiceCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            isRepeatPrint = src.IsRepeatPrint;
            barcode = src.Barcode;
            entries = src.Entries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public bool IsRepeatPrint
        {
            get { return isRepeatPrint; }
        }

        public string Barcode
        {
            get { return GetLocalizedValue(barcode); }
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
        public static string Serialize([NotNull] this IServiceCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new ServiceCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IServiceCheque>(copy, "ServiceCheque");
        }
    }
}
