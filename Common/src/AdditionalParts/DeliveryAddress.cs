using System;

namespace Resto.Data
{
    public partial class DeliveryAddress : IAddress, ICloneable
    {
        protected void SetFields(DeliveryAddress copy)
        {
            copy.Line1 = Line1;
            copy.Line2 = Line2;
            copy.Region = Region;
            copy.Street = Street;
            copy.House = House;
            copy.Building = Building;
            copy.Flat = Flat;
            copy.Entrance = Entrance;
            copy.Floor = Floor;
            copy.Doorphone = Doorphone;
            copy.AdditionalInfo = AdditionalInfo;
        }

        public object Clone()
        {
            var copy = new DeliveryAddress();
            SetFields(copy);
            return copy;
        }
    }
}