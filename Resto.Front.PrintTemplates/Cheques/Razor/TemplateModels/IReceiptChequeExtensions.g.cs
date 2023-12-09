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
    /// Дополнительные элементы квитанции об оплате/возврате (добавляются плагинами)
    /// </summary>
    public interface IReceiptChequeExtensions
    {
        /// <summary>
        /// Элементы, добавляемые до шапки
        /// </summary>
        [NotNull]
        IEnumerable<XElement> BeforeCheque { get; }

        /// <summary>
        /// Элементы, добавляемые после подвала
        /// </summary>
        [NotNull]
        IEnumerable<XElement> AfterCheque { get; }

    }

    internal sealed class ReceiptChequeExtensions : TemplateModelBase, IReceiptChequeExtensions
    {
        #region Fields
        private readonly List<XElement> beforeCheque = new List<XElement>();
        private readonly List<XElement> afterCheque = new List<XElement>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ReceiptChequeExtensions()
        {}

        private ReceiptChequeExtensions([NotNull] CopyContext context, [NotNull] IReceiptChequeExtensions src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            beforeCheque = src.BeforeCheque.ToList();
            afterCheque = src.AfterCheque.ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ReceiptChequeExtensions Convert([NotNull] CopyContext context, [CanBeNull] IReceiptChequeExtensions source)
        {
            if (source == null)
                return null;

            return new ReceiptChequeExtensions(context, source);
        }
        #endregion

        #region Props
        public IEnumerable<XElement> BeforeCheque
        {
            get { return beforeCheque; }
        }

        public IEnumerable<XElement> AfterCheque
        {
            get { return afterCheque; }
        }

        #endregion
    }

}
