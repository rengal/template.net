namespace Resto.Data
{
    public class PrepayItem
    {
        public decimal Sum { get; set; }

        public string PaymentType { get; set; }

        public string Comment { get; set; }

        public string CurrencyName { get; set; }

        public decimal SumInCurrency { get; set; }
    }
}