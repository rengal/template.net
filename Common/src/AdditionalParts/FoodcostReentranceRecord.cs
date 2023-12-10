using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class FoodcostReentranceRecord
    {
        [UsedImplicitly]
        public ProductGroup DishGroup
        {
            get { return Dish.Product.Parent; }
        }

        [UsedImplicitly]
        public string DishText
        {
            get { return Dish.ToString(); }
        }

        /// <summary>
        /// Суммарная прибыль, полученная с продажи блюда в рассматриваемом периоде
        /// </summary>
        [UsedImplicitly]
        public decimal ResearchPeriodDishProfit => ResearchPeriodDishSum - ResearchPeriodDishCost;

        /// <summary>
        /// Изменение прибыли по сравнению с базисным периодом.
        /// </summary>
        [UsedImplicitly]
        public decimal DeltaProfit => ResearchPeriodDishProfit - (BasePeriodDishSum - BasePeriodDishCost);

        /// <summary>
        /// Средняя наценка на блюдо в рассматриваемом периоде
        /// </summary>
        [UsedImplicitly]
        public decimal? DishMarkup => ResearchPeriodDishCost == 0m
            ? (decimal?) null
            : (ResearchPeriodDishSum - ResearchPeriodDishCost) / ResearchPeriodDishCost;
    }
}