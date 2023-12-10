using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public sealed partial class OutgoingService
    {
        public OutgoingService(Guid id, DateTime dateIncoming, DocumentStatus status, string documentNumber, User supplier, Account defaultAccount, Account revenueDebitAccount)
            : this(id, dateIncoming, documentNumber, status, supplier)
        {
            this.revenueDebitAccount = revenueDebitAccount;
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.OUTGOING_SERVICE; }
        }
    }
}