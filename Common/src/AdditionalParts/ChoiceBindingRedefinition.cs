using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Представляет переопределение привязки модификаторов, указанной в назначенной продукту схеме модификаторов.
    /// </summary>
    public partial class ChoiceBindingRedefinition
    {
        /// <summary>
        /// Применяет переопределение схемы модификаторов к переданной привязке модификатора.
        /// Привязка будет безвозвратно изменена, поэтому перед применением может потребоваться ее склонировать.
        /// </summary>
        /// <param name="choiceBinding">Привязка модификатора из схемы модификаторов.</param>
        public void ApplyTo([NotNull] ChoiceBinding choiceBinding)
        {
            if (!IsSameAs(choiceBinding))
            {
                throw new RestoException(
                    string.Format(
                        "Modifier choice binding Redefinition {0} cannot be applied to modifier choice binding {1}.",
                        this, choiceBinding));
            }

            choiceBinding.DefaultAmount = DefaultAmount;
            choiceBinding.FreeOfChargeAmount = FreeOfChargeAmount;

            if (MinimumAmount != null)
            {
                choiceBinding.MinimumAmount = MinimumAmount.Value;
            }
            if (MaximumAmount != null)
            {
                choiceBinding.MaximumAmount = MaximumAmount.Value;
            }

            if (ChildModifiers == null || choiceBinding.ChildModifiers == null)
            {
                return;
            }

            foreach (var childBinding in choiceBinding.ChildModifiers)
            {
                var childRedefinition = childBinding.GetSameBinding(ChildModifiers);
                if (childRedefinition == null)
                {
                    continue;
                }

                childRedefinition.ApplyTo(childBinding);
            }
        }
    }
}
