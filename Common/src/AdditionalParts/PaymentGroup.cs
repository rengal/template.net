using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class PaymentGroup
    {
        public string LocalName
        {
            get { return this.GetLocalName(); }
        }
    }
}