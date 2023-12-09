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
    /// Налоговая категория.
    /// </summary>
    public interface ITaxCategoryInfo
    {
        /// <summary>
        /// Сумма без НДС.
        /// </summary>
        decimal SumWithoutVat { get; }

        /// <summary>
        /// Название налоговой категории.
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Процент НДС.
        /// </summary>
        decimal VatPercent { get; }

        /// <summary>
        /// Сумма НДС.
        /// </summary>
        decimal VatSum { get; }

    }

    internal sealed class TaxCategoryInfo : TemplateModelBase, ITaxCategoryInfo
    {
        #region Fields
        private readonly decimal sumWithoutVat;
        private readonly string name;
        private readonly decimal vatPercent;
        private readonly decimal vatSum;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private TaxCategoryInfo()
        {}

        private TaxCategoryInfo([NotNull] CopyContext context, [NotNull] ITaxCategoryInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sumWithoutVat = src.SumWithoutVat;
            name = src.Name;
            vatPercent = src.VatPercent;
            vatSum = src.VatSum;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static TaxCategoryInfo Convert([NotNull] CopyContext context, [CanBeNull] ITaxCategoryInfo source)
        {
            if (source == null)
                return null;

            return new TaxCategoryInfo(context, source);
        }
        #endregion

        #region Props
        public decimal SumWithoutVat
        {
            get { return sumWithoutVat; }
        }

        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public decimal VatPercent
        {
            get { return vatPercent; }
        }

        public decimal VatSum
        {
            get { return vatSum; }
        }

        #endregion
    }

}
