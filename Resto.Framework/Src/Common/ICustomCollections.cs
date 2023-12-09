namespace System.Collections.Generic
{
    public interface ICloneableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        ICloneableDictionary<TKey, TValue> Clone();
    }

    public interface ICloneableList<T> : IList<T>
    {
        ICloneableList<T> Clone();
    }
}