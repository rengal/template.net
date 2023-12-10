using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class FoodcostCouldSaveRecord
    {
        [UsedImplicitly]
        public string CurrentSuppliersString => CurrentSuppliers.AsString();
    }
}
