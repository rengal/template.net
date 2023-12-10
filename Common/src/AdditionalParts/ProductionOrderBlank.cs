using System;

namespace Resto.Data
{
    public sealed partial class ProductionOrderBlank
    {
        /// <summary>
        /// Бланк получен через франшизу
        /// </summary>
        public bool IsFranchiseReplica => FranchiseMasterId != null;

        /// <summary>
        /// Возвращает true, если бланк действует на указанную дату
        /// </summary>
        /// <param name="date">Дата проверки действия бланка</param>
        public bool IsActual(DateTime date)
        {
            return DateStart.Date <= date.Date && DateEnd.Date >= date.Date;
        }
    }
}