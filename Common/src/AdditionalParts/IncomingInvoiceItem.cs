namespace Resto.Data
{
    public sealed partial class IncomingInvoiceItem
    {
        public override bool IsEmptyItem
        {
            get { return base.IsEmptyItem && SupplierProduct == null; }
        }
    }
}