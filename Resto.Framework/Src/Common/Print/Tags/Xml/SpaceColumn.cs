using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Столбец таблицы.</summary>
    public sealed class Column : ColumnBase
    {
        public Column([NotNull] params XAttribute[] attributes)
            : base(TagTable.ColumnTag, attributes)
        { }
    }
}