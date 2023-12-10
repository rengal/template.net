using System;
using System.Linq;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ChoiceBinding : ICloneable
    {
        [Transient]
        public int ForAll = 1;

        public object Clone()
        {
            var result = (ChoiceBinding)MemberwiseClone();

            if (ChildModifiers != null)
            {
                result.ChildModifiers = ChildModifiers.Select(m => m.Copy()).ToList();
            }

            return result;
        }

        /// <summary>
        /// Возвращает глубокую копию текущего объекта, с учетом <see cref="ChildModifiers"/>.
        /// </summary>
        public ChoiceBinding Copy()
        {
            return (ChoiceBinding)Clone();
        }
    }
}