using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class OutgoingDocumentListRecord
    {
        public override int CashDisplayNumber
        {
            get { return cashNumber; }
        }

        public override int SessionDisplayNumber
        {
            get { return sessionNumber; }
        }

        public override Store SalesStore
        {
            get { return Store; }
        }

        public override Account RevenueAccountData
        {
            get { return RevenueAccount; }
        }

        public override Account ExpenseAccountData
        {
            get { return ExpenseAccount; }
        }

        public override decimal? TotalCostData
        {
            get { return TotalCost; }
        }

        [NotNull]
        public override string PaymentDateString
        {
            get { return PaymentDate.HasValue ? PaymentDate.Value.ToShortDateString() : string.Empty; }
        }

        [NotNull]
        public override string DueDateString
        {
            get { return DueDate.HasValue ? DueDate.Value.ToShortDateString() : string.Empty; }
        }
    }
}