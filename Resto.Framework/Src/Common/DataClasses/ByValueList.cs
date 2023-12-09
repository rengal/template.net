namespace Resto.Framework.Data
{
    //This class is used to serialize a set of entities by value
    public sealed class ByValueList<T> : EntityList<T> where T : PersistedEntity
    {}
}