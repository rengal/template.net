using System;
using Resto.Common.Properties;
using Resto.Data;

namespace Resto.Common.Extensions
{
    public static class OfficialEmployeeRoleExtension
    {
        public static string Name(this OfficialEmployeeRole role)
        {
            switch (role)
            {
                case OfficialEmployeeRole.LEADER:
                    return Resources.OfficialEmployeeRoleExtensionLeaderName;
                case OfficialEmployeeRole.ACCOUNTANT:
                    return Resources.OfficialEmployeeRoleExtensionAccountantName;
                case OfficialEmployeeRole.TECHNOLOGIST:
                    return Resources.OfficialEmployeeRoleExtensionTechnologistName;
                case OfficialEmployeeRole.WORKS_MANAGER:
                    return Resources.OfficialEmployeeRoleExtensionWorkManagerName;
            }
            throw new NotSupportedException();
        }
    }
}