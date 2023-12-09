namespace Resto.Framework.Data
{
    public interface IWithId<T>
    {
        T Id { get; }
    }
}