using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class CashRegisterResult
    {
        [HasDefaultValue]
        private readonly List<CashRegisterVatData> dailyVatTotalizers = new List<CashRegisterVatData>();
        private bool haveDailyVatTotalizers;

        [NotNull]
        [HasDefaultValue]
        private readonly List<CashRegisterVatData> chequeVatTotalizers = new List<CashRegisterVatData>();
        private bool haveChequeVatTotalizers;

        [NotNull]
        public List<CashRegisterVatData> DailyVatTotalizers
        {
            get { return dailyVatTotalizers; }
        }

        public bool HaveDailyVatTotalizers
        {
            get { return haveDailyVatTotalizers; }
            set { haveDailyVatTotalizers = value; }
        }

        [NotNull]
        public List<CashRegisterVatData> ChequeVatTotalizers
        {
            get { return chequeVatTotalizers; }
        }

        public bool HaveChequeVatTotalizers
        {
            get { return haveChequeVatTotalizers; }
            set { haveChequeVatTotalizers = value; }
        }
    }
}