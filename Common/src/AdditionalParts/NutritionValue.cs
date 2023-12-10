using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class NutritionValue
    {
        public bool IsOnlyNullValues => !FatAmount.HasValue &&
                                        !FiberAmount.HasValue &&
                                        !CarbohydrateAmount.HasValue &&
                                        !EnergyAmount.HasValue;

        public void CopyValuesFrom([NotNull] NutritionValue other)
        {
            FatAmount = other.FatAmount;
            FiberAmount = other.FiberAmount;
            CarbohydrateAmount = other.CarbohydrateAmount;
            EnergyAmount = other.EnergyAmount;
        }
        
        public bool ValuesEqual([NotNull] NutritionValue other)
        {
            return FatAmount == other.FatAmount &&
                   FiberAmount == other.FiberAmount &&
                   CarbohydrateAmount == other.CarbohydrateAmount &&
                   EnergyAmount == other.EnergyAmount;
        }
    }
}
