using System.Collections.Generic;

namespace Resto.Data
{
    public abstract partial class AbstractVatInvoice
    {
        public override Store DocStore
        {
            get { return null; }
            set { }
        }

        public override Store DocStoreTo
        {
            get { return null; }
            set { }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return null; }
            set { }
        }

        public override Account DocAccount
        {
            get { return null; }
            set { }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get { return null; }
            set { }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.VAT_INVOICE; }
        }
    }
}
