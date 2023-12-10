using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class ScheduledIncomingRequest
    {
        public override ScheduledPaymentType PaymentType
        {
            get { return ScheduledPaymentType.SCHEDULED_INCOMING_PAYMENT; }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.SCHEDULED_INCOMING; }
        }

        public override AbstractScheduledPaymentRequest Copy()
        {
            return new ScheduledIncomingRequest(Id, DateIncoming, DocumentNumber, Start, End, Sum)
            {
                Name = Name,
                Period = Period,
                WriteoffAccount = WriteoffAccount,
                ExpenseAccount = ExpenseAccount,
                Conception = Conception,
                Counteragent = Counteragent,
                CashFlowCategory = CashFlowCategory,
                Departments = Departments,
            };
        }
    }
}

