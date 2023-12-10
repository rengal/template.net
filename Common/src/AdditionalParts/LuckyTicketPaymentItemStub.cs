using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class LuckyTicketPaymentItemStub
    {
        public override string ToString()
        {
            return Method + " " + Amount.GetValueOrFakeDefault() + " " + LuckyTicketTransaction;
        }
    }
}