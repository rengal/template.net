using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class ScheduledIncoming
    {
        public override AbstractScheduledPayment Copy()
        {
            return new ScheduledIncoming(Id, DateFrom, DateTo, Number, PaymentRequestId, Name, DueDate,
                PaymentType, Sum, Department, Paid)
            {
                Counteragent = Counteragent,
                CashFlowCategory = CashFlowCategory,
                Conception = Conception,
                WriteoffAccount = WriteoffAccount,
                ExpenseAccount = ExpenseAccount,
                PaymentRequestId = PaymentRequestId
            };
        }
    }
}
