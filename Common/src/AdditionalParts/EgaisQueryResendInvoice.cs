using System;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    partial class EgaisQueryResendInvoice
    {
        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisQueryResendInvoiceDocumentFullCaption,
                    DocumentNumber.IsNullOrWhiteSpace()
                        ? string.Empty
                        : DocumentNumber + " ",
                    DateIncoming ?? DateTime.Today);
            }
        }

        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_QUERY_RESEND_INVOICE; }
        }

        public override bool ReadyToSend
        {
            get { return !SourceRarId.IsNullOrWhiteSpace() && !WbRegId.IsNullOrWhiteSpace() && Status.Editable && !Deleted; }
        }
    }
}
