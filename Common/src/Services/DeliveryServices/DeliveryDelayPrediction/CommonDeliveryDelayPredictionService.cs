using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;
using Resto.Data;

namespace Resto.Common.Services.DeliveryServices.DeliveryDelayPrediction
{
    public static class CommonDeliveryDelayPredictionService
    {
        [NotNull]
        public static DeliveryDelayPredictionResult CalculateDeliveryDelayPrediction(
            DateTimeOffset now,
            [NotNull] DeliveryTerminalWorkload deliveryTerminalWorkload,
            [NotNull] Dictionary<CookingPlaceType, int> newOrderCookingTimeByKitchens,
            long deliveryDurationInMinutes,
            long selfServiceDeliveryDuration)
        {
            if (deliveryTerminalWorkload == null)
                throw new ArgumentNullException(nameof(deliveryTerminalWorkload));
            if (newOrderCookingTimeByKitchens == null)
                throw new ArgumentNullException(nameof(newOrderCookingTimeByKitchens));

            var checkError = CheckInputData(deliveryTerminalWorkload, newOrderCookingTimeByKitchens.Keys.ToList());
            if (checkError != null)
                return DeliveryDelayPredictionResult.CreateUnsuccessResult(checkError);

            var newOrderCookingCompleteTime =
                CalculateOrderCookingCompletePrediction(now, deliveryTerminalWorkload, newOrderCookingTimeByKitchens);

            var predictedTimeOnOneSide = TimeSpan.FromMinutes(deliveryDurationInMinutes) - TimeSpan.FromMinutes(selfServiceDeliveryDuration);
            if (predictedTimeOnOneSide < TimeSpan.Zero)
                predictedTimeOnOneSide = TimeSpan.Zero;

            return DeliveryDelayPredictionResult.CreateSuccessResult(newOrderCookingCompleteTime + predictedTimeOnOneSide, newOrderCookingCompleteTime, predictedTimeOnOneSide);
        }

        private static string CheckInputData(DeliveryTerminalWorkload deliveryTerminalWorkload,
                                             List<CookingPlaceType> cookingPlacesFromOrder)
        {
            var allCookingPlaceFromFront = deliveryTerminalWorkload.KitchenWorkload.Select(k => k.CookingPlaceType);

            if (cookingPlacesFromOrder.Any(cp => !allCookingPlaceFromFront.Contains(cp)))
                return Resources.DataFromFrontNotActual;

            var firstCookingPlaceWithoutCook = cookingPlacesFromOrder
                .FirstOrDefault(cookingPlace => deliveryTerminalWorkload.KitchenWorkload.Single(kitchenWorkload => kitchenWorkload.CookingPlaceType.Equals(cookingPlace)).CookCount == 0);

            return firstCookingPlaceWithoutCook != null
                ? string.Format(Resources.CookingPlaceNotContainsCookFormat, firstCookingPlaceWithoutCook.NameLocal)
                : null;
        }

        /// <summary>
        /// Считаем прогноз времени окончания приготовления заказа.
        /// </summary>
        /// <param name="now">Текущее время точки.</param>
        /// <param name="deliveryTerminalWorkload">Загруженность кухонь</param>
        /// <param name="newOrderCookingTimeByKitchens">Время готовки нового заказа по каждому месту приготовления.</param>
        private static DateTimeOffset CalculateOrderCookingCompletePrediction(DateTimeOffset now, DeliveryTerminalWorkload deliveryTerminalWorkload,
            IDictionary<CookingPlaceType, int> newOrderCookingTimeByKitchens)
        {
            if (newOrderCookingTimeByKitchens.Count == 0)
                return now;

            var cookingTimeByKitchens = new Dictionary<CookingPlaceType, TimeSpan>();
            foreach (var newOrderCookingTimeByKitchen in newOrderCookingTimeByKitchens)
            {
                var kitchenWorkload = TimeSpan.FromMinutes(deliveryTerminalWorkload.KitchenWorkload
                    .Single(p => p.CookingPlaceType.Equals(newOrderCookingTimeByKitchen.Key))
                    .KitchenWorkloadInMinutes);
                cookingTimeByKitchens.Add(newOrderCookingTimeByKitchen.Key, TimeSpan.FromMinutes(newOrderCookingTimeByKitchen.Value) + kitchenWorkload);
            }

            var maxCookingCompleteTime = now;
            foreach (var cookingTimeByKitchen in cookingTimeByKitchens)
            {
                var cooksCount = deliveryTerminalWorkload.KitchenWorkload
                    .Single(p => p.CookingPlaceType.Equals(cookingTimeByKitchen.Key))
                    .CookCount;
                var cookingCompleteTimeForSelectedKitchen = now + new TimeSpan(cookingTimeByKitchen.Value.Ticks / cooksCount);
                if (cookingCompleteTimeForSelectedKitchen > maxCookingCompleteTime)
                    maxCookingCompleteTime = cookingCompleteTimeForSelectedKitchen;
            }

            return maxCookingCompleteTime;
        }

        [Pure]
        public static DeliveryDelayPredictionServiceMode ParseOrDefault(string rawValue)
        {
            DeliveryDelayPredictionServiceMode result;
            if (Enum.TryParse(rawValue.Replace("_", ""), true, out result))
                return result;

            return DeliveryDelayPredictionServiceMode.Disable;
        }
    }

    public enum DeliveryDelayPredictionServiceMode
    {
        Enable,
        Disable,
        LogMode
    }
}
