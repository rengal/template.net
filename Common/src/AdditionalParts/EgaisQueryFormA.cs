using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisQueryFormA
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_QUERY_FORM_A; }
        }

        public override bool ReadyToSend
        {
            get { return !SourceRarId.IsNullOrWhiteSpace() && Status.Editable; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisQueryFormAFullCaption, ARegId.IsNullOrWhiteSpace() ? string.Empty : ARegId + " ", DateIncoming);
            }
        }
    }
}
