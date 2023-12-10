using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class EgaisMarkUnFix
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_MARK_UNFIX; }
        }

        public override string DocumentFullCaption
        {
            get { return string.Format(Resources.EgaisMarkUnFixFullCaption, DateIncoming); }
        }
    }
}
