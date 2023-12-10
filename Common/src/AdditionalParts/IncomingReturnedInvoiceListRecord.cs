namespace Resto.Data
{
    public partial class IncomingReturnedInvoiceListRecord
    {
        /// <summary>
        /// Название расходной накладной
        /// </summary>
        public override string OutgoingInvoiceCaptionString
        {
            get { return OutgoingInvoiceCaption; }
        }

        /// <summary>
        /// Тип покупателя
        /// </summary>
        public override string SupplierTypeSting
        {
            get
            {
                return Counteragent != null && Counteragent.Type != null
                    ? Counteragent.Type
                    : string.Empty;
            }
        }
    }
}
