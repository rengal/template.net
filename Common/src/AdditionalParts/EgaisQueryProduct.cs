using System;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    partial class EgaisQueryProduct
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_QUERY_PRODUCT; }
        }

        public override bool ReadyToSend
        {
            get { return !SourceRarId.IsNullOrWhiteSpace() && Parameters.Any() && Status.Editable && !Deleted; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisQueryProductFullCaption, DocumentNumber.IsNullOrWhiteSpace() ? string.Empty : DocumentNumber + " ", DateIncoming ?? DateTime.Today);
            }
        }
    }
}
