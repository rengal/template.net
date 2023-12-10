namespace Resto.Data
{
    public partial class EmployeeTransactionInfo
    {
        public decimal SignedSum
        {
            get { return Type == TransactionType.CREDIT || Type == TransactionType.EMPLOYEE_PAYMENT ? -Sum : Sum; }
        }
    }
}
