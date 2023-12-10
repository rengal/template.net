using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Описывает типы возможных контрагентов в системе.
    /// </summary>
    public partial class CounteragentType
    {
        /// <summary>
        /// Тип контрагента.
        /// </summary>
        public UserType UserType
        {
            get
            {
                UserType userType = UserType.NONE;

                if (_Value == EMPLOYEE._Value)
                {
                    userType = UserType.EMPLOYEE;
                }
                else if (_Value == CLIENT._Value)
                {
                    userType = UserType.CLIENT;
                }
                else if (_Value == SUPPLIER._Value)
                {
                    userType = UserType.SUPPLIER;
                }
                else if (_Value == COUNTERAGENT._Value)
                {
                    userType = UserType.ALL;
                }

                return userType;
            }
        }

        public bool IsNone
        {
            get { return UserType == UserType.NONE; }
        }

        public bool IsSupplier
        {
            get { return UserType == UserType.SUPPLIER; }
        }

        public bool IsClient
        {
            get { return UserType == UserType.CLIENT; }
        }

        public bool IsEmployee
        {
            get { return UserType == UserType.EMPLOYEE; }
        }

        /// <summary>
        /// Счёт, который соответствует типу контрагента.
        /// Порядок проверок важен, см. также resto.back.store.DocumentHelper#getRevenueDebitAccountFor
        /// </summary>
        public Account Account
        {
            get
            {
                if (IsSupplier)
                {
                    return CafeSetup.INSTANCE.ChartOfAccounts.SuppliersAccount;
                }
                if (IsClient)
                {
                    return CafeSetup.INSTANCE.ChartOfAccounts.ClientDeposits;
                }
                if (IsEmployee)
                {
                    return CafeSetup.INSTANCE.ChartOfAccounts.EmployeeCurrentLiabilities;
                }
                if (IsNone)
                {
                    return CafeSetup.INSTANCE.ChartOfAccounts.RevenueIncoming;
                }

                throw new RestoException(string.Format("Unknown user type {0} for counteragent type {1}",
                    UserType, _Value));
            }
        }
    }
}
