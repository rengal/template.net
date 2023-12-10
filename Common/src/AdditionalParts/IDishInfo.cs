namespace Resto.Data
{
    public interface IDishInfo
    {
        string Name { get; }
        string Num { get; }
        CookingPlaceType PlaceType { get; }
        Product OriginalProduct { get; }
    }
}