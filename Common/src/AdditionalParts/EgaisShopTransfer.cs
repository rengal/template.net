using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Common.Properties;

namespace Resto.Data
{
    partial class EgaisShopTransfer
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_SHOP_TRANSFER; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                string fullCaptionLayout;
                if (direction.Equals(EgaisShopTransferDirection.TO_SHOP))
                {
                    fullCaptionLayout = Resources.EgaisShopTransferToShopFullCaption;
                }
                else if (direction.Equals(EgaisShopTransferDirection.FROM_SHOP))
                {
                    fullCaptionLayout = Resources.EgaisShopTransferFromShopFullCaption;
                }
                else
                {
                    fullCaptionLayout = string.Empty;
                }
                return string.Format(fullCaptionLayout, DateIncoming);
            }
        }

        public override bool ReadyToSend
        {
            get { return base.ReadyToSend && Items.OfType<EgaisShopTransferItem>().All(item => item.IsCorrect); }
        }
    }
}
