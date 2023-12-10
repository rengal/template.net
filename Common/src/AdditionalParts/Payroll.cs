using System.Collections.Generic;
using Resto.Common.Localization;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class Payroll
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
        public override DocumentType DocumentType { get { return DocumentType.PAYROLL; } }

        public override string ToString()
        {
            return string.Format(Resources.PayrollDocumentStringFormat,
                                 DocumentType.GetLocalName(),
                                 DocumentNumber,
                                 dateFrom.ToShortDateString(),
                                 dateTo.ToShortDateString());
        }

        public override string DocumentFullCaption
        {
            get
            {
                return ToString();
            }
        }
    }
}