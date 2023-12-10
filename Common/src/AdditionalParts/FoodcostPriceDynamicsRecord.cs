using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class FoodcostPriceDynamicsRecord
    {
        /// <summary>
        /// Краткие текстовые описания рекомендаций
        /// </summary>
        private static readonly IDictionary<FoodcostRecommendation, string> ShortRecommendations =
            new Dictionary<FoodcostRecommendation, string>
            {
                {FoodcostRecommendation.CHANGE_SUPPLIER, Resources.FoodcostRecommendationSupplier},
                {FoodcostRecommendation.CHANGE_MARKUP, Resources.FoodcostRecommendationMarkup},
                {FoodcostRecommendation.LOSSES_EXPECTED, Resources.FoodcostRecommendationLosses },
            };

        public MeasureUnit MainUnit => Product.MainUnit;

        /// <summary>
        /// Абсолютное изменение средневзвешенной цены закупки товара
        /// (разница между исследуемым и базисным периодами)
        /// </summary>
        public decimal? AveragePriceDeltaValue => (AveragePriceResearchPeriod - AveragePriceBasePeriod).ZeroAsNull();

        /// <summary>
        /// Относительное изменение средневзвешенной цены закупки товара
        /// (разница между исследуемым и базисным периодами)
        /// </summary>
        [UsedImplicitly]
        public decimal? AveragePriceDeltaPercent => (AveragePriceResearchPeriod - AveragePriceBasePeriod).SafeDivide(AveragePriceBasePeriod, true);

        /// <summary>
        /// Разница с минимальной ценой, %
        /// </summary>
        [UsedImplicitly]
        public decimal? NullableLastAndMinPricesDeltaPercent => LastAndMinPricesDeltaPercent.ZeroAsNull();

        /// <summary>
        /// Относительная разница между текущей (последней) ценой закупки и
        /// средневзвешенной ценой закупки товара в базисном периоде
        /// </summary>
        [UsedImplicitly]
        public decimal? LastAndBasePricesDeltaPercent => (LastPriceValue - AveragePriceBasePeriod).SafeDivide(AveragePriceBasePeriod, true);

        /// <summary>
        /// Абсолютная разница между текущей (последней) ценой закупки и
        /// средневзвешенной ценой закупки товара в базисном периоде
        /// </summary>
        [UsedImplicitly]
        public decimal? LastAndBasePricesDeltaValue => (LastPriceValue - AveragePriceBasePeriod).ZeroAsNull();

        /// <summary>
        /// Абсолютная разница между натуральным объёмом продаж товара в исследуемом и базисном периодах
        /// </summary>
        [UsedImplicitly]
        public decimal? AmountDeltaValue => (AmountResearchPeriod - AmountBasePeriod).ZeroAsNull();

        /// <summary>
        /// Относительная разница между натуральным объёмом продаж товара в исследуемом и базисном периодах
        /// </summary>
        [UsedImplicitly]
        public decimal? AmountDeltaPercent => AmountDeltaValue.SafeDivide(AmountBasePeriod, true);

        /// <summary>
        /// Потери/экономия из-за изменения средней цены
        /// </summary>
        /// <remarks>
        /// <see cref="LossesSavingAveragePriceChanging"/> в бэке нужно отображать инвертированным.
        /// </remarks>
        [UsedImplicitly]
        public decimal? InvertedLossesSavingAveragePriceChanging =>
            LossesSavingAveragePriceChanging == 0m ? null : -LossesSavingAveragePriceChanging;

        /// <summary>
        /// Ожидаемые потери/экономия
        /// </summary>
        /// <remarks>
        /// <see cref="ExpectedLossesSaving"/> в бэке нужно отображать инвертированным.
        /// </remarks>
        [UsedImplicitly]
        public decimal? InvertedExpectedLossesSaving => ExpectedLossesSaving == 0m ? null : -ExpectedLossesSaving;

        public int Reentrance
        {
            get { return Dishes.Count; }
        }

        public string RecommendationsText
        {
            get { return Recommendations == null ? string.Empty : Recommendations.Select(r => ShortRecommendations[r]).JoinWithComma(); }
        }
    }
}