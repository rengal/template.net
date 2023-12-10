using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class OrderGuest
    {
        [HasDefaultValue]
        private int firstOrderNumber;
        public int FirstOrderNumber
        {
            get { return firstOrderNumber; }
            set { firstOrderNumber = value; }
        }

        [HasDefaultValue]
        private int? penultimateOrderTableNumber;
        public int? PenultimateOrderTableNumber
        {
            get { return penultimateOrderTableNumber; }
            set { penultimateOrderTableNumber = value; }
        }

        [HasDefaultValue]
        private int? previousOrderTableNumber;
        public int? PreviousOrderTableNumber
        {
            get { return previousOrderTableNumber; }
            set { previousOrderTableNumber = value; }
        }
    }
}