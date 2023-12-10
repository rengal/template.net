namespace Resto.Common.Plastek
{
    public class PlastekCustomerCardInfo
    {   
        private readonly double balance;
        private readonly PlastekCustomerCardStatus cardStatus;

        public PlastekCustomerCardInfo(double balance, PlastekCustomerCardStatus cardStatus)
        {
            this.balance = balance;
            this.cardStatus = cardStatus;
        }

        public double Balance
        {
            get { return balance; }
        }

        public PlastekCustomerCardStatus CardStatus
        {
            get { return cardStatus; }
        }
    }
}