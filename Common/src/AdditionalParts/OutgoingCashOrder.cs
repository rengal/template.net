namespace Resto.Data
{
    public partial class OutgoingCashOrder
    {
        public override DocumentType DocumentType
        {
            get { return DocumentType.OUTGOING_CASH_ORDER; }
        }
    }
}
