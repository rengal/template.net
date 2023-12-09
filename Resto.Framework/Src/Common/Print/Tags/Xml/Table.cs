using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Таблица с ячейками, расположенными в столбцах и строках.</summary>
    public sealed class Table : XElement
    {
        public Table(Columns collumns, Cells cells, params XAttribute[] attributes)
            : base(TagTable.Instance.Name, attributes, collumns, cells) { }
    }
}
