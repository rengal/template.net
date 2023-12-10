using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

// ReSharper disable CheckNamespace
namespace Resto.Data
// ReSharper restore CheckNamespace
{
    public partial class BankStatementTransaction
    {
        #region Constructor

        public BankStatementTransaction(
            [NotNull] DepartmentEntity department,
            [CanBeNull] User counterpart, 
            [CanBeNull] AbstractInvoiceDocument document, 
            DateTime date, 
            [NotNull] Account account,
            [NotNull] Account corrAccount, 
            decimal sum, 
            string comment,
            [CanBeNull] Conception conception,
            [CanBeNull] CashFlowCategory category, 
            bool toFlag)
        {
            record = new AccountRegisterRecord();

            record.Created = true;
            record.Sum = sum;
            record.Date = date;

            record.Department = department;
            record.Comment = comment;
            record.CashFlowCategory = category;

            record.ToFlag = toFlag;

            // если есть документ привязываем проводку к нему
            if (document != null)
            {
                record.DocumentId = document.Id;
                record.DocumentType = document.DocumentType;
                record.Number = document.DocumentNumber;
                record.Conception = document.Conception;
            }

            this.account = account;
            record.Account = corrAccount;

            if (conception != null)
            {
                record.Conception = conception;
            }

            if (counterpart != null)
            {
                record.SecondCounterAgent = counterpart;
            }
        }

        #endregion
    }
}
