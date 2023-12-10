using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Services.DeliveryServices.DeliveryDelayPrediction
{
    public sealed class DeliveryDelayPredictionResult
    {
        /// <summary>
        /// Расчетное время, к которому доставка будет доставлена. 
        /// </summary>
        public DateTimeOffset? PredictedDeliveryTime { get; private set; }

        /// <summary>
        /// Прогнозируемое время окончания готовки заказа.
        /// </summary>
        public DateTimeOffset? PredictedCookingCompleteTime { get; private set; }

        /// <summary>
        /// Предполагаемое время в пути.
        /// Расчитывается на основание настроек доставки.
        /// </summary>
        public TimeSpan? PredictedTimeOnWay { get; private set; }

        /// <summary>
        /// Признак того, что расчет был успешен.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string Error { get; private set; }

        private DeliveryDelayPredictionResult() { }

        [NotNull]
        public static DeliveryDelayPredictionResult CreateSuccessResult(DateTimeOffset predictedDeliveryTime, DateTimeOffset predictedCookingCompleteTime, TimeSpan predictedTimeOnWay)
        {
            return new DeliveryDelayPredictionResult
            {
                PredictedDeliveryTime = predictedDeliveryTime,
                PredictedCookingCompleteTime = predictedCookingCompleteTime,
                PredictedTimeOnWay = predictedTimeOnWay,
                Success = true,
            };
        }

        [NotNull]
        public static DeliveryDelayPredictionResult CreateUnsuccessResult(string error)
        {
            return new DeliveryDelayPredictionResult
            {
                Error = error,
                Success = false,
            };
        }
    }
}
