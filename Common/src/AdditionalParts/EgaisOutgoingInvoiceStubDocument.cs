using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    partial class EgaisOutgoingInvoiceStubDocument
    {
        public override DocumentType DocumentType
        {
            get { return DocumentType.EGAIS_OUTGOING_INVOICE; }
        }
    }
}
