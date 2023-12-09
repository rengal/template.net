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
    /// Информация об оплате в дополнительной валюте
    /// </summary>
    public interface ICurrencyInfo
    {
        /// <summary>
        /// Дополнительная валюта
        /// </summary>
        IAdditionalCurrency Currency { get; }

        /// <summary>
        /// Курс валюты
        /// </summary>
        decimal Rate { get; }

        /// <summary>
        /// Сумма в валюте
        /// </summary>
        decimal Sum { get; }

    }

    internal sealed class CurrencyInfo : TemplateModelBase, ICurrencyInfo
    {
        #region Fields
        private readonly AdditionalCurrency currency;
        private readonly decimal rate;
        private readonly decimal sum;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CurrencyInfo()
        {}

        private CurrencyInfo([NotNull] CopyContext context, [NotNull] ICurrencyInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            currency = context.GetConverted(src.Currency, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.AdditionalCurrency.Convert);
            rate = src.Rate;
            sum = src.Sum;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CurrencyInfo Convert([NotNull] CopyContext context, [CanBeNull] ICurrencyInfo source)
        {
            if (source == null)
                return null;

            return new CurrencyInfo(context, source);
        }
        #endregion

        #region Props
        public IAdditionalCurrency Currency
        {
            get { return currency; }
        }

        public decimal Rate
        {
            get { return rate; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        #endregion
    }

}
