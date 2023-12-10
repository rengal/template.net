using Resto.Common.Interfaces;

namespace Resto.Data
{
    /// <summary>
    /// Запись, с ЕГАИС-транзакцией, для определения данных, соответсвующих справке 2
    /// </summary>
    public interface IEgaisRecordWithTransactionBRegId : IEgaisRecordWithBRegId
    {
        EgaisTransactionPart TransactionBRegId { get; set; }
    }
}
