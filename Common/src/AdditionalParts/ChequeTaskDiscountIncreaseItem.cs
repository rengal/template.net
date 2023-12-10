using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public sealed class ChequeTaskDiscountIncreaseItem
    {
        #region Fields
        private readonly string name;
        private readonly decimal percent;
        private readonly decimal sum;
        private readonly string cardNumber;
        #endregion

        #region Ctors
        [UsedImplicitly]
        private ChequeTaskDiscountIncreaseItem()
        {}

        public ChequeTaskDiscountIncreaseItem(string name, decimal percent, decimal sum, string cardNumber = null)
        {
            this.name = name;
            this.percent = percent;
            this.sum = sum;
            this.cardNumber = cardNumber;
        }
        #endregion

        #region Props
        public string Name
        {
            get { return name; }
        }

        public decimal Percent
        {
            get { return percent; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public string CardNumber
        {
            get { return cardNumber; }
        }

        public bool ByCard
        {
            get { return cardNumber != null; }
        }
        #endregion
    }
}