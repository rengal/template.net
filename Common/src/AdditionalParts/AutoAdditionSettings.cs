using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Interfaces;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class AutoAdditionSettings : IAutoAdditionSettings
    {
        public static AutoAdditionSettings Instance
        {
            get { return EntityManager.INSTANCE.GetSingleton<AutoAdditionSettings>(); }
        }

        public IEnumerable<IAutoAdditionSettingsItem> GetItems()
        {
            return items;
        }

        public IEnumerable<Guid> AutoAdditionProductIds
        {
            get { return items.Where(aas => !aas.Deleted).Select(aas => aas.ProductId).Distinct(); }
        }
    }
}