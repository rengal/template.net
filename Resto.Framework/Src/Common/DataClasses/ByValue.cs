using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    //This class is used to serialize entity by value
    public sealed class ByValue<T> where T : CachedEntity
    {
        private readonly T value;

        public ByValue([NotNull] T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.value = value;
        }

        public T Value
        {
            get { return value; }
        }
    }
}