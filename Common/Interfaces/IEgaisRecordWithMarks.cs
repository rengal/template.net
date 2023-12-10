using System.Collections.Generic;

namespace Resto.Data
{
    /// <summary>
    /// Запись, содержащая список акцизных марок
    /// </summary>
    public interface IEgaisRecordWithMarks
    {
        /// <summary>
        /// Все марки записи
        /// </summary>
        List<EgaisMark> Marks { get; }

        /// <summary>
        /// Номер записи в гриде
        /// </summary>
        int Num { get; }

        /// <summary>
        /// Количество марок
        /// </summary>
        int MarkCount { get; }
    }
}
