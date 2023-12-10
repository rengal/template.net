namespace Resto.Data
{
    public partial class RestaurantSectionObject
    {
        public RestaurantSectionObject Copy()
        {
            return (RestaurantSectionObject)MemberwiseClone();
        }
    }
}