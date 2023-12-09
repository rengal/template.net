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
    /// Дополнительные элементы пречека (добавляются плагинами)
    /// </summary>
    public interface IBillChequeExtensions
    {
        /// <summary>
        /// Элементы, добавляемые до шапки
        /// </summary>
        [NotNull]
        IEnumerable<XElement> BeforeHeader { get; }

        /// <summary>
        /// Элементы, добавляемые после шапки
        /// </summary>
        [NotNull]
        IEnumerable<XElement> AfterHeader { get; }

        /// <summary>
        /// Элементы, добавляемые до подвала
        /// </summary>
        [NotNull]
        IEnumerable<XElement> BeforeFooter { get; }

        /// <summary>
        /// Элементы, добавляемые после подвала
        /// </summary>
        [NotNull]
        IEnumerable<XElement> AfterFooter { get; }

    }

    internal sealed class BillChequeExtensions : TemplateModelBase, IBillChequeExtensions
    {
        #region Fields
        private readonly List<XElement> beforeHeader = new List<XElement>();
        private readonly List<XElement> afterHeader = new List<XElement>();
        private readonly List<XElement> beforeFooter = new List<XElement>();
        private readonly List<XElement> afterFooter = new List<XElement>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private BillChequeExtensions()
        {}

        private BillChequeExtensions([NotNull] CopyContext context, [NotNull] IBillChequeExtensions src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            beforeHeader = src.BeforeHeader.ToList();
            afterHeader = src.AfterHeader.ToList();
            beforeFooter = src.BeforeFooter.ToList();
            afterFooter = src.AfterFooter.ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static BillChequeExtensions Convert([NotNull] CopyContext context, [CanBeNull] IBillChequeExtensions source)
        {
            if (source == null)
                return null;

            return new BillChequeExtensions(context, source);
        }
        #endregion

        #region Props
        public IEnumerable<XElement> BeforeHeader
        {
            get { return beforeHeader; }
        }

        public IEnumerable<XElement> AfterHeader
        {
            get { return afterHeader; }
        }

        public IEnumerable<XElement> BeforeFooter
        {
            get { return beforeFooter; }
        }

        public IEnumerable<XElement> AfterFooter
        {
            get { return afterFooter; }
        }

        #endregion
    }

}
