using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class ProductReplacementDocument
    {
        public override Account DocAccount
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override Store DocStore
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override Store DocStoreTo
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.PRODUCT_REPLACEMENT; }
        }
    }
}