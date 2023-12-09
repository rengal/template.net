using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Набор ячеек таблицы. Ячейки задаются слева направо, сверху вниз.</summary>
    public sealed class Cells : XElement
    {
        public Cells() : base(TagTable.CellsTag) { }

        public Cells(params Cell[] cells) : base(TagTable.CellsTag, cells) { }

        /// TODO избавиться или поставить проверку типа
        public Cells(params object[] cells) : base(TagTable.CellsTag, cells) { }
    }
}