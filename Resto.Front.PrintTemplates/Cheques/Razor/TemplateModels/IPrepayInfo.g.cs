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
    /// Информация о предоплатe
    /// </summary>
    public interface IPrepayInfo
    {
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        IPaymentType PaymentType { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Информация об оплате в дополнительной валюте. null, если оплата в основной валюте
        /// </summary>
        [CanBeNull]
        ICurrencyInfo CurrencyInfo { get; }

    }

    internal sealed class PrepayInfo : TemplateModelBase, IPrepayInfo
    {
        #region Fields
        private readonly decimal sum;
        private readonly PaymentType paymentType;
        private readonly string comment;
        private readonly CurrencyInfo currencyInfo;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PrepayInfo()
        {}

        private PrepayInfo([NotNull] CopyContext context, [NotNull] IPrepayInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sum = src.Sum;
            paymentType = context.GetConverted(src.PaymentType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert);
            comment = src.Comment;
            currencyInfo = context.GetConverted(src.CurrencyInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CurrencyInfo.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PrepayInfo Convert([NotNull] CopyContext context, [CanBeNull] IPrepayInfo source)
        {
            if (source == null)
                return null;

            return new PrepayInfo(context, source);
        }
        #endregion

        #region Props
        public decimal Sum
        {
            get { return sum; }
        }

        public IPaymentType PaymentType
        {
            get { return paymentType; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public ICurrencyInfo CurrencyInfo
        {
            get { return currencyInfo; }
        }

        #endregion
    }

}
