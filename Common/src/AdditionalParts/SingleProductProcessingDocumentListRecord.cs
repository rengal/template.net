using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class SingleProductProcessingDocumentListRecord
    {
        /// <summary>
        /// Разница по весу.
        /// </summary>
        public override decimal WeightDifferenceToShow
        {
            get { return WeightDifference.GetValueOrFakeDefault(); }
            set { WeightDifference = value; }
        }

        /// <summary>
        /// Среднний вес.
        /// </summary>
        public override decimal AverageWeightToShow
        {
            get { return AverageWeight.GetValueOrFakeDefault(); }
            set { AverageWeight = value; }
        }

        /// <summary>
        /// Отклонение по среднему весу
        /// </summary>
        public override decimal DeviationWeightToShow
        {
            get { return DeviationWeight.GetValueOrFakeDefault(); }
            set { DeviationWeight = value; }
        }

        public override decimal WeightInToShow
        {
            get { return WeightIn.GetValueOrFakeDefault(); }
        }

        public override decimal WeightOutToShow
        {
            get { return WeightOut.GetValueOrFakeDefault(); }
        }
    }
}