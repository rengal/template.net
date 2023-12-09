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
    /// Маркетинговая акция
    /// </summary>
    public interface IDiscountMarketingCampaign
    {
        /// <summary>
        /// Название акции
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Суммарная скидка по акции
        /// </summary>
        decimal TotalDiscount { get; }

        /// <summary>
        /// Комментарий для пречека
        /// </summary>
        [CanBeNull]
        string BillComment { get; }

        /// <summary>
        /// Операции по акции
        /// </summary>
        [NotNull]
        IEnumerable<IDiscountMarketingCampaignOperation> Operations { get; }

    }

    internal sealed class DiscountMarketingCampaign : TemplateModelBase, IDiscountMarketingCampaign
    {
        #region Fields
        private readonly string name;
        private readonly decimal totalDiscount;
        private readonly string billComment;
        private readonly List<DiscountMarketingCampaignOperation> operations = new List<DiscountMarketingCampaignOperation>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DiscountMarketingCampaign()
        {}

        private DiscountMarketingCampaign([NotNull] CopyContext context, [NotNull] IDiscountMarketingCampaign src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            totalDiscount = src.TotalDiscount;
            billComment = src.BillComment;
            operations = src.Operations.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountMarketingCampaignOperation.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static DiscountMarketingCampaign Convert([NotNull] CopyContext context, [CanBeNull] IDiscountMarketingCampaign source)
        {
            if (source == null)
                return null;

            return new DiscountMarketingCampaign(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public decimal TotalDiscount
        {
            get { return totalDiscount; }
        }

        public string BillComment
        {
            get { return GetLocalizedValue(billComment); }
        }

        public IEnumerable<IDiscountMarketingCampaignOperation> Operations
        {
            get { return operations; }
        }

        #endregion
    }

}
