namespace Resto.Data
{
    public partial class IncomingCashOrder
    {
        public override DocumentType DocumentType
        {
            get { return DocumentType.INCOMING_CASH_ORDER; }
        }
    }
}
