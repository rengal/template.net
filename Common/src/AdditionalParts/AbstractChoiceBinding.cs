using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class AbstractChoiceBinding
    {
        private static readonly ChoiceBindingByModifierEqualityComparer ByModifierEqualityComparer
            = new ChoiceBindingByModifierEqualityComparer();

        public override string ToString()
        {
            return modifier.ToString();
        }

        public bool IsGroupModifier
        {
            get { return modifier is ProductGroup; }
        }

        public bool IsSimpleModifier
        {
            get { return modifier is Product; }
        }

        /// <summary>
        /// Возвращает <c>true</c>, если модификатор переданной привязки эквивалентен модификатору текущей привязки.
        /// </summary>
        /// <param name="other">Привязка, с которой осуществляется сравнение.</param>
        public bool IsSameAs(AbstractChoiceBinding other)
        {
            return ByModifierEqualityComparer.Equals(this, other);
        }

        [CanBeNull]
        public TChoiceBinding GetSameBinding<TChoiceBinding>([NotNull] IEnumerable<TChoiceBinding> bindings)
            where TChoiceBinding : AbstractChoiceBinding
        {
            if (bindings == null)
            {
                throw new ArgumentNullException(nameof(bindings));
            }

            return bindings.FirstOrDefault(IsSameAs);
        }

        private class ChoiceBindingByModifierEqualityComparer : IEqualityComparer<AbstractChoiceBinding>
        {
            public bool Equals(AbstractChoiceBinding x, AbstractChoiceBinding y)
            {
                if (x == y)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return Equals(x.Modifier, y.Modifier);
            }

            public int GetHashCode(AbstractChoiceBinding obj)
            {
                return obj == null ? 0 : obj.Modifier.GetHashCode();
            }
        }
    }
}
