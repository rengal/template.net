using System;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Common.XmlSerialization;
using Resto.Framework.Xml;

namespace Resto.Data
{
    public partial class Department
    {
        public static string ALL_DEPARTMENTS = Resources.DepartmentAllTraders;

        public Department(Department department)
            : this(GuidGenerator.Next(), department.Description, department.Name, department.Parent,
                "", "", department.Address, department.Category1, department.Category2, department.Category3,
                department.Category4, department.Category5,
                CompanySetup.Corporation.DefaultDistributionAlgorithm,
                Serializer.DeepClone(department.VerificationActAccountsFilter),
                department.AccountingMethod,
                CafeSetup.INSTANCE.BusinessDateSettings,
                CafeSetup.INSTANCE.OperationalDaySettings,
                department.ExchangeDirPath, department.CashSystem, department.UnloadTime.GetValueOrDefault(),
                department.LoadTime.GetValueOrDefault())
        {
        }

        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.DEPARTMENT;
        }

        public string IsRegistered
        {
            get { return Registered ? Resources.CommonAdditionalPartsYes : Resources.CommonAdditionalPartsNo; }
        }

        public static Department EmptyDepartment
        {
            get
            {
                return new Department(Guid.Empty, string.Empty, string.Empty, null,
                    "", "", "", "", "", "", "", "",
                    DistributionAlgorithmType.DISTRIBUTION_NOT_SPECIFIED,
                    new VerificationActAccountsFilter(),
                    AccountingMethod.SLIDING_AVERAGE,
                    CafeSetup.INSTANCE.BusinessDateSettings,
                    CafeSetup.INSTANCE.OperationalDaySettings,
                    "", null, null, null);
            }
        }
    }
}