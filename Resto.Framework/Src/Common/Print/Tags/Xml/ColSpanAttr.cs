using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Количество столбцов, которое занимает ячейка в таблице. Если количество равно 0, то ячейка занимает все оставшиеся столбцы в строке таблицы.</summary>
    public class ColSpanAttr : XAttribute
    {
        public ColSpanAttr(int value) : base(TagTable.ColspanAttr, value) { }
    }
}
