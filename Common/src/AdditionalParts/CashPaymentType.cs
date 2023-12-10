namespace Resto.Data
{
    partial class CashPaymentType
    {
        /// <summary>
        /// Тип оплаты "Наличные" является фискальным
        /// </summary>
        /// <returns> Является ли тип оплаты фискальным, если да - true, иначе - false</returns>
        /// <remarks>
        /// Симметричная реализация на сервере в resto.front.payment.CashPaymentType.isFiscalType.
        /// Бэковскую реализацию, сделанную по задаче RMS-40113, считаем первичной.
        /// </remarks>
        public override bool IsFiscalType()
        {
            return true;
        }
    }
}
