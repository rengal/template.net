using System;
using System.Linq;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisBalanceDocument
    {
        /// <summary>
        /// Тип документа который может быть создан на основе остатков ЕГАИС
        /// </summary>
        private enum LinkedDocumentType
        {
            TransferToShop,
            TransferFromShop,
            WriteoffFromShop
        }

        public override EgaisDocumentTypes Type => EgaisDocumentTypes.EGAIS_BALANCE;

        public override bool IsEditable => balanceStatus.Editable;

        public override string DocumentFullCaption
        {
            get
            {
                return DateIncoming == null 
                    ? BalanceType.GetLocalName()
                    : string.Format(Resources.EgaisBalanceDocumentFullCaption, BalanceType.GetLocalName(), DateIncoming);
            }
        }

        public bool ReadyToSend
        {
            get
            {
                return !SourceRarId.IsNullOrWhiteSpace() && IsEditable && !Deleted;
            }
        }

        public override string DocumentStatusStr
        {
            get { return BalanceStatus.GetLocalName(); }
        }

        public bool AllowCreateTransferToShop
        {
            get { return CheckAllowCreateLinkedDoc(LinkedDocumentType.TransferToShop); }
        }

        public bool AllowCreateTransferFromShop
        {
            get { return CheckAllowCreateLinkedDoc(LinkedDocumentType.TransferFromShop); }
        }

        public bool AllowCreateWriteoffFromShop
        {
            get { return CheckAllowCreateLinkedDoc(LinkedDocumentType.WriteoffFromShop); }
        }

        /// <summary>
        /// Проверяет можно ли создать внутренние документы по данным запросам остатков
        /// </summary>
        private bool CheckAllowCreateLinkedDoc(LinkedDocumentType docType)
        {
            if (BalanceStatus != EgaisBalanceDocumentStatus.REPLY_RECEIVED)
            {
                return false;
            }

            switch (docType)
            {
                case LinkedDocumentType.TransferToShop:
                    return BalanceType == EgaisBalanceDocumentType.STORE;
                case LinkedDocumentType.TransferFromShop:
                case LinkedDocumentType.WriteoffFromShop:
                    return BalanceType == EgaisBalanceDocumentType.SHOP;
            }
            return false;
        }
    }
}
