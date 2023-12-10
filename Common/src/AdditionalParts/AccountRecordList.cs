using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Бэковское дополнение для работы с классом <see cref="AccountRecordList"/>.
    /// </summary>   
    public partial class AccountRecordList
    {
        public AccountRecordList(Account account, decimal? startBalance, decimal? sumTotal, IEnumerable<AccountRegisterRecord> records):
            this(account, startBalance, sumTotal, true)
        {
            this.records = records.ToList();
        }
    }
}
