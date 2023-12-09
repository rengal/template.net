using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Толщина пустой границы между ячейками.</summary>
    public class CellSpacingAttr : XAttribute
    {
        public CellSpacingAttr(int value) : base(TagTable.CellspacingAttr, value) { }
    }
}
