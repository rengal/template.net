using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Common.Localization;

namespace Resto.Data
{
    partial class EgaisAbstractInternalDocument
    {
        public override string DocumentStatusStr
        {
            get { return status.GetLocalName(); }
        }

        public override bool IsEditable
        {
            get { return Status.Editable; }
        }

        public virtual bool ReadyToSend
        {
            get
            {
                return Items.Any() && Items.OfType<EgaisShopWriteoffItem>().All(item => item.IsCorrect) &&
                       Status.Editable && !Deleted;
            }
        }

        protected string GetDocumentTypeStr(ILocalizableName documentType)
        {
            return documentType == null ? string.Empty : "'" + documentType.GetLocalName() + "'";
        }
    }
}
