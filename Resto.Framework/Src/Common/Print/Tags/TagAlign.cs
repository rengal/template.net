using System.Collections.Generic;
using System.Xml;

using Resto.Framework.Common.Print.Alignment;
using Resto.Framework.Common.Print.Tags.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// <summary>
    /// Тэг, задающий параметры выравнивания строк на ленте
    /// </summary>
    public sealed class TagAlign : TagBase
    {
        /// <summary>
        /// Задает выравнивание "по центру"
        /// </summary>
        /// <remarks>
        /// Выравнивание "по центру" обеспечивается за счет вставки дополнительных пробелов в начало строки. Если разместить текст ровно по центру невозможно, он будет смещен влево (т.е. справа будет на один пробел больше)
        /// </remarks>
        public readonly static TagAlign Center;
        /// <summary>
        /// Задает выравнивание "с растяжением"
        /// </summary>
        /// <remarks>
        /// Выравнивание "с растяжением" обеспечивается за счет вставки дополнительных пробелов между словами. При этом, если невозможно вставить между всеми словами одинаковое количество пробелов, то более разреженным будет конец строки.
        /// </remarks>
        public readonly static TagAlign Justify;
        /// <summary>
        /// Задает выравнивание "по левому краю"
        /// </summary>
        public readonly static TagAlign Left;
        /// <summary>
        /// Задает выравнивание "по правому краю"
        /// </summary>
        public readonly static TagAlign Right;

        private readonly IAlign Align;

        static TagAlign()
        {
            Center = new TagAlign(AlignAttrValue.Center.GetName(), AlignCenter.Instance);
            Justify = new TagAlign(AlignAttrValue.Justify.GetName(), AlignJustify.Instance);
            Left = new TagAlign(AlignAttrValue.Left.GetName(), AlignLeft.Instance);
            Right = new TagAlign(AlignAttrValue.Right.GetName(), AlignRight.Instance);
        }

        private TagAlign(string name, IAlign align)
            : base(name)
        {
            Align = align;
        }

        /// <summary>
        /// Задает выравнивание текста, которое будет использовано при форматировании дочерних элементов <paramref name="node"/>
        /// </summary>
        /// <param name="tape">Виртуальная лента</param>
        /// <param name="node">Обрабатываемое Xml-дерево, корнем которого могут быть элементы left, center, right и justify</param>
        /// <param name="tags">Словарь всех тэгов, доступных для форматирования дочерних элементов <paramref name="node"/> (в качестве ключа используется имя тэга)</param>
        /// <remarks>
        /// Для выравнивания текста виртуальная лента использует один из объектов, реализующих интерфейс IAlign.
        /// </remarks>
        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var oldAlign = tape.Align;
            tape.SetAlign(Align);
            base.Format(tape, node, tags);
            tape.SetAlign(oldAlign);
        }
    }
}
