using Resto.Common;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class Manufacture
    {
        public Manufacture(Manufacture manufacture)
            : this(
                GuidGenerator.Next(),
                manufacture.Description,
                manufacture.Name,
                manufacture.Parent,
                "", "", manufacture.Address, manufacture.Category1, manufacture.Category2, manufacture.Category3,
                manufacture.Category4, manufacture.Category5,
                CompanySetup.Corporation.DefaultDistributionAlgorithm,
                new VerificationActAccountsFilter(),
                AccountingMethod.SLIDING_AVERAGE,
                CafeSetup.INSTANCE.BusinessDateSettings,
                CafeSetup.INSTANCE.OperationalDaySettings)
        {
        }

        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.MANUFACTURE;
        }
    }
}