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
    /// Пречек
    /// </summary>
    public interface IBillCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Номер повторного пречека
        /// </summary>
        int RepeatBillNumber { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Дополнительные элементы
        /// </summary>
        [NotNull]
        IBillChequeExtensions Extensions { get; }

        /// <summary>
        /// Сведения для промежуточной квитанции дозаказа
        /// </summary>
        [CanBeNull]
        IAdditionalServiceChequeInfo AdditionalServiceChequeInfo { get; }

        /// <summary>
        /// Скидки по маркетинговым акциям
        /// </summary>
        [CanBeNull]
        IDiscountMarketingCampaigns DiscountMarketingCampaigns { get; }

    }

    internal sealed class BillCheque : TemplateModelBase, IBillCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly int repeatBillNumber;
        private readonly Order order;
        private readonly BillChequeExtensions extensions;
        private readonly AdditionalServiceChequeInfo additionalServiceChequeInfo;
        private readonly DiscountMarketingCampaigns discountMarketingCampaigns;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private BillCheque()
        {}

        internal BillCheque([NotNull] CopyContext context, [NotNull] IBillCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            repeatBillNumber = src.RepeatBillNumber;
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            extensions = context.GetConverted(src.Extensions, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.BillChequeExtensions.Convert);
            additionalServiceChequeInfo = context.GetConverted(src.AdditionalServiceChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.AdditionalServiceChequeInfo.Convert);
            discountMarketingCampaigns = context.GetConverted(src.DiscountMarketingCampaigns, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountMarketingCampaigns.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public int RepeatBillNumber
        {
            get { return repeatBillNumber; }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public IBillChequeExtensions Extensions
        {
            get { return extensions; }
        }

        public IAdditionalServiceChequeInfo AdditionalServiceChequeInfo
        {
            get { return additionalServiceChequeInfo; }
        }

        public IDiscountMarketingCampaigns DiscountMarketingCampaigns
        {
            get { return discountMarketingCampaigns; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IBillCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new BillCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IBillCheque>(copy, "BillCheque");
        }
    }
}
