using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class InvoiceDocumentSimpleListRecord
    {
        public virtual string DocumentShortCaption
        {
            get { return !Number.IsNullOrEmpty() ? string.Format(Resources.AbstractDocumentListRecordNumberFrom, Number, Date.GetValueOrDefault().ToShortDateString()) : string.Empty; }
        }
    }
}
