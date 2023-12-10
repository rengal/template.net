using Resto.Framework.Data;

namespace Resto.Data
{
    [DataClass("CashRegisterVatData")]
    public class CashRegisterVatData : FiscalRegisterTaxItem
    {
        private bool haveTaxAmount;
        private decimal taxAmount;

        private bool haveTaxableSum;
        private decimal taxableSum;

        public bool HaveTaxAmount
        {
            get { return haveTaxAmount; }
            set { haveTaxAmount = value; }
        }

        public decimal TaxAmount
        {
            get { return taxAmount; }
            set { taxAmount = value; }
        }

        public bool HaveTaxableSum
        {
            get { return haveTaxableSum; }
            set { haveTaxableSum = value; }
        }

        public decimal TaxableSum
        {
            get { return taxableSum; }
            set { taxableSum = value; }
        }
    }
}
