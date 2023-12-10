using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class IncomingInvoiceListRecord
    {
        public override string IncomingInvoiceCaptionString
        {
            get
            {
                return InvoiceIncomingNumber;
            }
        }

        public override decimal? TotalCostData
        {
            get
            {
                return StoresList.Any() && StoresList.First().GetDepartmentEntity().VatAccumulator != null
                           ? SumWithoutNds.GetValueOrFakeDefault()
                           : Sum.GetValueOrFakeDefault();
            }
        }

        public override string InvoicesAsString
        {
            get { return Invoice; }
        }

        [NotNull]
        public override string DueDateString
        {
            get { return DueDate.HasValue ? DueDate.Value.ToShortDateString() : string.Empty; }
        }

        [NotNull]
        public override string PaymentDateString
        {
            get { return PaymentDate.HasValue ? PaymentDate.Value.ToShortDateString() : string.Empty; }
        }

        [NotNull]
        public override string IsMatchesToTheOrderString
        {
            get { return GetStringOfBoolean(MatchesToTheOrder); }
        }

        [NotNull]
        public override string IsDeliveryOnTimeString
        {
            get { return GetStringOfBoolean(DeliveryOnTime); }
        }

        [NotNull]
        private string GetStringOfBoolean(bool? value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.Value ? Resources.CommonAdditionalPartsYes : Resources.CommonAdditionalPartsNo;
        }
    }
}