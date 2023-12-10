using Resto.Common.Localization;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class EgaisMarkConfirm
    {
        public override string DocumentStatusStr
        {
            get { return MarkStatus.GetLocalName(); }
        }

        public override bool IsEditable
        {
            get { return false; }
        }

        public virtual bool ReadyToSend
        {
            get
            {
                return false;
            }
        }

        public override bool IsDeleteable
        {
            get { return false; }
        }

        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_MARK_CONFIRM; }
        }

        public override string DocumentFullCaption
        {
            get { return string.Format(Resources.EgaisMarkConfirmFullCaption, DateIncoming, BrandInfo.PosApp); }
        }
    }
}
