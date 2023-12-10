namespace Resto.Data
{
    public sealed partial class ReturnedInvoice
    {
        public override DocumentType DocumentType
        {
            get
            {
                return DocumentType.RETURNED_INVOICE;
            }
        }
    }
}