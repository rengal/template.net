namespace Resto.Data
{
    /// <summary>
    /// Маркировка товара в соответствии со протоколом ФФД 1.2
    /// </summary>
    public sealed class Ffd12ChequeSaleData
    {
        public Ffd12ChequeSaleData()
        {
        }

        public Ffd12ChequeSaleData(string markingCode, int? status, bool? isFractionalAmount, int? amountNumerator,
            int? amountDenominator, int unit)
        {
            MarkingCode = markingCode;
            Status = status;
            IsFractionalAmount = isFractionalAmount;
            AmountNumerator = amountNumerator;
            AmountDenominator = amountDenominator;
            Unit = unit;
        }

        /// <summary>Marking code.</summary>
        public string MarkingCode { get; set; }

        /// <summary>New status.</summary>
        public int? Status { get; set; }

        /// <summary>Fractional sale flag.</summary>
        public bool? IsFractionalAmount { get; set; }

        /// <summary>Fractional amount numerator.</summary>
        public int? AmountNumerator { get; set; }

        /// <summary>Fractional amount denomonator (number of items in package with marking).</summary>
        public int? AmountDenominator { get; set; }

        /// <summary>Unit of measurement according to Ffd 1.2</summary>
        public int Unit { get; set; }
    }
}