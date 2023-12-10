using System;
using System.Collections.Generic;
using Resto.Data;

namespace Resto.Common
{
    public static class DepartmentHelper
    {
        /// <summary>
        /// Возвращает обязательные счета, по котором должен строиться акт сверки.
        /// </summary>
        public static IEnumerable<Account> GetMandatoryVerificationActAccounts()
        {
            yield return CafeSetup.INSTANCE.ChartOfAccounts.SuppliersAccount;
            yield return CafeSetup.INSTANCE.ChartOfAccounts.ClientDeposits;
            yield return CafeSetup.INSTANCE.ChartOfAccounts.EmployeeDeposits;
        }

        /// <summary>
        /// Возвращает счета, по которым должен строиться акт сверки по умолчанию.
        /// </summary>
        public static IEnumerable<Account> GetDefaultVerificationActAccounts()
        {
            foreach (var mandatoryVerificationActAccount in GetMandatoryVerificationActAccounts())
            {
                yield return mandatoryVerificationActAccount;
            }
            yield return CafeSetup.INSTANCE.ChartOfAccounts.SuppliersAdvancePaymentAccount;
            yield return CafeSetup.INSTANCE.ChartOfAccounts.InternalSuppliersAccount;
            yield return CafeSetup.INSTANCE.ChartOfAccounts.EmployeeCurrentLiabilities;
        }
    }
}
