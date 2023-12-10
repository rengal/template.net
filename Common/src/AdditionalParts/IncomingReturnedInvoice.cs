using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public sealed partial class IncomingReturnedInvoice
    {
        public override string DocumentFullCaption
        {
            get
            {
                string duplicateAddition = IsDuplicate ? Resources.AbstractDocumentFullCaptionCopyWithFollowingAddition : string.Empty;
                var automaticDocumentAddition = IsAutomatic.GetValueOrDefault(false)
                    ? Resources.AbstractDocumentFullCaptionAutoCreated
                    : string.Empty;
                if (!IsAutomatic.GetValueOrDefault(false))
                {
                    duplicateAddition = duplicateAddition.TrimEnd(',', ' ');
                }
                var shouldAdd = !duplicateAddition.IsNullOrWhiteSpace() ||
                                !automaticDocumentAddition.IsNullOrWhiteSpace();
                var additionalText = shouldAdd
                    ? string.Format("({0}{1})", duplicateAddition, automaticDocumentAddition)
                    : string.Empty;

                return string.Format(Resources.AbstractDocumentNumberFrom2, DocumentType.GetLocalName(), DocumentNumber, DateIncoming.ToShortDateString(), additionalText);
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.INCOMING_RETURNED_INVOICE; }
        }
    }
}