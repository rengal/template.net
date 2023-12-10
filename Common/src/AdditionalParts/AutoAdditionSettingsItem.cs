using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Interfaces;

namespace Resto.Data
{
    public partial class AutoAdditionSettingsItem : IAutoAdditionSettingsItem
    {
        public Guid ProductId
        {
            get { return Product.Id; }
        }

        public IList<Guid> InitiatorProductsId
        {
            get { return InitiatorProducts.Select(p => p.Id).ToList(); }
        }
    }
}