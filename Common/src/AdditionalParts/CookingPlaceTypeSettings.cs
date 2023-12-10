using System;
using Resto.Framework.Attributes.JetBrains;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    public sealed partial class CookingPlaceTypeSettings
    {
        public CookingPlaceTypeSettings([NotNull] RestaurantSection cookingPlace, [NotNull] IEnumerable<RestaurantSection> additionalCookingPlaces)
            : this(cookingPlace)
        {
            if (cookingPlace == null)
                throw new ArgumentNullException(nameof(cookingPlace));
            if (additionalCookingPlaces == null)
                throw new ArgumentNullException(nameof(additionalCookingPlaces));

            this.additionalCookingPlaces = new HashSet<RestaurantSection>(additionalCookingPlaces.Where(cp => cp.Id != cookingPlace.Id));
        }

        //[NotNull, Pure]
        //public IEnumerable<RestaurantSection> GetAllCookingPlaces() => AdditionalCookingPlaces.StartWith(CookingPlace);
        //todo debugnow
    }
}