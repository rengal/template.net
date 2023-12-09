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
    /// Скидка, применённая к заказу
    /// </summary>
    public interface IDiscountItem
    {
        /// <summary>
        /// Тип скидки
        /// </summary>
        [NotNull]
        IDiscountType Type { get; }

        /// <summary>
        /// Признак категориальной скидки
        /// </summary>
        bool IsCategorized { get; }

        /// <summary>
        /// Способ добавления скидки
        /// </summary>
        DiscountSource Source { get; }

        /// <summary>
        /// Информация о скидочной карте
        /// </summary>
        [CanBeNull]
        IDiscountCardInfo CardInfo { get; }

        /// <summary>
        /// Кем авторизовано добавление скидки
        /// </summary>
        [CanBeNull]
        IAuthData AuthData { get; }

        /// <summary>
        /// Детализировать в пречеке (с учётом выборочно применённых скидок)
        /// </summary>
        bool PrintDetailedInPrecheque { get; }

        /// <summary>
        /// Суммы скидок по позициям заказа (ненулевые)
        /// </summary>
        [NotNull]
        IDictionary<IOrderEntry, decimal> DiscountSums { get; }

    }

    internal sealed class DiscountItem : TemplateModelBase, IDiscountItem
    {
        #region Fields
        private readonly DiscountType type;
        private readonly bool isCategorized;
        private readonly DiscountSource source;
        private readonly DiscountCardInfo cardInfo;
        private readonly AuthData authData;
        private readonly bool printDetailedInPrecheque;
        private readonly Dictionary<OrderEntry, decimal> discountSums = new Dictionary<OrderEntry, decimal>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DiscountItem()
        {}

        private DiscountItem([NotNull] CopyContext context, [NotNull] IDiscountItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            type = context.GetConverted(src.Type, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountType.Convert);
            isCategorized = src.IsCategorized;
            source = src.Source;
            cardInfo = context.GetConverted(src.CardInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountCardInfo.Convert);
            authData = context.GetConverted(src.AuthData, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.AuthData.Convert);
            printDetailedInPrecheque = src.PrintDetailedInPrecheque;
            discountSums = src.DiscountSums.ToDictionary(kvp => context.GetConverted(kvp.Key, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntry.Convert), kvp => kvp.Value);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DiscountItem Convert([NotNull] CopyContext context, [CanBeNull] IDiscountItem source)
        {
            if (source == null)
                return null;

            return new DiscountItem(context, source);
        }
        #endregion

        #region Props
        public IDiscountType Type
        {
            get { return type; }
        }

        public bool IsCategorized
        {
            get { return isCategorized; }
        }

        public DiscountSource Source
        {
            get { return source; }
        }

        public IDiscountCardInfo CardInfo
        {
            get { return cardInfo; }
        }

        public IAuthData AuthData
        {
            get { return authData; }
        }

        public bool PrintDetailedInPrecheque
        {
            get { return printDetailedInPrecheque; }
        }

        public IDictionary<IOrderEntry, decimal> DiscountSums
        {
            get { return discountSums.ToDictionary(kvp => (IOrderEntry)kvp.Key, kvp => (decimal)kvp.Value); }
        }

        #endregion
    }

}
