namespace Resto.Data
{
    public partial class ChequeCardPayment
    {
        private bool isFiscal;

        /// <summary>
        /// Фискальный ли тип
        /// </summary>
        public bool IsFiscal
        {
            get { return isFiscal; }
            set { isFiscal = value; }
        }

        private int chequeNumber;

        /// <summary>
        /// Номер чека для платежа
        /// </summary>
        public int ChequeNumber
        {
            get { return chequeNumber; }
            set { chequeNumber = value; }
        }

        private string paymentRegisterId;

        /// <summary>
        /// Идентификатор регистра ФР
        /// </summary>
        public string PaymentRegisterId
        {
            get { return paymentRegisterId; }
            set { paymentRegisterId = value; }
        }

        private bool isDefaultNonCash;

        /// <summary>
        /// Является ли PaymentRegisterId регистром по умолчанию
        /// </summary>
        public bool IsDefaultNonCash
        {
            get { return isDefaultNonCash; }
            set { isDefaultNonCash = value; }
        }

        private string currencyName;

        /// <summary>
        /// Название валюты (для наличных типов)
        /// </summary>
        public string CurrencyName
        {
            get { return currencyName; }
            set { currencyName = value; }
        }

        private decimal sumInCurrency;

        /// <summary>
        /// Сумма в валюте (для наличных типов)
        /// </summary>
        public decimal SumInCurrency
        {
            get { return sumInCurrency; }
            set { sumInCurrency = value; }
        }
    }
}