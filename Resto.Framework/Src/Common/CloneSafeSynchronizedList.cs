namespace System.Collections.Generic
{
    public static class CollectionsHelper
    {
        public static CloneSafeSynchronizedList<T> ToCloneSafeList<T>(this IEnumerable<T> list)
        {
            return new CloneSafeSynchronizedList<T>(list);
        }
    }

    public class CloneSafeSynchronizedList<T> : SynchronizedCollection<T>, ICloneableList<T>
    {
        public CloneSafeSynchronizedList()
        { }

        public CloneSafeSynchronizedList(IEnumerable<T> list)
            : base(new object(), list)
        { }

        public virtual ICloneableList<T> Clone()
        {
            lock (SyncRoot)
            {
                return new CloneSafeSynchronizedList<T>(this);
            }
        }
    }

    public sealed class DeepCloneSafeSynchronizedList<T> : CloneSafeSynchronizedList<T> where T : ICloneable
    {
        public DeepCloneSafeSynchronizedList()
        { }

        public DeepCloneSafeSynchronizedList(IEnumerable<T> list)
            : base(list)
        { }

        public override ICloneableList<T> Clone()
        {
            lock (SyncRoot)
            {
                var clone = new DeepCloneSafeSynchronizedList<T>();
                foreach (var item in Items)
                {
                    clone.Add((T)item.Clone());
                }
                return clone;
            }
        }
    }
}