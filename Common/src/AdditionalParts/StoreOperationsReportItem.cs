using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Localization;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class StoreOperationsReportItem
    {
        public string GetItemName()
        {
            if (DocumentType != null)
            {
                if (DocumentType.Equals(DocumentType.INCOMING_INVOICE))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNamePN, (SecondaryCounteragent != null ? SecondaryCounteragent.NameLocal: ""));
                }
                if (DocumentType.Equals(DocumentType.INTERNAL_TRANSFER))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameVP, (Incoming ? SecondaryAccount.NameLocal + @"/" + PrimaryStore.NameLocal : PrimaryStore.NameLocal + @"/" + SecondaryAccount.NameLocal));
                }
                if (DocumentType.Equals(DocumentType.PRODUCTION_DOCUMENT))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameAP, (Incoming ? SecondaryAccount.NameLocal + @"/" + PrimaryStore.NameLocal : PrimaryStore.NameLocal + @"/" + SecondaryAccount.NameLocal));
                }
                if (DocumentType.Equals(DocumentType.TRANSFORMATION_DOCUMENT))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameAPR, (Incoming ? SecondaryAccount.NameLocal + @"/" + PrimaryStore.NameLocal : PrimaryStore.NameLocal + @"/" + SecondaryAccount.NameLocal));
                }
                if (DocumentType.Equals(DocumentType.INCOMING_INVENTORY))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameINV, (SecondaryAccount != null ? SecondaryAccount.NameLocal: ""));
                }
                if (DocumentType.Equals(DocumentType.OUTGOING_INVOICE))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameRN, (SecondaryCounteragent != null ? SecondaryCounteragent.NameLocal: ""));
                }
                if (DocumentType.Equals(DocumentType.WRITEOFF_DOCUMENT))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameAS, (SecondaryAccount != null ? SecondaryAccount.NameLocal: ""));
                }
                if (DocumentType.Equals(DocumentType.SALES_DOCUMENT))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameAR, (SecondaryAccount != null ? SecondaryAccount.NameLocal: ""));
                }
                if (DocumentType.Equals(DocumentType.RETURNED_INVOICE))
                {
                    return string.Format(Resources.StoreOperationsReportItemGetItemNameVN, SecondaryCounteragent != null ? SecondaryCounteragent.NameLocal: "");
                }
                return DocumentType.GetLocalName();
            }
            if (Type != null)
                return Type.GetLocalName();
            return "";
        }

        public string GetReportName()
        {
            if (DocumentType != null)
            {
                if (DocumentType.Equals(DocumentType.INCOMING_INVOICE))
                {
                    return "ПН " + GetReportCounteragentName(SecondaryCounteragent);
                }
                if (DocumentType.Equals(DocumentType.OUTGOING_INVOICE))
                {
                    return "РН " + GetReportCounteragentName(SecondaryCounteragent);
                }
                if (DocumentType.Equals(DocumentType.RETURNED_INVOICE))
                {
                    return string.Format("ВН {0}", GetReportCounteragentName(SecondaryCounteragent));
                }
            }
            return GetItemName();
        }

        private static string GetReportCounteragentName([CanBeNull]User counteragent)
        {
            return counteragent != null
                       ? (!string.IsNullOrEmpty(counteragent.Company)
                              ? counteragent.Company
                              : counteragent.NameLocal)
                       : "";
        }
    }
}