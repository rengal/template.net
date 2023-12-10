using System;
using System.Linq;
using System.Text;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisShopIncoming
    {
        public EgaisShopIncomingType NullableEgaisIncomingType
        {
            get { return EgaisIncomingType; }
        }

        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_SHOP_INCOMING; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisShopIncomingFullCaption, GetDocumentTypeStr(NullableEgaisIncomingType), DateIncoming);
            }
        }

        public bool AllowOpenLinkedWriteoff
        {
            get { return EgaisIncomingType == EgaisShopIncomingType.MIXED && !MixedWriteoffRegId.IsNullOrWhiteSpace(); }
        }
    }
}
