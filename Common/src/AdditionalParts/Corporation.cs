using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Currency;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class Corporation : ICurrencyProvider
    {
        ICurrency ICurrencyProvider.Currency 
        {
            get { return Currency; }
        }

        public IList<CookingPlaceType> CookingPlaceTypes
        {   //TODO Сделать inline этого метода
            get { return EntityManager.INSTANCE.GetAllNotDeleted<CookingPlaceType>(); }
        }

        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.CORPORATION;
        }
    }
}