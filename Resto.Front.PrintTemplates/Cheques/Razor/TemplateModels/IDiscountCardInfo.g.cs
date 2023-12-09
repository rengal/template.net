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
    /// Информация о скидочной карте
    /// </summary>
    public interface IDiscountCardInfo
    {
        /// <summary>
        /// Номер карты
        /// </summary>
        [NotNull]
        string Card { get; }

        /// <summary>
        /// Маскированный номер карты
        /// </summary>
        [NotNull]
        string MaskedCard { get; }

        /// <summary>
        /// Владелец карты
        /// </summary>
        [NotNull]
        string Owner { get; }

    }

    internal sealed class DiscountCardInfo : TemplateModelBase, IDiscountCardInfo
    {
        #region Fields
        private readonly string card;
        private readonly string maskedCard;
        private readonly string owner;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DiscountCardInfo()
        {}

        private DiscountCardInfo([NotNull] CopyContext context, [NotNull] IDiscountCardInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            card = src.Card;
            maskedCard = src.MaskedCard;
            owner = src.Owner;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DiscountCardInfo Convert([NotNull] CopyContext context, [CanBeNull] IDiscountCardInfo source)
        {
            if (source == null)
                return null;

            return new DiscountCardInfo(context, source);
        }
        #endregion

        #region Props
        public string Card
        {
            get { return GetLocalizedValue(card); }
        }

        public string MaskedCard
        {
            get { return GetLocalizedValue(maskedCard); }
        }

        public string Owner
        {
            get { return GetLocalizedValue(owner); }
        }

        #endregion
    }

}
