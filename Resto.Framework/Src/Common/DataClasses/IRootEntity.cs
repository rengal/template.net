namespace Resto.Framework.Data
{
    public interface IRootEntity : IEntity
    {
        void AfterDeserialization();
    }
}