using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class EgaisRequestRepeal
    {
        /// <summary>
        /// Документ имеет статус, в котором его можно послать в ЕГАИС.
        /// </summary>
        public bool IsSendable => Status.Editable && !IsConfirmable;

        /// <summary>
        /// Документ имеет статус, в котором
        /// с отменой можно согласиться или отказаться.
        /// </summary>
        public bool IsConfirmable => RepealStatus.Answerable;

        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_REQUEST_REPEAL; }
        }

        public override bool ReadyToSend => Status.Editable && !Deleted;

        public override string DocumentFullCaption
        {
            get
            {
                if (RepealedDocumentType == EgaisDocumentTypes.EGAIS_INCOMING_INVOICE)
                {
                    return string.Format(Resources.EgaisRequestRepealIncomingInvoiceActFullCaption,
                        RepealedRegId, DateIncoming);
                }

                if (RepealedDocumentType == EgaisDocumentTypes.EGAIS_OUTGOING_INVOICE)
                {
                    return string.Format(Resources.EgaisRequestRepealOutgoingInvoiceActFullCaption,
                        RepealedRegId, DateIncoming);
                }

                if (RepealedDocumentType == EgaisDocumentTypes.EGAIS_SHOP_INCOMING)
                {
                    return string.Format(Resources.EgaisRequestRepealShopIncomingFullCaption,
                        RepealedRegId, DateIncoming);
                }

                if (RepealedDocumentType == EgaisDocumentTypes.EGAIS_SHOP_WRITEOFF)
                {
                    return string.Format(Resources.EgaisRequestRepealShopWriteoffFullCaption,
                        RepealedRegId, DateIncoming);
                }

                return string.Format(Resources.EgaisRequestRepealUnknownFullCaption, RepealedRegId, DateIncoming);
            }
        }
    }
}