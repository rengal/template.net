using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;

namespace Resto.Common.Services.DeliveryServices
{
    public class StartTimeCalculationRequest
    {
        public long? DeliveryDurationInMinutes { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsSelfService { get; set; }
        [CanBeNull]
        public DeliveryTerminalSettings DeliveryTerminalSettings { get; set; }
    }
}
