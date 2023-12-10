using Resto.Common.Interfaces;
using Resto.Framework.Data;

namespace Resto.Data
{
    public sealed partial class GoodRecord : IRestoGridRow
    {
        [Transient]
        public decimal costWhole;
        [Transient]
        public decimal costForOne;

        public GoodRecord(int number, Product product, MeasureUnit unit, decimal? amount, decimal? remainder, decimal costWhole, decimal costForOne)
            : this(number, product, unit, amount, remainder)
        {
            CostWhole = costWhole;
            CostForOne = costForOne;
        }

        public decimal CostWhole
        {
            get { return costWhole; }
            set { costWhole = value; }
        }

        public decimal CostForOne
        {
            get { return costForOne; }
            set { costForOne = value; }
        }

        #region IRestoGridRow Members

        public bool IsEmptyRecord
        {
            get { return product == null; }
        }

        #endregion
    }
}