using System;
using System.Collections.Generic;

namespace Resto.Data
{
    /// <summary>
    /// Бэковское дополнение для работы с классом Account.
    /// </summary>
    public partial class Account : IComparable, IComparable<Account>
    {
        public string IdString
        {
            get { return Id.ToString(); }
        }

        public override string ToString()
        {
            return NameLocal;
        }

        public CounteragentType CounteragentType
        {
            get { return Type.CounteragentType; }
        }

        public bool IsCashFlowAccount()
        {
            return type == AccountType.CASH
                || CafeSetup.INSTANCE.ChartOfAccounts.SuppliersAdvancePaymentAccount.Equals(this)
                || CafeSetup.INSTANCE.ChartOfAccounts.MoneyGivenOnAccount.Equals(this);
        }

        /// <summary>
        /// Возвращает true, если текущему пользователю возможно редактировать транзакции данного счета.
        /// </summary>
        public bool IsEditableCashBook
        {
            get
            {
                User u = ServerSession.CurrentSession.GetCurrentUser();
                return u.IsAdministrator || (Permission.CAN_EDIT_CASH_BOOK.IsAllowedForCurrentUser && (ResponsibleUsers.Count == 0 || ResponsibleUsers.Contains(u)));                
            }
        }
        
        #region IComparable Members

        /// <summary>
        /// Сравнивает текущий объект с указанным по названию. В случае совпадения названий, объекты сравниваются по идентификаторам.
        /// </summary>
        public int CompareTo(object obj)
        {
            return CompareTo((Account)obj);
        }

        #endregion IComparable Members

        #region IComparable<Account> Members

        /// <summary>
        /// Сравнивает текущий объект с указанным по названию. В случае совпадения названий, объекты сравниваются по идентификаторам.
        /// </summary>
        public int CompareTo(Account other)
        {
            if (other == null)
                return 1;
            if (this == other || Equals(other))
            {
                return 0;
            }

            int res = StringComparer.CurrentCulture.Compare(NameLocal, other.NameLocal);
            if (res == 0)
                res = Comparer<Guid>.Default.Compare(Id, other.Id);
            return res;
        }

        #endregion IComparable<Account> Members
    }
}
