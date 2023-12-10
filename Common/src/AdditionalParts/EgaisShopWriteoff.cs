using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisShopWriteoff
    {
        public EgaisShopWriteoffType NullableEgaisWriteoffType
        {
            get { return EgaisWriteoffType; }
        }

        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_SHOP_WRITEOFF; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                string documentCaption;
                if (egaisRegister == EgaisRegister.SHOP)
                {
                    documentCaption = Resources.EgaisShopWriteoffSecondRegisterFullCaption;
                }
                else if (egaisRegister == EgaisRegister.STORE)
                {
                    documentCaption = Resources.EgaisShopWriteoffFirstRegisterFullCaption;
                }
                else
                {
                    documentCaption = Resources.EgaisShopWriteoffNoRegisterFullCaption;
                }

                return string.Format(documentCaption, GetDocumentTypeStr(NullableEgaisWriteoffType), DateIncoming);
            }
        }

        public string DocumentShortCaption
        {
            get
            {
                return string.Format(Resources.EgaisShopWriteofNoRegisterShortCaption,
                    DocumentNumber, DateIncoming, GetDocumentTypeStr(NullableEgaisWriteoffType));
            }
        }

        /// <summary>
        /// Можно ли создать постановку на баланс по акту списания
        /// </summary>
        public bool AllowCreateShopIncomingByWriteoff
        {
            get { return !DocumentRegId.IsNullOrWhiteSpace() && EgaisWriteoffType == EgaisShopWriteoffType.MIXED; }
        }
    }
}
