using Resto.Common.Extensions;
using Resto.Common.Localization;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class AbstractSalesDocument
    {
        public override string DocumentFullCaption
        {
            get
            {
                string duplicateAddition = IsDuplicate ? Resources.AbstractDocumentFullCaptionCopyWithFollowingAddition : string.Empty;
                string automatDocumentAddition = IsAutomatic.GetValueOrFakeDefault() ? Resources.AbstractDocumentFullCaptionAutoCreated : string.Empty;
                if (!IsAutomatic.GetValueOrFakeDefault())
                    duplicateAddition = duplicateAddition.TrimEnd(new[] { ',', ' ' });
                bool add = !string.IsNullOrEmpty(duplicateAddition) || !string.IsNullOrEmpty(automatDocumentAddition);
                return string.Format(Resources.AbstractSalesDocumentDocumentFullCaption, DocumentType.GetLocalName(), DocumentNumber, 
                    DateIncoming.ToShortDateString(), (add ? string.Format("({0}{1})", duplicateAddition, automatDocumentAddition) : string.Empty));
            }
        }
    }
}