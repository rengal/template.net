using Resto.Common;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class CentralOffice
    {
        public CentralOffice(CentralOffice centralOffice)
            : this(GuidGenerator.Next(), centralOffice.Description, centralOffice.Name, centralOffice.Parent,
                "", "", centralOffice.Address, centralOffice.Category1, centralOffice.Category2, centralOffice.Category3,
                centralOffice.Category4, centralOffice.Category5,
                CompanySetup.Corporation.DefaultDistributionAlgorithm,
                new VerificationActAccountsFilter(),
                AccountingMethod.SLIDING_AVERAGE,
                CafeSetup.INSTANCE.BusinessDateSettings,
                CafeSetup.INSTANCE.OperationalDaySettings)
        {
        }

        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.CENTRALOFFICE;
        }
    }
}