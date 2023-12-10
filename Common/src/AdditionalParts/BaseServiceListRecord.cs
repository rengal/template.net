using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class BaseServiceListRecord
    {
        public override Account RevenueAccountData
        {
            get { return RevenueAccount; }
        }

        public override string IncomingInvoiceCaptionString
        {
            get { return InvoiceIncomingNumber; }
        }

        public override string InvoicesAsString
        {
            get { return Invoice; }
        }

        public override string DueDateString
        {
            get { return (DueDate != null) ? DueDate.GetValueOrFakeDefault().ToShortDateString() : ""; }
        }
        public override string DocNumberString
        {
            get { return DocNumber; }
        }
        public override string DocDateString
        {
            get { return DocDate != null ? DocDate.GetValueOrFakeDefault().ToShortDateString() : ""; }
        }
        public override string SupplierString
        {
            get { return Supplier != null ? Supplier.NameLocal: string.Empty; }
        }
        public override string SupplierTypeSting
        {
            get { return Supplier != null ? Supplier.Type : string.Empty; }
        }

        public override EditableDocumentType EditableDocumentType
        {
            get { return EditableDocumentType.GetDocumentType(this); }
        }
    }
}