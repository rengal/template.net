namespace Resto.Data
{
    partial class ConfigurablePaymentType
    {
        /// <summary>
        /// Настраиваемый тип оплаты - фискальный, если фискальным является его базовый тип оплаты.
        /// </summary>
        /// <returns> Является ли тип оплаты фискальным, если да - true, иначе - false</returns>
        /// <remarks>
        /// Симметричная реализация на сервере в resto.front.payment.ConfigurablePaymentType.isFiscalType.
        /// Бэковскую реализацию, сделанную по задаче RMS-40113, считаем первичной.
        /// </remarks>
        public override bool IsFiscalType()
        {
            return basePaymentType.IsFiscalType();
        }
    }
}
