using System;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    partial class EgaisQueryNotAnsweredInvoices
    {
        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisQueryNotAnsweredInvoicesFullCaption,
                DocumentNumber.IsNullOrWhiteSpace()
                    ? string.Empty
                    : DocumentNumber + " ",
                DateIncoming ?? DateTime.Today);
            }
        }

        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_QUERY_NOT_ANSWERED_INVOICES; }
        }

        public override bool ReadyToSend
        {
            get { return !SourceRarId.IsNullOrWhiteSpace() && !Consignee.IsNullOrWhiteSpace() && Status.Editable && !Deleted; }
        }
    }
}
