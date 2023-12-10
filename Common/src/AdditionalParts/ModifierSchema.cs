using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class ModifierSchema
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор схемы.</param>
        /// <param name="name">Наименование схемы.</param>
        /// <param name="splittableProduct">Признак "Блюдо можно разделить".</param>
        /// <param name="modifiers">Список модификаторов.</param>
        public ModifierSchema(Guid id, LocalizableValue name, bool splittableProduct, List<ChoiceBinding> modifiers)
            : this(id, name, splittableProduct)
        {
            this.modifiers = modifiers;
        }


        /// <summary>
        /// Возвращает наименование шкалы размеров, привязанной к схеме.
        /// </summary>
        [CanBeNull]
        public string ProductScaleName
        {
            get { return ProductScale != null ? ProductScale.NameLocal : null; }
        }

        public override string ToString()
        {
            return string.Format("Modifier schema '{0}': {1}", NameLocal, base.ToString());
        }
    }
}
