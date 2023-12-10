using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class ClosedSessionDocument
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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.SESSION_ACCEPTANCE; }
        }
    }
}