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
    /// Операция по маркетинговой акции
    /// </summary>
    public interface IDiscountMarketingCampaignOperation
    {
        /// <summary>
        /// Название операции
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Скидка
        /// </summary>
        decimal Discount { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

    }

    internal sealed class DiscountMarketingCampaignOperation : TemplateModelBase, IDiscountMarketingCampaignOperation
    {
        #region Fields
        private readonly string name;
        private readonly decimal discount;
        private readonly string comment;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DiscountMarketingCampaignOperation()
        {}

        private DiscountMarketingCampaignOperation([NotNull] CopyContext context, [NotNull] IDiscountMarketingCampaignOperation src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            discount = src.Discount;
            comment = src.Comment;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DiscountMarketingCampaignOperation Convert([NotNull] CopyContext context, [CanBeNull] IDiscountMarketingCampaignOperation source)
        {
            if (source == null)
                return null;

            return new DiscountMarketingCampaignOperation(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public decimal Discount
        {
            get { return discount; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        #endregion
    }

}
