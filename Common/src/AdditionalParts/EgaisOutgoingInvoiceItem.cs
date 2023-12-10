using System;

namespace Resto.Data
{
    public partial class EgaisOutgoingInvoiceItem
    {
        public EgaisOutgoingInvoiceItem(Guid id, int num, MeasureUnit amountUnit, Guid? containerId, EgaisProductInfo productInfo,
            string packId, string party, string aRegId, string bRegId, EgaisOrganizationInfo originalClient)
            : this(id, num, null, amountUnit, containerId, productInfo,
                null, packId, party, aRegId, bRegId, null, null, null, null, originalClient, null)
        { }
    }
}
