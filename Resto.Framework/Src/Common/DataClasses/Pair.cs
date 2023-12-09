using System;

namespace Resto.Framework.Data
{
    [DataClass("Pair")]
    public sealed class Pair
    {
        private Object first;
        private Object second;

        public Pair() { }

        public Pair(Object first, Object second)
        {
            this.first = first;
            this.second = second;
        }

        public Object First
        {
            get { return first; }
            set { first = value; }
        }

        public Object Second
        {
            get { return second; }
            set { second = value; }
        }

    }
}