using Resto.Common;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class CentralStore
    {
        public CentralStore(CentralStore centralStore)
            : this(
                GuidGenerator.Next(),
                centralStore.Description,
                centralStore.Name,
                centralStore.Parent,
                "", "", centralStore.Address, centralStore.Category1, centralStore.Category2, centralStore.Category3,
                centralStore.Category4, centralStore.Category5,
                CompanySetup.Corporation.DefaultDistributionAlgorithm, 
                new VerificationActAccountsFilter(), 
                AccountingMethod.SLIDING_AVERAGE,
                CafeSetup.INSTANCE.BusinessDateSettings,
                CafeSetup.INSTANCE.OperationalDaySettings)
        {
        }

        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.CENTRALSTORE;
        }
    }
}