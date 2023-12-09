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
    /// Скидки по маркетинговым акциям
    /// </summary>
    public interface IDiscountMarketingCampaigns
    {
        /// <summary>
        /// Общая скидка
        /// </summary>
        decimal TotalDiscount { get; }

        /// <summary>
        /// Маркетинговые акции
        /// </summary>
        [NotNull]
        IEnumerable<IDiscountMarketingCampaign> Campaigns { get; }

    }

    internal sealed class DiscountMarketingCampaigns : TemplateModelBase, IDiscountMarketingCampaigns
    {
        #region Fields
        private readonly decimal totalDiscount;
        private readonly List<DiscountMarketingCampaign> campaigns = new List<DiscountMarketingCampaign>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DiscountMarketingCampaigns()
        {}

        private DiscountMarketingCampaigns([NotNull] CopyContext context, [NotNull] IDiscountMarketingCampaigns src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            totalDiscount = src.TotalDiscount;
            campaigns = src.Campaigns.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountMarketingCampaign.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DiscountMarketingCampaigns Convert([NotNull] CopyContext context, [CanBeNull] IDiscountMarketingCampaigns source)
        {
            if (source == null)
                return null;

            return new DiscountMarketingCampaigns(context, source);
        }
        #endregion

        #region Props
        public decimal TotalDiscount
        {
            get { return totalDiscount; }
        }

        public IEnumerable<IDiscountMarketingCampaign> Campaigns
        {
            get { return campaigns; }
        }

        #endregion
    }

}
