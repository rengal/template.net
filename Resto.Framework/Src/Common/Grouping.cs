using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Common
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; set; }
        private IEnumerable<TElement> List { get; set; }

        public Grouping(TKey key, IEnumerable<TElement> list)
        {
            Key = key;
            List = list;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IGrouping<TKey, TElement> AsGrouping()
        {
            return this;
        }
    }
}
