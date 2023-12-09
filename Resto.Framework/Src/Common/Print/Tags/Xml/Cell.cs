using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Ячейка таблицы.</summary>
    public class Cell : XElement
    {
        protected Cell(string name, params object[] content) : base(name, content) { }

        public Cell(object content) : base(TagTable.CellTag, content) { }

        public Cell(params object[] content) : base(TagTable.CellTag, content) { }
    }
}
