using System.Linq;

namespace Resto.Data
{
    public partial class NonCashPaymentType
    {
        private static readonly string[] NonFiscalPaymentTypeNames = {
            PaymentSystemNames.Hoist,
            PaymentSystemNames.LuckyTicket,
            PaymentSystemNames.Edelweiss,
            PaymentSystemNames.Epitome
        };

        public string GetCardTypeName()
        {
            return paymentSystem != null ? paymentSystem.NameLocal : cardTypeName;
        }

        public bool IsPlatius()
        {
            return PaymentSystem != null && PaymentSystem.IsIikoNet;
        }

        /// <summary>
        /// Фискальными являются безналичные типы оплат, если это:
        /// * Банковские карты
        /// * Безналичный расчет - если в его настройках стоит галка "Является фискальным" (сюда же отностися тип оплаты "Ваучер"). Кроме Hoist, LuckyTicket, Edelweiss, Epitome, PrtrolPlus - они всегда нефискальные
        /// </summary>
        /// <returns> Является ли тип оплаты фискальным, если да - true, иначе - false</returns>
        /// <remarks>
        /// Симметричная реализация на сервере в resto.front.payment.NonCashPaymentType.isFiscalType.
        /// Бэковскую реализацию, сделанную по задаче RMS-40113, считаем первичной.
        /// </remarks>
        public override bool IsFiscalType()
        {
            return PaymentGroup == PaymentGroup.CARD ||
                   PrintCheque && !NonFiscalPaymentTypeNames.Contains(CardTypeName);
        }
    }
}
