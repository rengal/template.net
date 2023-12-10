using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class EgaisMarkFix
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_MARK_FIX; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisMarkFixFullCaption, DateIncoming);
            }
        }
    }
}
