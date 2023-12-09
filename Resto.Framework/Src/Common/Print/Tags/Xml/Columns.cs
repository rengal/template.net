using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Столбцы таблицы.</summary>
    public sealed class Columns : XElement
    {
        public Columns(params ColumnBase[] content) : base(TagTable.ColumnsTag, content) { }
    }
}
