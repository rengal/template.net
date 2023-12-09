namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Заполнение строки символами (по умолчанию '-').</summary>
    public class Line : Fill
    {
        public Line(string symbols) : base(TagDoc.LINE_TAG, symbols) { }

        public Line() : this("-") { }
    }
}
