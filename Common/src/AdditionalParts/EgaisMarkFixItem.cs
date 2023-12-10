using System.Linq;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisMarkFixItem : IEgaisRecordWithTransactionBRegId
    {
        private EgaisTransactionPart transactionBRegId;

        /// <summary>
        /// Транзакция со справкой 2
        /// </summary>
        public EgaisTransactionPart TransactionBRegId
        {
            get { return transactionBRegId; }
            set
            {
                transactionBRegId = value;
                BRegId = transactionBRegId != null ? value.Key.BRegId : string.Empty;
            }
        }

        public override bool IsCorrect
        {
            get { return !BRegId.IsNullOrWhiteSpace() && Marks.Any(); }
        }
    }
}
