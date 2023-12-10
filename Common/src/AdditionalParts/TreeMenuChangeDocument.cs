using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class TreeMenuChangeDocument
    {
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

        public override DepartmentEntity DocDepartmentTo
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override Account DocAccount
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.MENU_CHANGE; }
        }

        public DateTime? NullableDateTo
        {
            get { return dateTo == DateTimeServerConstants.MAX_DATE.DateTime ? (DateTime?)null : dateTo; }
            set { dateTo = value.GetValueOrDefault(DateTimeServerConstants.MAX_DATE.DateTime); }
        }
    }
}