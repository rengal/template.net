using System;

using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class CashRegisterTask
    {
        [Transient]
        private Guid cashierId;

        public Guid CashierId
        {
            get { return cashierId; }
            set { cashierId = value; }
        }

        private string cashierTaxpayerId;
        /// <summary>
        /// ИНН кассира
        /// </summary>
        public string CashierTaxpayerId
        {
            get { return cashierTaxpayerId; }
            set { cashierTaxpayerId = value; }
        }
    }
}