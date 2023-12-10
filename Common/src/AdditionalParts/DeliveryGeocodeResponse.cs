using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    partial class DeliveryGeocodeResponse
    {
        public bool Valid
        {
            get { return Latitude.HasValue && Longitude.HasValue; }
        }

        public DeliveryZonePoint ToPoint()
        {
            if (!Valid)
            {
                throw new InvalidOperationException();
            }
            return new DeliveryZonePoint(Latitude.GetValueOrFakeDefault(), Longitude.GetValueOrFakeDefault());
        }
    }
}