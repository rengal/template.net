using System.Collections.Generic;
using System.Xml;

using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// <summary>
    /// Базовый интерфейс для всех тэгов.
    /// </summary>
    /// <remarks>
    /// Тэги отвечают за выставление правил форматирования, настройки ленты, запись текста на ленту, а также могут сами выполнять форматирование. 
    /// </remarks>
    public interface ITag
    {
        /// <summary>
        /// Возвращает имя тэга
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Форматировует элемент Xml-дерева и записывает результат на виртуальную ленту
        /// </summary>
        /// <param name="tape">Виртуальная лента</param>
        /// <param name="node">Обрабатываемое Xml-дерево (должно соответствовать Doc.xsd)</param>
        /// <param name="tags">Словарь всех тэгов, доступных для форматирования дочерних элементов <paramref name="node"/> (в качестве ключа используется имя тэга)</param>
        /// <remarks>
        /// Метод вызывается в случае, если имя xml-тэга <paramref name="node"/> совпадает со значением свойства <seealso cref="Name"/>.
        /// </remarks>
        void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags);
    }
}
