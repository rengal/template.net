using System;

namespace Resto.Data
{
    public sealed partial class IncomingService
    {
        public IncomingService(Guid id, DateTime dateIncoming, DocumentStatus status, string documentNumber, User supplier, Account defaultAccount, Account revenueCreditAccount
            )
            : this(id, dateIncoming, documentNumber, status, supplier, revenueCreditAccount)
        {
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.INCOMING_SERVICE; }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return Department; }
            set { }
        }
    }
}